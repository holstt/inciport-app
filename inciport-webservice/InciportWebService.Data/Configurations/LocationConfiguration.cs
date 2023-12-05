using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  internal class LocationConfiguration : IEntityTypeConfiguration<Location> {

    public void Configure(EntityTypeBuilder<Location> builder) {
      // Relations
      builder.HasOne(l => l.Address).WithOne().HasForeignKey<Address>(nameof(Location) + "Id").IsRequired(); // Define one to one relation.
      builder.OwnsOne(l => l.Coordinates, c => {
        c.Property(c => c.Latitude).IsRequired();
        c.Property(c => c.Longitude).IsRequired();
      }).Navigation(p => p.Coordinates).IsRequired(); // To same table
    }
  }
}