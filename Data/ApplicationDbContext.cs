using bfws.Models;
using bfws.Models.DBModels;
using BFWS.Models.DBModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bfws.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gender> Gender { get; set; }
        public DbSet<MaritalState> MaritalState { get; set; }
        public DbSet<Salutation> Salutation { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<Street> Streets { set; get; }
        public DbSet<District> District { get; set; }
        public DbSet<AccountStatus> AccountStatus { set; get; }
        public DbSet<BillStatus> BillStatus { set; get; }
        public DbSet<WaterBill> WaterBills { set; get; }
        public DbSet<ClientAccount> ClientAccounts { set; get; }
        public DbSet<Client> Clients { set; get; }
        public DbSet<BillRate> BillRate { set; get; }
        public DbSet<BillPayment> BillPayments { set; get; }
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().HasOne(a => a.Gender);
         

        }

        public DbSet<bfws.Models.DBModels.Meter> Meter { get; set; }

    }
}
