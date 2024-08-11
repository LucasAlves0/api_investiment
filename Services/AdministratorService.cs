using InvestmentPortfolioAPI.Data;
using InvestmentPortfolioAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InvestmentPortfolioAPI.Services
{
    public class AdministratorService
    {
        private readonly InvestmentContext _context;

        public AdministratorService(InvestmentContext context)
        {
            _context = context;
        }

        public async Task<List<Administrator>> GetAllAsync()
        {
            return await _context.Administrators.ToListAsync();
        }

        public async Task<Administrator?> GetByIdAsync(int id)
        {
            return await _context.Administrators.FindAsync(id);
        }

        public async Task AddAsync(Administrator administrator)
        {
            _context.Administrators.Add(administrator);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Administrator administrator)
        {
            _context.Entry(administrator).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var admin = await _context.Administrators.FindAsync(id);
            if (admin != null)
            {
                _context.Administrators.Remove(admin);
                await _context.SaveChangesAsync();
            }
        }
    }
}
