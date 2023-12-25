using Database.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Config
{
    public class EnumFieldConfig : IEntityTypeConfiguration<EnumField>
    {
        public void Configure(EntityTypeBuilder<EnumField> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.EnumType)
              .WithMany(x => x.EnumFields)
              .HasForeignKey(x => x.EnumTypeId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

           


        }
    }
}
