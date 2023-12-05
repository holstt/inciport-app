using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InciportWebService.Data.TestCategories;

namespace InciportWebService.IntegrationTests.InciportsControllerIntegrationTests {

  internal class TestDbIntializer {
    public string AdminRoleLoginToken { get; set; }
    public string ManagerRoleLoginToken { get; set; }
    public string MaintainerRoleLoginToken { get; private set; }

    private readonly IServiceProvider _services;
    private TestDataUserFactory _testData;
    private ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private List<MainCategory> _aalborgOptions;
    private List<MainCategory> _odenseOptions;
    private MainCategory _mainOptionWithSubCats;
    private MainCategory _mainOptionNoSubCats;
    private List<WorkerTeam> _aalborgTeams;
    private List<WorkerTeam> _odenseTeams;
    private List<IncidentReportEntity> _aalborgIncidentReports;
    private List<IncidentReportEntity> _odenseIncidentReports;
    private MunicipalityEntity _aalborg;
    private MunicipalityEntity _odense;
    private readonly AuthencicationService _userService;
    private readonly IConfiguration _configuration;

    public TestDbIntializer(IServiceProvider services) {
      _services = services ?? throw new ArgumentNullException(nameof(services));
      _testData = new TestDataUserFactory(services);
      _dbContext = _services.GetRequiredService<ApplicationDbContext>();
      _userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
      _userService = _services.GetRequiredService<AuthencicationService>();
      _configuration = _services.GetRequiredService<IConfiguration>();
    }

    public async Task InitializeTestDbAsync() {
      DbDefaultInitializer initializer = new DbDefaultInitializer(_dbContext, _services, _configuration);
      await initializer.InitializeAsync();
      Console.WriteLine("Initializing db with TEST data...");

      SeedMunicipalities();
      SeedWorkerTeams();
      SeedCategoryOptions();
      SeedAalborgIncidentReports();
      SeedOdenseIncidentReports();
      SeedUsers();
    }

    private void SeedWorkerTeams() {
      WorkerTeam team1 = new WorkerTeam("Team A", false);
      WorkerTeam team2 = new WorkerTeam("Team A", false);
      WorkerTeam team3 = new WorkerTeam("Team C", false);

      _aalborgTeams = new List<WorkerTeam>() { team1, team2, team3 };
      _aalborg.WorkerTeams.AddRange(_aalborgTeams);
      _dbContext.SaveChanges();

      WorkerTeam team4 = new WorkerTeam("Team D", true);
      WorkerTeam team5 = new WorkerTeam("Team D", false);

      List<WorkerTeam> odenseTeams = new List<WorkerTeam>() { team4, team5 };
      _odense.WorkerTeams.AddRange(odenseTeams);
      _odenseTeams = odenseTeams;
      _dbContext.SaveChanges();
    }

    private void SeedCategoryOptions() {
      _mainOptionWithSubCats = new MainCategory() {
        Title = TestCategories.Main.ROAD,
        SubCategories = new List<Category>() {
             new Category {
                Title = TestCategories.Sub.HOLE,
             },
             new Category {
               Title = TestCategories.Sub.ICE,
             },
             new Category {
                 Title = TestCategories.Sub.VEGETATION_NEAR_ROAD,
             }
           }
      };

      _mainOptionNoSubCats = new MainCategory() {
        Title = TestCategories.Main.LIGHT,
      };

      _aalborgOptions = new List<MainCategory> { _mainOptionWithSubCats, _mainOptionNoSubCats };
      _aalborg.MainCategories.AddRange(_aalborgOptions);
      _dbContext.SaveChanges();

      MainCategory mainOption3 = new MainCategory() {
        //Id = 3,
        Title = TestCategories.Main.PUBLIC_FACILITIES,
        SubCategories = new List<Category>() {
             new Category {
                //Id = 4,
                Title = TestCategories.Sub.GARBAGE,
             },
             new Category {
               //Id = 5,
               Title = TestCategories.Sub.TOILETS,
             },
             new Category {
                 //Id = 6,
                 Title = TestCategories.Sub.TABLES_AND_BENCHES,
             }
           }
      };

      _odenseOptions = new List<MainCategory> { mainOption3 };
      _odense.MainCategories.AddRange(_odenseOptions);
      _dbContext.SaveChanges();
    }

    private void SeedOdenseIncidentReports() {
      IncidentReportEntity inciport1 = new IncidentReportEntity {
        //Id = 1,
        Status = ReportStatus.Recieved,
        Description = "There's a big hole in road. Please fix it!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[0].Id),
        AssignedTeam = null,
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date,
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.Date,
        //TimeStampModifiedUtc = DateTimeOffset.UtcNow,
        ContactInformation = new ContactInformation {
          Email = "tom@jensen.dk",
          Name = "Tom Jensen",
          PhoneNumber = "52472185"
        },
        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("1")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("2")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("3")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.044513, 9.924438),
          Address = new Address(
            street: "Odensevej 12",
            city: "Odense",
            zipCode: "5000",
            country: "Denmark",
            municipality: "Odense")
        },
      };

      IncidentReportEntity inciport2 = new IncidentReportEntity {
        //Id = 2,
        Status = ReportStatus.InProgress,
        Description = "There's ice on the road. Please bring the salt!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[1].Id),
        AssignedTeam = _odenseTeams.First(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date.AddDays(-2),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.Date.AddDays(-2),
        // No contact information!
        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("5")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("6")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.048706, 9.925894),
          Address = new Address(
            street: "Odensevej 3",
            city: "Odense",
            zipCode: "5000",
            country: "Denmark",
            municipality: "Odense")
        },
      };

      IncidentReportEntity inciport3 = new IncidentReportEntity {
        //Id = 1,
        Status = ReportStatus.Completed,
        Description = "Wheres the light?!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[2].Id),
        AssignedTeam = _odenseTeams[1],
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date.AddDays(-10),
        //TimeStampModifiedUtc = DateTimeOffset.UtcNow.AddDays(-10),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.Date.AddDays(-10),
        ContactInformation = new ContactInformation {
          Email = "lars@jensen.dk",
          Name = "Lars Jensen",
          PhoneNumber = "54897536"
        },
        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("7")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.040003, 9.942636),
          Address = new Address(
            street: "Odensevej 10",
            city: "Odense",
            zipCode: "5000",
            country: "Denmark",
            municipality: "Odense")
        },
      };

      IncidentReportEntity inciport4 = new IncidentReportEntity {
        //Id = 1,
        Status = ReportStatus.Archived,
        Description = "Theres no table here!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[2].Id),
        AssignedTeam = _odenseTeams[1],
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date.AddDays(-40),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.Date.AddDays(-40),
        ContactInformation = new ContactInformation {
          Email = "hans@jensen.dk",
          Name = "Hans Jensen",
          PhoneNumber = "54897536"
        },

        ImageReferences = new List<ImageReference> {
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.042432, 9.914919),
          Address = new Address(
            street: "Vej 12",
            city: "Odense",
            zipCode: "5000",
            country: "Denmark",
            municipality: "Odense")
        },
      };

      _odenseIncidentReports = new List<IncidentReportEntity> {
        inciport1, inciport2, inciport3, inciport4
      };
      _odense.IncidentReports.AddRange(_odenseIncidentReports);
      _dbContext.SaveChanges();
    }

    private void SeedAalborgIncidentReports() {
      IncidentReportEntity inciport1 = new IncidentReportEntity {
        Status = ReportStatus.Recieved,
        Description = "There's a big hole in road. Please fix it!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_mainOptionWithSubCats, _mainOptionWithSubCats.SubCategories[1].Id),
        AssignedTeam = _aalborgTeams.First(), // No assigned team!
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date,
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.Date,
        ContactInformation = new ContactInformation {
          Email = "tom@jensen.dk",
          Name = "Tom Jensen",
          PhoneNumber = "52472185"
        },
        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("1")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("2")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("3")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.044513, 9.924438),
          Address = new Address(
            street: "Nytorv 12",
            city: "Aalborg",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Aalborg")
        },
      };

      IncidentReportEntity inciport2 = new IncidentReportEntity {
        //Id = 2,
        Status = ReportStatus.InProgress,
        Description = "There's ice on the road. Please bring the salt!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_mainOptionNoSubCats),
        AssignedTeam = _aalborgTeams.First(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date.AddDays(-2),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.Date.AddDays(-2),

        // No contact information!

        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("5")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("6")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.048706, 9.925894),
          Address = new Address(
            street: "Karnersvej 12",
            city: "Aalborg",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Aalborg")
        },
      };

      IncidentReportEntity inciport3 = new IncidentReportEntity {
        //Id = 1,
        Status = ReportStatus.Completed,
        Description = "Wheres the light?!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_mainOptionWithSubCats, _mainOptionWithSubCats.SubCategories[0].Id),
        AssignedTeam = _aalborgTeams[1],
        TimestampCreatedUtc = DateTimeOffset.UtcNow.Date.AddDays(-20),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.Date.AddDays(-20),
        ContactInformation = new ContactInformation {
          Email = "lars@jensen.dk",
          Name = "Lars Jensen",
          PhoneNumber = "54897536"
        },
        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("7")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.040003, 9.942636),
          Address = new Address(
            street: "Hadsundvej 12",
            city: "Aalborg",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Aalborg")
        },
      };

      IncidentReportEntity inciport4 = new IncidentReportEntity {
        //Id = 1,
        Status = ReportStatus.Archived,
        Description = "Theres no table here!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_mainOptionWithSubCats, _mainOptionWithSubCats.SubCategories[1].Id),
        AssignedTeam = _aalborgTeams[2],
        TimestampCreatedUtc = DateTimeOffset.UnixEpoch.AddDays(-40),
        TimestampLastModifiedUtc = DateTimeOffset.UnixEpoch.AddDays(-40),
        ContactInformation = new ContactInformation {
          Email = "hans@jensen.dk",
          Name = "Hans Jensen",
          PhoneNumber = "54897536"
        },

        ImageReferences = new List<ImageReference> {
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.042432, 9.914919),
          Address = new Address(
            street: "Vej 12",
            city: "Aalborg",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Aalborg")
        },
      };

      _aalborgIncidentReports = new List<IncidentReportEntity> {
        inciport1, inciport2, inciport3, inciport4
      };
      _aalborg.IncidentReports.AddRange(_aalborgIncidentReports);
      _dbContext.SaveChanges();
    }

    private void SeedMunicipalities() {
      _aalborg = new MunicipalityEntity() {
        Name = "Aalborg",
      };
      _odense = new MunicipalityEntity {
        Name = "Odense",
      };
      List<MunicipalityEntity> municipalities = new List<MunicipalityEntity>() { _aalborg, _odense };
      _dbContext.Municipalities.AddRange(municipalities);
      _dbContext.SaveChanges();
    }

    private void SeedUsers() {
      AdminRoleLoginToken = _testData.CreateAdminUserAsync(_aalborg).GetAwaiter().GetResult();
      ManagerRoleLoginToken = _testData.CreateManagerUserAsync(_aalborg).GetAwaiter().GetResult();
      MaintainerRoleLoginToken = _testData.CreateMaintainerUserAsync(_dbContext.Users.ToList()).GetAwaiter().GetResult();
      _dbContext.SaveChanges();
    }
  }
}