using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReportAPI.Models.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.Property(x => x.Comment)
                   .HasMaxLength(5000)
                    .IsRequired();

            builder.Property(x => x.HoursCount)
                   .IsRequired();

            builder.Property(x => x.Date)
                   .HasColumnType("date")
                   .IsRequired();
        }
    }
}
