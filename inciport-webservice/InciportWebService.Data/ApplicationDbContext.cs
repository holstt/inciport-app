using InciportWebService.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using InciportWebService.Application;

namespace InciportWebService.Data {

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext {
    public DbSet<IncidentReportEntity> IncidentReportEntities { get; set; }
    public DbSet<MainCategory> MainCategories { get; set; }
    public DbSet<WorkerTeam> WorkerTeams { get; set; }
    public DbSet<MunicipalityEntity> Municipalities { get; set; }
    public DbSet<ImageReference> Images { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
    }

    public async Task ResetDbAsync() {
      Console.WriteLine("Reset database started!");
      Console.WriteLine("Deleting database...");
      await Database.EnsureDeletedAsync();
      Console.WriteLine("Creating database");
      await Database.EnsureCreatedAsync();
      Console.WriteLine("Database reset succesfully!");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      // Discover configurations automatically.
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

      // When mapping db data to classes -> always use properties in order to be able to do any custom mapping before setting the property.
      modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);

      base.OnModelCreating(modelBuilder);
    }

    public void ClearTracker() => ChangeTracker.Clear();
  }
}