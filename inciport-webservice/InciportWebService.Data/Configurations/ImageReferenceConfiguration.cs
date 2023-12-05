using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  internal class ImageReferenceConfiguration : IEntityTypeConfiguration<ImageReference> {

    public void Configure(EntityTypeBuilder<ImageReference> builder) {
      builder.Property(p => p.RelativePhysicalPath).IsRequired();
    }
  }
}