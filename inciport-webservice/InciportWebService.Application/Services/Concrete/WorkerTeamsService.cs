using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class WorkerTeamsService : BaseService, IWorkerTeamsService {
    private readonly IApplicationDbContext _dbContext;

    public WorkerTeamsService(IApplicationDbContext dbContext) : base(dbContext) {
      _dbContext = dbContext;
    }

    public async Task<WorkerTeam> GetWorkerTeam(int municipalityId, int workerTeamId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      return await GetDbWorkerTeamAsync(municipalityId, workerTeamId);
    }

    public async Task<List<WorkerTeam>> GetWorkerTeams(int municipalityId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      return await GetDbWorkerTeamsAsync(municipalityId);
    }

    private async Task<List<WorkerTeam>> GetDbWorkerTeamsAsync(int municipalityId) {
      return (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                            .Select(m => m.WorkerTeams
                                              .Where(w => !w.IsArchived))
                                            .FirstOrDefaultAsync()).ToList();
    }

    //public async Task<List<WorkerTeam>> UpdateWorkerTeam(int municipalityId, GetWorkerTeamDto inputDto) {
    //}

    public async Task<WorkerTeam> CreateWorkerTeams(int municipalityId, CreateWorkerTeamDto inputDto) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      WorkerTeam model = new WorkerTeam(inputDto.Name, isArchived: false);
      _dbContext.Municipalities.FirstOrDefault(m => m.Id == municipalityId).WorkerTeams.Add(model);
      await _dbContext.SaveChangesAsync();
      return model;
    }

    //public async Task<WorkerTeam> DeleteWorkerTeam(int municipalityId, int workerTeamId) {
    //}

    private async Task<WorkerTeam> GetDbWorkerTeamAsync(int municipalityId, int workerTeamId) {
      WorkerTeam workerTeam = (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                                                  .Select(m => m.WorkerTeams.Where(c => c.Id == workerTeamId && !c.IsArchived)).FirstOrDefaultAsync())
                                                                  .FirstOrDefault();
      if (workerTeam is null) {
        throw new NotFoundException("Worker team", workerTeamId);
      }

      return workerTeam;
    }
  }
}