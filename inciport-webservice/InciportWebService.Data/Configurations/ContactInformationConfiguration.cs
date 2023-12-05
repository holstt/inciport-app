using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data.Persistence.Configurations {

  internal class ContactInformationConfiguration : IEntityTypeConfiguration<ContactInformation> {

    public void Configure(EntityTypeBuilder<ContactInformation> builder) {
      builder.Property(t => t.Name).HasMaxLength(256).IsRequired();
      builder.Property(t => t.PhoneNumber).HasMaxLength(256).IsRequired();
      builder.Property(t => t.Email).HasMaxLength(256).IsRequired();
    }
  }
}