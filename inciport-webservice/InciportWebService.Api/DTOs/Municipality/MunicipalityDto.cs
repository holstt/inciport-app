using InciportWebService.Application;
using System.Linq;

namespace InciportWebService.Api {

  public class MunicipalityDto {
    public int Id { get; set; }
    public string Name { get; set; }
    public int IncidentReportCount { get; set; }
    public int WorkerCount { get; set; }
    public int CategoriesCount { get; set; }
    public int UsersCount { get; set; }

    public static MunicipalityDto FromEntity(MunicipalityEntity entitity) {
      return new MunicipalityDto {
        IncidentReportCount = entitity.IncidentReports.Count,
        WorkerCount = entitity.WorkerTeams.Count,
        CategoriesCount = entitity.MainCategories.Count,
        UsersCount = entitity.Users.Count,
        Id = entitity.Id,
        Name = entitity.Name
      };
    }
  }
}