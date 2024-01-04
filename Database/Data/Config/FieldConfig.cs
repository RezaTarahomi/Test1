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
    public class FieldConfig : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.Entity)
              .WithMany(x => x.Fields)
              .HasForeignKey(x => x.EntityId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Parent)
             .WithMany(x=>x.EntityChilds)
             .HasForeignKey(x => x.ParentId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EnumType)
             .WithMany(x => x.Fields)
             .HasForeignKey(x => x.EnumTypeId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
