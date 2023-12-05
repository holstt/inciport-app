using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data.Persistence.Configurations {

  internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser> {

    public void Configure(EntityTypeBuilder<ApplicationUser> builder) {
      builder.Property(p => p.Role).HasMaxLength(256).IsRequired();
      builder.Property(p => p.FullName).HasMaxLength(256).IsRequired();
    }
  }
}