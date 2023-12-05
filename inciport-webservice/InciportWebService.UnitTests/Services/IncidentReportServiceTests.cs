using AutoFixture;
using FluentAssertions;
using InciportWebService.Application;
using InciportWebService.Data;
using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InciportWebService.UnitTests.Services {

  public class IncidentReportServiceTests : IDisposable {
    private DbContextOptions<ApplicationDbContext> _options;
    private ApplicationDbContext _dbContext;
    private CategoriesService _categoryService;
    private WorkerTeamsService _workerTeamsService;
    private IncidentReportService _incidentReportService;
    private Fixture _fixture;
    private IncidentReportEntity _incidentReport;
    private MainCategory _mainCategory;
    private ChosenMainCategoryEntity _chosenMainCategoryEntity;
    private IImageSaver _imageSaver;
    private const int MUNICIPALITYID = 1;

    public IncidentReportServiceTests() {
      _options = new DbContextOptionsBuilder<ApplicationDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
      _dbContext = new ApplicationDbContext(_options);

      _categoryService = new CategoriesService(_dbContext);
      _workerTeamsService = new WorkerTeamsService(_dbContext);

      _incidentReportService = new IncidentReportService(_dbContext, _categoryService, _workerTeamsService, _imageSaver);

      Random random = new Random();

      _fixture = new Fixture();
      _fixture.Register(() => new ChosenMainCategoryEntity { MainCategoryId = 1, SubCategoryId = 2 });
      _fixture.Register(() => new WorkerTeam("Team1") { IsArchived = false });
      var workerTeam = _fixture.Create<WorkerTeam>();
      _fixture.Register(() => new Coordinates(random.Next(-90, 91), random.Next(-180, 181)));
      _fixture.Register(() => new Category { IsArchived = false });
      Category subCategory1 = _fixture.Create<Category>();
      List<Category> subCategories = new List<Category> { subCategory1 };
      _fixture.Register(() => new MainCategory { IsArchived = false, SubCategories = subCategories });
      _mainCategory = _fixture.Create<MainCategory>();
      _chosenMainCategoryEntity = _fixture.Create<ChosenMainCategoryEntity>();
      _fixture.Register(() => new IncidentReportEntity { ChosenMainCategoryEntity = _chosenMainCategoryEntity, Location = _fixture.Create<Location>() });
      _incidentReport = _fixture.Create<IncidentReportEntity>();
      var incidentReports = new List<IncidentReportEntity> { _incidentReport };
      var workerTeams = new List<WorkerTeam> { _fixture.Create<WorkerTeam>() };
      var mainCategories = new List<MainCategory> { _fixture.Create<MainCategory>() };
      var municipality = new MunicipalityEntity { Id = MUNICIPALITYID, Name = "Test", MainCategories = mainCategories, WorkerTeams = workerTeams, IncidentReports = incidentReports };

      _dbContext.Add(municipality);
      _dbContext.SaveChanges();
    }

    public void Dispose() {
      _dbContext.Database.EnsureDeleted();
      _dbContext.Dispose();
    }

    [Fact]
    public async Task GetIncidentReportAsync_WhereStatusNotArchvied_ReturnsCorrectReport() {
      //ARRANGE
      _incidentReport.Status = ReportStatus.Recieved;
      _dbContext.SaveChanges();

      var expected = expectedResultHelper(_incidentReport);

      //ACT
      var actual = await _incidentReportService.GetIncidentReportAsync(MUNICIPALITYID, 1);

      //ASSERT
      actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetIncidentReportsAsync_WhereStatusNotArchived_ReturnsAllReports() {
      //ARRANGE
      _incidentReport.Status = ReportStatus.Recieved;
      _dbContext.SaveChanges();
      var report = expectedResultHelper(_incidentReport);
      var expected = new List<IncidentReport> { report };
      //ACT
      var actual = await _incidentReportService.GetIncidentReportsAsync(MUNICIPALITYID);

      //ASSERT
      actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CreateIncidentReportAsync_WithValidInput_GetsCreated() {
      //ARRANGE
      var dto = new ChosenMainCategoryDto { Id = 1, ChosenSubCategory = new ChosenSubCategoryDto { Id = 2 } };
      var reportToBeCreated = new CreateIncidentReportDto {
        ChosenMainCategory = dto,
        ContactInformation = ContactInformationDto.FromModel(_incidentReport.ContactInformation),
        Location = LocationDto.FromModel(_incidentReport.Location),
        Description = _incidentReport.Description
      };

      ChosenMainCategory cat = new ChosenMainCategory {
        Id = 1,
        Title = _mainCategory.Title,
        IsArchived = _mainCategory.IsArchived,
        ChosenSubCategory = new Category {
          Id = 2,
          IsArchived = _mainCategory.SubCategories.First().IsArchived,
          Title = _mainCategory.SubCategories.First().Title
        }
      };

      var expected = _incidentReport;

      //ACT
      await _incidentReportService.CreateIncidentReportAsync(MUNICIPALITYID, reportToBeCreated);
      var actualReportSaved = _dbContext.Municipalities.Find(MUNICIPALITYID).IncidentReports.Skip(1).First();

      expected.TimestampCreatedUtc = actualReportSaved.TimestampCreatedUtc;
      expected.TimestampLastModifiedUtc = actualReportSaved.TimestampLastModifiedUtc;
      expected.Id = actualReportSaved.Id;
      expected.Location.Id = actualReportSaved.Location.Id;

      //ASSERT
      actualReportSaved.Should().BeEquivalentTo(expected, option => option.Excluding(x => x.Location.Address.Id));
    }

    [Fact]
    public async Task UpdateIncidentReportAsync_WithChangedStatus_ReturnsUpdatedReport() {
      //ARRANGE

      UpdateIncidentReportDto reportToUpdate = new UpdateIncidentReportDto {
        Id = 1,
        Status = ReportStatus.InProgress,
        Location = LocationDto.FromModel(_incidentReport.Location),
        AssignedTeam = GetWorkerTeamDto.FromModel(_incidentReport.AssignedTeam),
        ChosenMainCategory = new ChosenMainCategoryDto {
          Id = _incidentReport.ChosenMainCategoryEntity.MainCategoryId,
          ChosenSubCategory = new ChosenSubCategoryDto { Id = _incidentReport.ChosenMainCategoryEntity.SubCategoryId },
        },
        ContactInformation = ContactInformationDto.FromModel(_incidentReport.ContactInformation),
        Description = _incidentReport.Description
      };
      var expected = expectedResultHelper(_incidentReport);
      expected.Status = ReportStatus.InProgress;

      //ACT
      var actualUpdatedIncidentReport = await _incidentReportService.UpdateIncidentReportAsync(MUNICIPALITYID, 1, reportToUpdate);
      expected.TimestampLastModifiedUtc = actualUpdatedIncidentReport.TimestampLastModifiedUtc;
      //ASSERT
      actualUpdatedIncidentReport.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task DeleteIncidentReportAsync_WhereReportExistsAndCorrectId_GetsArchivedCorrectly() {
      //ARRANGE

      //ACT
      await _incidentReportService.DeleteIncidentReportAsync(MUNICIPALITYID, 1);

      //ASSERT
      Assert.Empty(_dbContext.Municipalities.First().IncidentReports);
    }

    private IncidentReport expectedResultHelper(IncidentReportEntity expected) {
      return new IncidentReport {
        Id = expected.Id,
        AssignedTeam = expected.AssignedTeam,
        ChosenMainCategory = new ChosenMainCategory { Id = 1, IsArchived = _mainCategory.IsArchived, Title = _mainCategory.Title, ChosenSubCategory = _mainCategory.SubCategories.First() },
        Status = expected.Status,
        ContactInformation = expected.ContactInformation,
        Description = expected.Description,
        ImageReferences = expected.ImageReferences,
        Location = expected.Location,
        TimestampCreatedUtc = expected.TimestampCreatedUtc,
        TimestampLastModifiedUtc = expected.TimestampLastModifiedUtc
      };
    }
  }
}