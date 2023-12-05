using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class GetWorkerTeamDto {
    [Required]
    public int? Id { get; set; }

    [Required, StringLength(256)]
    public string Name { get; set; }

    public static GetWorkerTeamDto FromModel(WorkerTeam model) {
      if (model is null) {
        return null;
      }

      return new GetWorkerTeamDto {
        Id = model.Id,
        Name = model.Name
      };
    }
  }
}