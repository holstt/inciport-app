using InciportWebService.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface IWorkerTeamsService {

    Task<WorkerTeam> GetWorkerTeam(int municipalityId, int workerTeamId);

    Task<List<WorkerTeam>> GetWorkerTeams(int municipalityId);

    //Task<List<WorkerTeam>> UpdateWorkerTeam(int municipalityId, GetWorkerTeamDto inputDto);

    Task<WorkerTeam> CreateWorkerTeams(int municipalityId, CreateWorkerTeamDto inputDto);

    //Task<WorkerTeam> DeleteWorkerTeam(int municipalityId, int workerTeamId);
  }
}