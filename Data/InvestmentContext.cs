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

        // Defina DbSets para cada entidade que deseja mapear para o banco de dados
        public DbSet<InvestmentProduct> InvestmentProducts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração adicional do modelo, se necessário
            modelBuilder.Entity<InvestmentProduct>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Certifique-se de não configurar propriedades inexistentes
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Quantity)
                .HasPrecision(18, 2);
        }
    }
}
