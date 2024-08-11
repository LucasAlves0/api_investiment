// Models/TransactionDetail.cs
using System;

namespace InvestmentPortfolioAPI.Models
{
    public class TransactionDetail
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Participant { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsPurchase { get; set; }
    }
}
