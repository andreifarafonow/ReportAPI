using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReportAPI.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email)
                   .HasMaxLength(254)
                   .IsRequired();
            builder.HasIndex(x => x.Email)
                   .IsUnique();

            builder.Property(x => x.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.LastName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Patronymic)
                   .HasMaxLength(100);

            builder.HasMany(x => x.Reports)
                   .WithOne(x => x.User)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
