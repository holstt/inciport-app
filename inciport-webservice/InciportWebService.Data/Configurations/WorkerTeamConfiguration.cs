using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  internal class WorkerTeamConfiguration : IEntityTypeConfiguration<WorkerTeam> {

    public void Configure(EntityTypeBuilder<WorkerTeam> builder) {
      builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
      builder.Property(p => p.IsArchived).IsRequired();
    }
  }
}