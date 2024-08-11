using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentPortfolioAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Customer")]
        public int BuyerId { get; set; }

        [ForeignKey("Customer")]
        public int SellerId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsPurchase { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } 
    }
}
