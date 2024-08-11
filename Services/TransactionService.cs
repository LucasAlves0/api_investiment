using InvestmentPortfolioAPI.Data;
using InvestmentPortfolioAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvestmentPortfolioAPI.Services
{
    public class TransactionService
    {
        private readonly InvestmentContext _context;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(InvestmentContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ProcessBuyAsync(int productId, int buyerId, int sellerId, int quantity)
        {
            var product = await _context.InvestmentProducts.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }
            if (product.QuantityAvailable < quantity)
            {
                throw new InvalidOperationException("Quantidade insuficiente disponível.");
            }

            product.QuantityAvailable -= quantity;

            var transaction = new Transaction
            {
                ProductId = productId,
                BuyerId = buyerId,
                SellerId = sellerId,
                Quantity = quantity,
                IsPurchase = true,
                Price = product.Price, // Assuming price is set during purchase
                Date = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task ProcessSellAsync(int productId, int buyerId, int sellerId, int quantity)
        {
            var product = await _context.InvestmentProducts.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }

            product.QuantityAvailable += quantity;

            var transaction = new Transaction
            {
                ProductId = productId,
                BuyerId = buyerId,
                SellerId = sellerId,
                Quantity = quantity,
                IsPurchase = false,
                Price = product.Price, // Assuming price is set during sale
                Date = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetCustomerTransactionsAsync(int customerId)
        {
            return await _context.Transactions
                .Where(t => t.BuyerId == customerId || t.SellerId == customerId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }
    }
}
