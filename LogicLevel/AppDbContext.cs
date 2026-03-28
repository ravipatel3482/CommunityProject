using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectDataStructure.Addressrelatedclasses;
using ProjectDataStructure.Electronics;
using ProjectDataStructure.Enum;
using ProjectDataStructure.IdentityClass;
using System.Linq;

namespace LogicLevel
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        //public DbSet<ModelYear> ModelYears { get; set; }
        //public DbSet<IndiaCar> IndiaCars { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<IndiaCity> IndiaCities { get; set; }
        public DbSet<ServicesOrder> ServicesOrders { get; set; }
        public DbSet<IndiaUserAddress> IndianUserAddresses { get; set; }
        //public DbSet<Product> products { get; set; }
        public DbSet<ServicesProvider> ServicesProviders { get; set; }
        public DbSet<ServicesType> SerciesTypes { get; set; }
        //public DbSet<ProviderServices> ProviderServices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }

    }
}