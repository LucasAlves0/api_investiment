namespace InvestmentPortfolioAPI.Models
{
    public class InvestmentProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Tipo do produto (ações, títulos, etc.)
        public decimal Price { get; set; } // Preço atual do produto
        public DateTime? MaturityDate { get; set; } // Data de vencimento, se aplicável
        public int QuantityAvailable { get; set; } // Quantidade disponível para negociação
    }
}
