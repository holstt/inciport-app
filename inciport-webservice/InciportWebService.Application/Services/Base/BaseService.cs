using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public abstract class BaseService {
    public IApplicationDbContext DbContext { get; set; }

    public BaseService(IApplicationDbContext dbContext) {
      DbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
    }

    protected async Task EnsureMunicipalityExistsAsync(int municipalityId) {
      bool municipalityExists = await DbContext.Municipalities.AnyAsync(m => m.Id == municipalityId);
      if (!municipalityExists) {
        throw new NotFoundException("Municipality", municipalityId);
      }
    }

    protected async Task EnsureInciportsExistsAsync(int inciportId) {
      bool inciportExists = await DbContext.IncidentReportEntities.AnyAsync(m => m.Id == inciportId && m.Status != ReportStatus.Archived);
      if (!inciportExists) {
        throw new NotFoundException("Incident Report", inciportId);
      }
    }
  }
}