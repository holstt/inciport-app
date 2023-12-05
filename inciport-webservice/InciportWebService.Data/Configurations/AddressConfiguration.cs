using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  internal class AddressConfiguration : IEntityTypeConfiguration<Address> {

    public void Configure(EntityTypeBuilder<Address> builder) {
      builder.Property(p => p.Street).HasMaxLength(256).IsRequired();
      builder.Property(p => p.City).HasMaxLength(256).IsRequired();
      builder.Property(p => p.ZipCode).HasMaxLength(256).IsRequired();
      builder.Property(p => p.Country).HasMaxLength(256).IsRequired();
      builder.Property(p => p.Municipality).HasMaxLength(256).IsRequired();
    }
  }
}