using Database.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Config
{
    public class ResponseParameterConfig : IEntityTypeConfiguration<ResponseParameter>
    {
        public void Configure(EntityTypeBuilder<ResponseParameter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();


            builder.HasOne(x => x.Api)
                .WithMany(x => x.ResponseParameters)
                .HasForeignKey(x => x.ApiId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
