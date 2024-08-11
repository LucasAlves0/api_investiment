namespace InvestmentPortfolioAPI.Models
{
    public class BuyRequest
    {
        public int ProductId { get; set; }
        public string CustomerId { get; set; } = string.Empty; // Inicializa com string vazia
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}