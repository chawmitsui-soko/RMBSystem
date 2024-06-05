using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecordManagementPortalDev.Data;
using RecordManagementPortalDev.Models;

namespace RecordManagementPortalDev.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }        

        public DbSet<Department> Departments { get; set; }

        public DbSet<OrderRequests> OrderRequests { get; set; }

        public DbSet<ApplicationUser> AppUsers { get; set; }

        public DbSet<BillRateMaster> BillRateMaster { get; set; }

        public DbSet<CrtnType> CartonType { get; set; }

        public DbSet<CrtnMonthlyBals> CtnMonthlyClosingBal { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }

        public DbSet<CtnStockReceiving> CtnStockReceiving { get; set; }

        public DbSet<Job> Job { get; set; }

        public DbSet<JobsDetLoc> JobsDetLoc { get; set; }

        public DbSet<LocationHistory> LocationHistory { get; set; }

        public DbSet<LogScanPack> LogScanPack { get; set; }

        public DbSet<JobServiceLevel> JobServiceLevel { get; set; }

        public DbSet<RecordDetail> RecordDetail { get; set; }

        public DbSet<ApprovalSetup> ApprovalSetup { get; set; }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<CartonDetails> CartonDetails { get; set; }        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //builder.Entity<IdentityUser>().ToTable("AppUsers");
            builder.Entity<ApplicationUser>().ToTable("AppUsers");
            builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("AppUserToken");
            builder.Entity<IdentityRole>().ToTable("AppRoles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims");            
        }
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.UserCode).HasMaxLength(15);
    }
}