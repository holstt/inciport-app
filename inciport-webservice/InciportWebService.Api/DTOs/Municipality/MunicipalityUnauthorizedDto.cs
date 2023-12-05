using InciportWebService.Application;
using InciportWebService.Data;

namespace InciportWebService.Api {

  public class MunicipalityUnauthorizedDto {
    public int Id { get; set; }
    public string Name { get; set; }

    public static MunicipalityUnauthorizedDto FromEntity(MunicipalityEntity model) {
      return new MunicipalityUnauthorizedDto {
        Id = model.Id,
        Name = model.Name
      };
    }
  }
}