using Database.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.HasOne(x => x.ForeignKey)
             .WithMany()
             .HasForeignKey(x => x.ForeignKeyId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
