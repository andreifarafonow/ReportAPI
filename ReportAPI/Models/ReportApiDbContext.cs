using Microsoft.EntityFrameworkCore;
using ReportAPI.Models.Configurations;

namespace ReportAPI.Models
{
    public class ReportApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }

        public ReportApiDbContext(DbContextOptions<ReportApiDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
        }
    }
}
