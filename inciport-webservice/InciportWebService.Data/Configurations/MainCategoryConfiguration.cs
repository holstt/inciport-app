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

  internal class MainCategoryConfiguration : IEntityTypeConfiguration<MainCategory> {

    public void Configure(EntityTypeBuilder<MainCategory> builder) {
      builder.Property(t => t.IsArchived).IsRequired();
      builder.Property(t => t.Title).HasMaxLength(256).IsRequired();

      // Relations
      builder.HasMany(c => c.SubCategories).WithOne().OnDelete(DeleteBehavior.Cascade); // Cascade delete but do not require, as it is a TPH relation (inheritance)
    }
  }
}