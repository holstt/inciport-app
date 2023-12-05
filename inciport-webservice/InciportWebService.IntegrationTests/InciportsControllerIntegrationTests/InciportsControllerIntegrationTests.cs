using InciportWebService.Api;
using InciportWebService.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using FluentAssertions;
using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.TestHost;
using InciportWebService.Application;

namespace InciportWebService.IntegrationTests.InciportsControllerIntegrationTests {

  public class InciportsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>> {
    // Valid auth tokens

    private readonly WebApplicationFactory<Startup> _clientFactory;
    private readonly ITestOutputHelper _output; // Use this to console print from tests instead of CW.
    private readonly HttpClient _client;
    private TestDbIntializer _testdata;

    //private readonly HttpClient _client;
    private const string ENV_VARIABLE_HOST = "HOST";
    private const string ENV_VARIABLE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

    public InciportsControllerIntegrationTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output) {
      Environment.SetEnvironmentVariable(ENV_VARIABLE_ENVIRONMENT, "Integration");
      Environment.SetEnvironmentVariable(ENV_VARIABLE_HOST, "Localhost");

      // ARRANGE
      _clientFactory = factory;
      _output = output;
      _client = CreateClient(); // Recreate client for every test.
    }

    private HttpClient CreateClient() {
      return _clientFactory.WithWebHostBuilder(builder => {
        // Configure services AFTER regular .ConfigureServices
        builder.ConfigureTestServices(async services => {
          // Remove org. db context.
          ServiceDescriptor descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
          services.Remove(descriptor);
          //Create in memory database that resembles the real db, but is much faster to recreate between each test.
          services.AddDbContext<ApplicationDbContext>(options => {
            options.UseInMemoryDatabase(databaseName: "inciport").UseLazyLoadingProxies();
          });

          ServiceProvider serviceProvider = services.BuildServiceProvider();

          using (IServiceScope scope = serviceProvider.CreateScope()) {
            IServiceProvider scopedServices = scope.ServiceProvider;
            // Reset db to start from a clean slate before running integration tests.
            ApplicationDbContext dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
            await dbContext.ResetDbAsync(); // Ensure clean db.
            _testdata = new TestDbIntializer(serviceProvider);
            await _testdata.InitializeTestDbAsync();
          }
        });
      }).CreateClient();
    }

    [Fact]
    public async Task DeleteIncidentReport_AsAuthorizedMaintainer_ReturnsStatusNoContent() {
      // Arrange
      const int ID = 1;
      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testdata.MaintainerRoleLoginToken);
      // Act
      HttpResponseMessage actualResponse = await _client.DeleteAsync($"api/municipalities/{ID}/inciports/{ID}");

      // Assert
      Assert.Equal(HttpStatusCode.NoContent, actualResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteIncidentReport_AsAuthorizedAdmin_ReturnsNoContent() {
      // Arrange
      const int municipalityId = 1;

      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testdata.AdminRoleLoginToken);
      // Act
      HttpResponseMessage actualResponse = await _client.DeleteAsync($"api/municipalities/{municipalityId}/inciports/{municipalityId}");

      // Assert
      Assert.Equal(HttpStatusCode.NoContent, actualResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteIncidentReport_AsAuthorizedManager_ReturnsStatusNoContent() {
      // Arrange
      const int MUNICIPALITY_ID = 1;
      const int INCIDENT_REPORT_ID = 1;
      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testdata.ManagerRoleLoginToken);

      // Act
      HttpResponseMessage actualResponse = await _client.DeleteAsync($"api/municipalities/{MUNICIPALITY_ID}/inciports/{INCIDENT_REPORT_ID}");

      // Assert
      Assert.Equal(HttpStatusCode.NoContent, actualResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteIncidentReport_AsUnauthorized_ReturnsUnauthorizedResult() {
      // Arrange
      int municipalityId = 1;
      string url = $"api/municipalities/{municipalityId}/inciports/{municipalityId}";
      HttpClient client = CreateClient();
      HttpStatusCode expected = HttpStatusCode.Unauthorized;

      // Act
      HttpResponseMessage actual = await client.DeleteAsync(url);

      // Assert
      Assert.Equal(expected, actual.StatusCode);
    }

    [Fact]
    public async Task PostIncidentReports_ReturnsStatusCreated() {
      int municipalityId = 1;

      // Arrange
      var expectedStatusCode = HttpStatusCode.Created;
      CreateIncidentReportDto inputDto = CreatePostDto();
      // Act
      HttpResponseMessage actualResponse = await CreateClient().PostAsJsonAsync($"api/municipalities/{municipalityId}/inciports", inputDto);

      // Assert
      Assert.Equal(expectedStatusCode, actualResponse.StatusCode);
    }

    private CreateIncidentReportDto CreatePostDto() {
      return new CreateIncidentReportDto {
        Description = "There's a big hole in road. Please fix it!",
        ChosenMainCategory = new ChosenMainCategoryDto {
          Id = 1,
          ChosenSubCategory = new ChosenSubCategoryDto {
            Id = 2
          }
        },
        ContactInformation = new ContactInformationDto {
          Email = "tom@jensen.dk",
          Name = "Tom Jensen",
          PhoneNumber = "52472185"
        },
        ImagesBase64 = new List<string>(),
        Location = new LocationDto() {
          Latitude = 57.044513,
          Longitude = 9.924438,
          Address = new AddressDto {
            City = "Aalborg",
            Country = "Denmark",
            Street = "Nytorv 12",
            ZipCode = "9000",
            Municipality = "Aalborg"
          }
        }
      };
    }

    [Fact]
    public async Task GetIncidentReports_WithIncidentReportsExists_ReturnsOkWithCorrectDtoCount() {
      // Arrange
      var expectedStatusCode = HttpStatusCode.OK;
      int expectedCount = 3;
      int municipalityId = 1;

      // Act
      HttpResponseMessage actualResponse = await CreateClient().GetAsync($"api/municipalities/{municipalityId}/inciports");

      // Assert
      string jsonResponse = await actualResponse.Content.ReadAsStringAsync();
      _output.WriteLine(jsonResponse);
      List<GetIncidentReportDto> actualDto = JsonConvert.DeserializeObject<List<GetIncidentReportDto>>(jsonResponse);
      Assert.Equal(expectedStatusCode, actualResponse.StatusCode);
      Assert.Equal(expectedCount, actualDto.Count);
    }

    [Fact]
    public async Task GetIncidentReport_WithReportNotExists_ReturnsNotFound() {
      // Arrange
      const int ID = 99;
      int municipalityId = 1;

      // Act
      HttpResponseMessage actualResponse = await CreateClient().GetAsync($"api/municipalities/{municipalityId}/inciports/{ID}");

      // Assert
      Assert.Equal(HttpStatusCode.NotFound, actualResponse.StatusCode);
    }

    [Fact]
    public async Task GetIncidentReport_WithReportExists_ReturnsResult() {
      // Arrange
      const int ID = 1;
      int municipalityId = 1;

      GetIncidentReportDto expectedDto = GetExpectedDto();

      // Act
      HttpResponseMessage actualResponse = await CreateClient().GetAsync($"api/municipalities/{municipalityId}/inciports/{ID}");

      string jsonResponse = await actualResponse.Content.ReadAsStringAsync();
      _output.WriteLine(jsonResponse);
      GetIncidentReportDto actualDto = JsonConvert.DeserializeObject<GetIncidentReportDto>(jsonResponse);

      // Assert
      Assert.Equal(HttpStatusCode.OK, actualResponse.StatusCode);
      actualDto.Should().BeEquivalentTo(expectedDto);
    }

    private GetIncidentReportDto GetExpectedDto() {
      return new GetIncidentReportDto {
        Id = 1,
        Status = ReportStatus.Recieved,
        Description = "There's a big hole in road. Please fix it!",
        ChosenMainCategory = new GetChosenMainCategoryDto {
          Id = 1,
          Title = "Road",
          ChosenSubCategory = new CategoryDto {
            Id = 3, // This is id 2 even though it is the first sub category, as main and sub categories is placed in same table
            Title = "Ice"
          }
        },
        AssignedTeam = new GetWorkerTeamDto {
          Id = 1,
          Name = "Team A"
        },
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date,
        TimestampModifiedUtc = DateTimeOffset.UtcNow.Date,
        ContactInformation = new ContactInformationDto {
          Email = "tom@jensen.dk",
          Name = "Tom Jensen",
          PhoneNumber = "52472185"
        },
        ImageUrls = new List<string> {
        // NB:Has no port number when using test client.
          $"http://localhost/api/municipalities/1/inciports/1/images/1",
          $"http://localhost/api/municipalities/1/inciports/1/images/2",
          $"http://localhost/api/municipalities/1/inciports/1/images/3",
        },
        Location = new LocationDto() {
          Latitude = 57.044513,
          Longitude = 9.924438,
          Address = new AddressDto {
            City = "Aalborg",
            Country = "Denmark",
            Street = "Nytorv 12",
            ZipCode = "9000",
            Municipality = "Aalborg"
          }
        }
      };
    }
  }
}