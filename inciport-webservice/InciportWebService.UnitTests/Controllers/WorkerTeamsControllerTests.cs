//using FluentAssertions;
//using InciportWebService.Api;
//using InciportWebService.Api.Controllers;
//using InciportWebService.Domain;
//using InciportWebService.Infrastructure;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace InciportWebService.UnitTests {
//  public class WorkerTeamsControllerTests : IDisposable {
//    private readonly DbContextOptions<ApplicationDbContext> _options;
//    private readonly ApplicationDbContext _dbContext;
//    private readonly WorkerTeamsController _workerTeamsController;

//    public WorkerTeamsControllerTests() {
//      // XXX: Centralize db code somewhere

//      // ARRANGE
//      //Create in memory database
//      _options = new DbContextOptionsBuilder<ApplicationDbContext>()
//      .UseInMemoryDatabase(databaseName: "inciport")
//      .Options;
//      _dbContext = new ApplicationDbContext(_options); // Creates a new db context for each test.
//                                                       // Create default controller for all tests to use if no furter setup required (such as a controller context)

//      _workerTeamsController = new WorkerTeamsController(_dbContext, null);
//    }

//    // Clean up db between each test - Else will use same db for all tests!
//    public void Dispose() {
//      // Remove from memory
//      _dbContext.Database.EnsureDeleted();
//      _dbContext.Dispose();
//    }

//    [Fact]
//    public async Task UpdateWorkerTeam_WithTeamArchived_ReturnsNotFound() {
//      const int ID = 2;
//      // ARRANGE
//      Seed_WithArchived();

//      // ACT
//      GetWorkerTeamDto inputTeam = new GetWorkerTeamDto { Id = ID, Name = "Updated team" };
//      IActionResult result = await _workerTeamsController.UpdateWorkerTeam(ID, inputTeam);

//      //ASSERT
//      Assert.IsType<NotFoundResult>(result);
//    }

//    [Fact]
//    public async Task UpdateWorkerTeam_WithTeamNotExists_ReturnsNotFound() {
//      const int MUNICIPALITY_ID = 1;
//      const int INCIDENT_REPORT_ID = 99;

//      // ARRANGE
//      Seed_WithNoArchived(); // XXX: Vel ikke nødvendigt??

//      // ACT
//      GetWorkerTeamDto inputTeam = new GetWorkerTeamDto { Id = INCIDENT_REPORT_ID, Name = "Updated team" };
//      IActionResult result = await _workerTeamsController.UpdateWorkerTeam(MUNICIPALITY_ID, INCIDENT_REPORT_ID, inputTeam);

//      //ASSERT
//      Assert.IsType<NotFoundResult>(result);
//    }

//    [Fact]
//    public async Task UpdateWorkerTeam_WithValidUpdate_UpdatesTeam() {
//      const int ID = 1;
//      // ARRANGE
//      Seed_WithNoArchived();
//      WorkerTeam expectedUpdatedTeamSaved = new WorkerTeam { Id = ID, Name = "Updated team", IsArchived = false };
//      GetWorkerTeamDto inputTeam = new GetWorkerTeamDto { Id = ID, Name = "Updated team" };

//      // ACT
//      _ = await _workerTeamsController.UpdateWorkerTeam(ID, inputTeam) as OkObjectResult;
//      WorkerTeam actualUpdatedTeamSaved = _dbContext.WorkerTeams.Find(ID);

//      //ASSERT
//      actualUpdatedTeamSaved.Should().BeEquivalentTo(expectedUpdatedTeamSaved);
//    }

//    [Fact]
//    public async Task UpdateWorkerTeam_WithValidUpdate_ReturnsCorrectResponse() {
//      const int ID = 1;
//      // ARRANGE
//      Seed_WithNoArchived();
//      GetWorkerTeamDto expectedUpdatedTeamDto = new GetWorkerTeamDto { Id = ID, Name = "Updated team" };
//      GetWorkerTeamDto inputTeam = new GetWorkerTeamDto { Id = ID, Name = "Updated team" };

//      // ACT
//      OkObjectResult actualResult = await _workerTeamsController.UpdateWorkerTeam(ID, inputTeam) as OkObjectResult;
//      GetWorkerTeamDto actualUpdatedTeamDto = actualResult.Value as GetWorkerTeamDto;

//      //ASSERT
//      Assert.NotNull(actualResult); // Is null if unexpected return object.
//      actualUpdatedTeamDto.Should().BeEquivalentTo(expectedUpdatedTeamDto);
//    }

//    [Fact]
//    public async Task UpdateWorkerTeam_WithDifferentIds_ReturnsStatusBadRequest() {
//      const int ID1 = 1;
//      const int ID2 = 2;
//      // ARRANGE
//      //Seed_WithNoArchived();
//      //GetWorkerTeamDto expectedTeam = new GetWorkerTeamDto { Id = ID, Name = "Updated team" };
//      GetWorkerTeamDto inputTeam = new GetWorkerTeamDto { Id = ID1, Name = "Updated team" };

//      // ACT
//      BadRequestObjectResult actualResult = await _workerTeamsController.UpdateWorkerTeam(ID2, inputTeam) as BadRequestObjectResult;
//      string actualErrorMessage = actualResult.Value as string;

//      //ASSERT
//      Assert.NotNull(actualResult); // Is null if unexpected return object.
//                                    // Assert that error message mentions conflicting ids.
//      actualErrorMessage.Should().Contain(ID1.ToString());
//      actualErrorMessage.Should().Contain(ID2.ToString());
//    }

//    [Fact]
//    public async Task CreateWorkerTeam_ReturnsStatusCreatedWithCorrectDto() {
//      // ARRANGE
//      const int ID = 1;
//      WorkerTeamsController controller = CreateWorkerTeamsController(_dbContext);

//      string expectedLocationString = $"http://baseurl/path/{ID}";
//      GetWorkerTeamDto expectedTeamDtoReturned = new GetWorkerTeamDto { Id = ID, Name = "New Team" };
//      CreateWorkerTeamDto inputTeam = new CreateWorkerTeamDto { Name = "New Team" };

//      // ACT
//      CreatedResult actualResult = await controller.CreateWorkerTeam(inputTeam) as CreatedResult;
//      string actualLocationString = actualResult.Location;
//      GetWorkerTeamDto actualTeamDtoReturned = actualResult.Value as GetWorkerTeamDto;

//      //ASSERT
//      Assert.NotNull(actualResult); // Is null if unexpected return object.
//      Assert.Equal(expectedLocationString, actualLocationString);
//      actualTeamDtoReturned.Should().BeEquivalentTo(expectedTeamDtoReturned);
//    }

//    [Fact]
//    public async Task CreateWorkerTeam_GetsCreated() {
//      // ARRANGE
//      const int ID = 1;
//      WorkerTeamsController controller = CreateWorkerTeamsController(_dbContext);

//      WorkerTeam expectedTeamModelSaved = new WorkerTeam { Id = ID, IsArchived = false, Name = "New Team" };
//      CreateWorkerTeamDto inputTeam = new CreateWorkerTeamDto { Name = "New Team" };

//      // ACT
//      _ = await controller.CreateWorkerTeam(1, 1, inputTeam) as CreatedResult;
//      WorkerTeam actualTeamModelSaved = _dbContext.WorkerTeams.Find(ID);

//      //ASSERT
//      actualTeamModelSaved.Should().BeEquivalentTo(expectedTeamModelSaved);
//    }

//    private WorkerTeamsController CreateWorkerTeamsController(ApplicationDbContext dbContext) {
//      DefaultHttpContext httpContext = new DefaultHttpContext();
//      httpContext.Request.Host = new HostString("host");
//      httpContext.Request.Path = new PathString("/path");

//      ControllerContext controllerContext = new ControllerContext() {
//        HttpContext = httpContext,
//      };

//      //Arrange
//      Dictionary<string, string> configurationContext = new Dictionary<string, string> {
//    {"BaseUrl", "http://baseurl"},
//    };

//      IConfiguration configuration = new ConfigurationBuilder()
//          .AddInMemoryCollection(configurationContext)
//          .Build();

//      WorkerTeamsController controller = new WorkerTeamsController(dbContext, null) {
//        ControllerContext = controllerContext
//      };
//      return controller;
//    }

//    [Fact]
//    public async Task DeleteWorkerTeam_WithTeamNotExists_ReturnsNotFound() {
//      const int ID = 99;
//      // ARRANGE
//      Seed_WithNoArchived();

//      // ACT
//      IActionResult result = await _workerTeamsController.DeleteWorkerTeam(ID);

//      //ASSERT
//      Assert.IsAssignableFrom<NotFoundResult>(result);
//    }

//    [Fact]
//    public async Task DeleteWorkerTeam_WithTeamExists_GetsArchived() {
//      // ARRANGE
//      const int MUNICIPALITY_ID = 1;
//      const int WORKER_TEAM_ID = 1;
//      WorkerTeam teamInDb = new WorkerTeam(id: WORKER_TEAM_ID, name: "Team B", isArchived: false);
//      _dbContext.WorkerTeams.Add(teamInDb);
//      _dbContext.SaveChanges();

//      WorkerTeam expectedTeam = new WorkerTeam(id: WORKER_TEAM_ID, name: "Team B", isArchived: true);

//      // ACT
//      NoContentResult actualResult = await _workerTeamsController.DeleteWorkerTeam(MUNICIPALITY_ID, WORKER_TEAM_ID) as NoContentResult;
//      WorkerTeam actualTeam = _dbContext.WorkerTeams.Find(WORKER_TEAM_ID);

//      //ASSERT
//      Assert.NotNull(actualResult); // Is null if unexpected return object.
//      actualTeam.Should().BeEquivalentTo(expectedTeam);
//    }

//    [Fact]
//    public async Task GetWorkerTeam_WithTeamExists_ReturnsCorrectTeam() {
//      const int ID = 2;
//      // ARRANGE
//      Seed_WithNoArchived();
//      GetWorkerTeamDto expectedTeam = new GetWorkerTeamDto { Id = ID, Name = "Team B" };

//      // ACT
//      OkObjectResult result = await _workerTeamsController.GetWorkerTeams(ID) as OkObjectResult;
//      GetWorkerTeamDto actualTeam = result.Value as GetWorkerTeamDto;

//      //ASSERT
//      Assert.NotNull(result); // Is null if unexpected return object.
//      actualTeam.Should().BeEquivalentTo(expectedTeam);
//    }

//    [Fact]
//    public async Task GetWorkerTeam_WithTeamNotExists_ReturnsNotFound() {
//      const int ID = 99;
//      // ARRANGE
//      Seed_WithNoArchived();

//      // ACT
//      IActionResult result = await _workerTeamsController.GetWorkerTeam(ID);

//      //ASSERT
//      Assert.IsAssignableFrom<NotFoundResult>(result);
//    }

//    [Fact]
//    public async Task GetWorkerTeam_WithTeamArchived_ReturnsNotFound() {
//      const int ID = 2;
//      // ARRANGE
//      Seed_WithArchived();

//      // ACT
//      IActionResult result = await _workerTeamsController.GetWorkerTeam(ID);

//      //ASSERT
//      Assert.IsAssignableFrom<NotFoundResult>(result);
//    }

//    [Fact]
//    public async Task GetWorkerTeams_WithNoArchived_ReturnsAll() {
//      // ARRANGE
//      Seed_WithNoArchived();
//      List<GetWorkerTeamDto> expectedTeams = CreateExpectedTeams_Get_WithNoArchived_ReturnsAll();

//      // ACT
//      OkObjectResult result = await _workerTeamsController.GetWorkerTeams() as OkObjectResult;
//      List<GetWorkerTeamDto> actualTeams = result.Value as List<GetWorkerTeamDto>;

//      //ASSERT
//      Assert.NotNull(result); // Is null if unexpected return object.
//      actualTeams.Should().BeEquivalentTo(expectedTeams);
//    }

//    [Fact]
//    public async Task GetWorkerTeams_WithNoTeams_ReturnsEmptyList() {
//      // ARRANGE
//      //Seed_Get_WithArchived_ReturnsOnlyNotArchived();
//      List<GetWorkerTeamDto> expectedTeams = new List<GetWorkerTeamDto>();

//      // ACT
//      OkObjectResult actualResult = await _workerTeamsController.GetWorkerTeams() as OkObjectResult;
//      List<GetWorkerTeamDto> actualTeams = actualResult.Value as List<GetWorkerTeamDto>;

//      //ASSERT
//      Assert.NotNull(actualResult); // Is null if unexpected return object.
//      actualTeams.Should().BeEquivalentTo(expectedTeams);
//    }

//    [Fact]
//    public async Task GetWorkerTeams_WithArchived_ReturnsOnlyNotArchived() {
//      // ARRANGE
//      Seed_WithArchived();
//      List<GetWorkerTeamDto> expectedTeams = CreateExpectedTeams_Get_WithArchived_ReturnsOnlyNotArchived();

//      // ACT
//      OkObjectResult actualResult = await _workerTeamsController.GetWorkerTeams() as OkObjectResult;
//      List<GetWorkerTeamDto> actualTeams = actualResult.Value as List<GetWorkerTeamDto>;

//      //ASSERT
//      Assert.NotNull(actualResult); // Is null if unexpected return object.
//      actualTeams.Should().BeEquivalentTo(expectedTeams);
//    }

//    private static List<GetWorkerTeamDto> CreateExpectedTeams_Get_WithArchived_ReturnsOnlyNotArchived() {
//      GetWorkerTeamDto team1 = new GetWorkerTeamDto {
//        Id = 1,
//        Name = "Team A",
//      };
//      return new List<GetWorkerTeamDto>() { team1 };
//    }

//    private static List<GetWorkerTeamDto> CreateExpectedTeams_Get_WithNoArchived_ReturnsAll() {
//      GetWorkerTeamDto team1 = new GetWorkerTeamDto {
//        Id = 1,
//        Name = "Team A",
//      };

//      GetWorkerTeamDto team2 = new GetWorkerTeamDto {
//        Id = 2,

//        Name = "Team B",
//      };

//      return new List<GetWorkerTeamDto>() { team1, team2 };
//    }

//    private void Seed_WithArchived() {
//      WorkerTeam team1 = new WorkerTeam {
//        Name = "Team A",
//        IsArchived = false,
//        //Municipality = "Aalborg"
//      };

//      WorkerTeam team2 = new WorkerTeam {
//        Name = "Team B",
//        IsArchived = true, // Archived!
//        //Municipality = "Aalborg"
//      };

//      List<WorkerTeam> teams = new List<WorkerTeam>() { team1, team2, };
//      _dbContext.WorkerTeams.AddRange(teams);
//      _dbContext.SaveChanges();
//    }

//    private void Seed_WithNoArchived() {
//      WorkerTeam team1 = new WorkerTeam {
//        Name = "Team A",
//        IsArchived = false,
//        //Municipality = "Aalborg"
//      };

//      WorkerTeam team2 = new WorkerTeam {
//        Name = "Team B",
//        IsArchived = false,
//        //Municipality = "Aalborg"
//      };

//      List<WorkerTeam> teams = new List<WorkerTeam>() { team1, team2, };
//      _dbContext.WorkerTeams.AddRange(teams);
//      _dbContext.SaveChanges();
//    }
//  }
//}