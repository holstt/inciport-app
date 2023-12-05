using InciportWebService.Application;
using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  internal class MunicipalityConfiguration : IEntityTypeConfiguration<MunicipalityEntity> {

    public void Configure(EntityTypeBuilder<MunicipalityEntity> builder) {
      // Propertie config
      builder.Property(m => m.Name).HasMaxLength(256).IsRequired();

      // Relations
      builder.HasMany(m => m.IncidentReports).WithOne().IsRequired(); // Define required relationships
      builder.HasMany(m => m.MainCategories).WithOne().IsRequired();
      builder.HasMany(m => m.WorkerTeams).WithOne().IsRequired();
      builder.HasMany(m => m.Users).WithOne().OnDelete(DeleteBehavior.Cascade); // Cascade delete but do not require
    }
  }
}