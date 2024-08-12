using Microsoft.EntityFrameworkCore;
using InvestmentPortfolioAPI.Models;

namespace InvestmentPortfolioAPI.Data
{
    public class InvestmentContext : DbContext
    {
        public InvestmentContext(DbContextOptions<InvestmentContext> options)
            : base(options)
        {
        }

  
        public DbSet<InvestmentProduct> InvestmentProducts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<InvestmentProduct>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);


            modelBuilder.Entity<Transaction>()
                .Property(t => t.Quantity)
                .HasPrecision(18, 2);
        }
    }
}
