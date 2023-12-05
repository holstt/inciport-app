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

  internal class IncidentReportEntityConfiguration : IEntityTypeConfiguration<IncidentReportEntity> {

    public void Configure(EntityTypeBuilder<IncidentReportEntity> builder) {
      // Properties only
      builder.Property(i => i.Status).IsRequired(); // Enum stored as an integer.
      builder.Property(i => i.TimestampCreatedUtc).IsRequired();
      builder.Property(i => i.TimestampLastModifiedUtc).IsRequired();
      builder.Property(i => i.Description).IsRequired();

      // Relations
      builder.OwnsOne(i => i.ChosenMainCategoryEntity, c => { // Move to same table.
        c.Property(c => c.MainCategoryId).IsRequired();
      }).Navigation(i => i.ChosenMainCategoryEntity).IsRequired();

      builder.HasOne(i => i.Location).WithOne().HasForeignKey<Location>(nameof(IncidentReportEntity) + "Id").IsRequired();
      // Optional relations
      builder.HasOne(i => i.ContactInformation).WithOne().HasForeignKey<ContactInformation>(nameof(IncidentReportEntity) + "Id").OnDelete(DeleteBehavior.Cascade);
      builder.HasMany(i => i.ImageReferences).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
  }
}