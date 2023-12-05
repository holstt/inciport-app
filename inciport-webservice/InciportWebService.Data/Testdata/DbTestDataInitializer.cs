using InciportWebService.Application;

using InciportWebService.Domain;
using InciportWebService.Data.Persistence.Testdata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  public static class TestCategories {

    public static class Main {
      public const string PUBLIC_FACILITIES = "Public facilities";
      public const string LIGHT = "Lighting";
      public const string ROAD = "Road";
    }

    public static class Sub {
      public const string GARBAGE = "Garbage";
      public const string TOILETS = "Toilets";
      public const string TABLES_AND_BENCHES = "Tables and benches";
      public const string HOLE = "Hole";
      public const string ICE = "Ice";
      public const string VEGETATION_NEAR_ROAD = "Vegetation near road";
    }
  }

  public class DbTestDataInitializer {
    // Provide login tokens on all seeded users
    public string MaintainerRoleDemoLoginToken { get; private set; }
    public string ManagerRoleDemoLoginToken { get; set; }
    public string AdminRoleDemoLoginToken { get; set; }

    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    private readonly TestDataUserFactory _testData;

    private List<MainCategory> _aalborgOptions;
    private List<MainCategory> _odenseOptions;
    private WorkerTeamsTestData _aalborgTeams;
    private WorkerTeamsTestData _odenseTeams;

    private List<IncidentReportEntity> _aalborgIncidentReports;
    private List<IncidentReportEntity> _odenseIncidentReports;

    private MunicipalityEntity _aalborg;
    private MunicipalityEntity _odense;
    private MunicipalityEntity _aarhus;

    public DbTestDataInitializer(ApplicationDbContext appDbContext, IServiceProvider services, IConfiguration configuration) {
      _dbContext = appDbContext;
      _services = services;
      _configuration = configuration;
      _testData = new TestDataUserFactory(services);
    }

    public async Task InitializeAsync() {
      DbDefaultInitializer initializer = new DbDefaultInitializer(_dbContext, _services, _configuration);
      await initializer.InitializeAsync();
      Console.WriteLine("Initializing db with TEST data...");
      SeedMunicipalities();
      SeedWorkerTeams();
      SeedCategoryOptions();
      SeedAalborgIncidentReports();
      SeedOdenseIncidentReports();
      await SeedUsersAsync();
      _dbContext.SaveChanges();
      Console.WriteLine("Db initialized!");
    }

    private async Task SeedUsersAsync() {
      MaintainerRoleDemoLoginToken = await _testData.CreateMaintainerUserAsync(_dbContext.Users.ToList());
      AdminRoleDemoLoginToken = await _testData.CreateAdminUserAsync(_aalborg);
      ManagerRoleDemoLoginToken = await _testData.CreateManagerUserAsync(_aalborg);
      ManagerRoleDemoLoginToken = await _testData.CreateAdminUserAsync(_odense);
      ManagerRoleDemoLoginToken = await _testData.CreateManagerUserAsync(_odense);
      ManagerRoleDemoLoginToken = await _testData.CreateAdminUserAsync(_aarhus);
      ManagerRoleDemoLoginToken = await _testData.CreateManagerUserAsync(_aarhus);
      _dbContext.SaveChanges();
    }

    private void SeedMunicipalities() {
      _aalborg = new MunicipalityEntity() {
        Name = "Aalborg",
      };
      _odense = new MunicipalityEntity {
        Name = "Odense",
      };
      _aarhus = new MunicipalityEntity {
        Name = "Aarhus",
      };
      List<MunicipalityEntity> municipalities = new List<MunicipalityEntity>() { _aalborg, _odense, _aarhus };
      _dbContext.Municipalities.AddRange(municipalities);
      _dbContext.SaveChanges();
    }

    private void SeedWorkerTeams() {
      WorkerTeamsTestDataFactory factory = new WorkerTeamsTestDataFactory(_dbContext);

      _aalborgTeams = factory.Create(5, 10, _aalborg);
      _odenseTeams = factory.Create(5, 10, _odense);
    }

    private void SeedAalborgIncidentReports() {
      IncidentReportEntity inciport1 = new IncidentReportEntity {
        Status = ReportStatus.Completed,
        Description = "There's a big hole in road. Please fix it!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_aalborgOptions[0], _aalborgOptions[0].SubCategories[0].Id),
        AssignedTeam = null,
        TimestampCreatedUtc = DateTimeOffset.UtcNow,
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow,
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
          Address = new Address(street: "Nytorv 12",
            city: "Aalborg",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Aalborg")
        },
      };

      IncidentReportEntity inciport2 = new IncidentReportEntity {
        Status = ReportStatus.InProgress,
        Description = "There's ice on the road. Please bring the salt!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_aalborgOptions[0], _aalborgOptions[0].SubCategories[1].Id),
        AssignedTeam = _aalborgTeams.ChooseNext(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.AddDays(-2),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.AddDays(-2),
        // No contact information!
        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("5")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("6")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.048706, 9.925894),
          Address = new Address(street: "Nytorv 12",
            city: "Aalborg",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Aalborg")
        },
      };

      IncidentReportEntity inciport3 = new IncidentReportEntity {
        Status = ReportStatus.Completed,
        Description = "Wheres the light?!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_aalborgOptions[1]),

        AssignedTeam = _aalborgTeams.ChooseNext(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.AddDays(-10),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.AddDays(-10),
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
          Address = new Address(street: "Hadsundvej 12",
            city: "Aalborg",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Aalborg")
        },
      };

      IncidentReportEntity inciport4 = new IncidentReportEntity {
        Status = ReportStatus.Archived,
        Description = "Theres no table here!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_aalborgOptions[0], _aalborgOptions[0].SubCategories[2].Id),
        AssignedTeam = _aalborgTeams.ChooseNext(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.AddDays(-40),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.AddDays(-40),
        ContactInformation = new ContactInformation {
          Email = "hans@jensen.dk",
          Name = "Hans Jensen",
          PhoneNumber = "54897536"
        },

        ImageReferences = new List<ImageReference>(),
        Location = new Location() {
          Coordinates = new Coordinates(57.042432, 9.914919),
          Address = new Address(street: "Vej 12",
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

    private void SeedOdenseIncidentReports() {
      IncidentReportEntity inciport1 = new IncidentReportEntity {
        Status = ReportStatus.Recieved,
        Description = "There's a big hole in road. Please fix it!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[0].Id),
        AssignedTeam = null,
        TimestampCreatedUtc = DateTimeOffset.UtcNow,
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow,
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
          Address = new Address(street: "Odensevej 12",
            city: "Odense",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Odense")
        },
      };

      IncidentReportEntity inciport2 = new IncidentReportEntity {
        Status = ReportStatus.InProgress,
        Description = "There's ice on the road. Please bring the salt!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[0].Id),
        AssignedTeam = _odenseTeams.ChooseNext(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.AddDays(-2),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.AddDays(-2),
        // No contact information!
        ImageReferences = new List<ImageReference> {
          new ImageReference(ImageSaver.CreateRelativeImagePath("5")),
          new ImageReference(ImageSaver.CreateRelativeImagePath("6")),
        },
        Location = new Location() {
          Coordinates = new Coordinates(57.048706, 9.925894),
          Address = new Address(street: "Odensevej 3",
            city: "Odense",
            zipCode: "9000",
            country: "Denmark",
            municipality: "Odense")
        },
      };

      // This should be archived at startup
      IncidentReportEntity inciport3 = new IncidentReportEntity {
        Status = ReportStatus.Completed,
        Description = "Wheres the light?!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[0].Id),
        AssignedTeam = _odenseTeams.ChooseNext(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.AddDays(-50),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.AddDays(-40),
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
          Address = new Address(street: "Odensevej 10",
            city: "Odense",
            zipCode: "5000",
            country: "Denmark",
            municipality: "Odense")
        }
      };

      IncidentReportEntity inciport4 = new IncidentReportEntity {
        Status = ReportStatus.Completed,
        Description = "Theres no table here!",
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromCategory(_odenseOptions[0], _odenseOptions[0].SubCategories[0].Id),
        AssignedTeam = _odenseTeams.ChooseNext(),
        TimestampCreatedUtc = DateTimeOffset.UtcNow.AddDays(-50),
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow.AddDays(-20),
        ContactInformation = new ContactInformation {
          Email = "hans@jensen.dk",
          Name = "Hans Jensen",
          PhoneNumber = "54897536"
        },

        ImageReferences = new List<ImageReference>(),
        Location = new Location() {
          Coordinates = new Coordinates(57.042432, 9.914919),
          Address = new Address(street: "Vej 12",
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

    private void SeedCategoryOptions() {
      MainCategory mainOption1 = new() {
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

      MainCategory mainOption2 = new() {
        Title = TestCategories.Main.LIGHT,
      };

      List<MainCategory> aalborgMainCategoryOptions = new List<MainCategory> {
        mainOption1, mainOption2
      };
      _aalborgOptions = aalborgMainCategoryOptions;
      _aalborg.MainCategories.AddRange(_aalborgOptions);
      _dbContext.SaveChanges();

      MainCategory mainOption3 = new() {
        Title = TestCategories.Main.PUBLIC_FACILITIES,
        SubCategories = new List<Category>() {
             new Category {
                Title = TestCategories.Sub.GARBAGE,
             },
             new Category {
               Title = TestCategories.Sub.TOILETS,
             },
             new Category {
                 Title = TestCategories.Sub.TABLES_AND_BENCHES,
             }
           }
      };

      List<MainCategory> odenseMainCategoryOptions = new List<MainCategory> {
        mainOption3
      };
      _odenseOptions = odenseMainCategoryOptions;
      _odense.MainCategories.AddRange(odenseMainCategoryOptions);
      _dbContext.SaveChanges();
    }
  }
}