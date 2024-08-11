using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InvestmentPortfolioAPI.Data;
using InvestmentPortfolioAPI.Models;

namespace InvestmentPortfolioAPI.Services
{
    public class InvestmentProductService
    {
        private readonly InvestmentContext _context;
        private readonly SmtpEmailService _emailService;
        private readonly ILogger<InvestmentProductService> _logger;

        public InvestmentProductService(InvestmentContext context, SmtpEmailService emailService, ILogger<InvestmentProductService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IEnumerable<InvestmentProduct>> GetAllAsync()
        {
            return await _context.InvestmentProducts.ToListAsync();
        }

        public async Task<InvestmentProduct?> GetByIdAsync(int id)
        {
            return await _context.InvestmentProducts.FindAsync(id);
        }

        public async Task<InvestmentProduct> CreateAsync(InvestmentProduct product)
        {
            _context.InvestmentProducts.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<InvestmentProduct?> UpdateAsync(InvestmentProduct product)
        {
            var existingProduct = await _context.InvestmentProducts.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Type = product.Type;
            existingProduct.Price = product.Price;
            existingProduct.MaturityDate = product.MaturityDate;
            existingProduct.QuantityAvailable = product.QuantityAvailable;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.InvestmentProducts.FindAsync(id);
            if (product != null)
            {
                _context.InvestmentProducts.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task NotifyExpiringProductsAsync()
        {
            var expirationThreshold = DateTime.Now.AddDays(7);
            var expiringProducts = await _context.InvestmentProducts
                .Where(p => p.MaturityDate <= expirationThreshold)
                .ToListAsync();

            if (!expiringProducts.Any())
            {
                _logger.LogInformation("Nenhum produto vencido encontrado.");
                return;
            }

            var administrators = await _context.Administrators.ToListAsync();
            if (!administrators.Any())
            {
                _logger.LogInformation("Nenhum administrador cadastrado para notificações.");
                return;
            }

            foreach (var product in expiringProducts)
            {
                var maturityDate = product.MaturityDate?.ToShortDateString() ?? "sem data de vencimento";
                var message = $"Lembrete: O produto de investimento '{product.Name}' está se aproximando da data de vencimento de {maturityDate}.";

                foreach (var admin in administrators)
                {
                    try
                    {
                        await _emailService.SendEmailAsync(admin.Email, "Alerta de expiração do produto", message);
                        _logger.LogInformation($"E-mail enviado para {admin.Name} sobre o produto {product.Name}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Falha ao enviar e-mail para {admin.Name}: {ex.Message}");
                    }
                }
            }
        }

        public async Task<List<TransactionDetail>> GetProductTransactionsAsync(int productId)
        {
            return await _context.Transactions
                .Where(t => t.ProductId == productId)
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDetail
                {
                    Id = t.Id,
                    Date = t.Date,
                    Type = t.IsPurchase ? "Compra" : "Venda",
                    Quantity = t.Quantity,
                    Price = t.Price,
                    Participant = t.IsPurchase ? $"Comprador ID: {t.BuyerId}" : $"Vendedor ID: {t.SellerId}"
                })
                .ToListAsync();
        }
    }
}