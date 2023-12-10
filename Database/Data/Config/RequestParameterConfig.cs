using Database.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Config
{
    public class RequestParameterConfig : IEntityTypeConfiguration<RequestParameter>
    {
        public void Configure(EntityTypeBuilder<RequestParameter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();


            builder.HasOne(x => x.Api)
                .WithMany(x => x.RequestParameters)
                .HasForeignKey(x => x.ApiId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
