using InciportWebService.Application;
using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface IApplicationDbContext {
    DbSet<ImageReference> Images { get; set; }
    DbSet<IncidentReportEntity> IncidentReportEntities { get; set; }
    DbSet<MainCategory> MainCategories { get; set; }
    DbSet<MunicipalityEntity> Municipalities { get; set; }
    DbSet<WorkerTeam> WorkerTeams { get; set; }
    DbSet<ApplicationUser> Users { get; set; }

    Task ResetDbAsync();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void ClearTracker();

    EntityEntry Update(object entity);
  }
}