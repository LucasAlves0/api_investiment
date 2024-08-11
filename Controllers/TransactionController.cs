using Microsoft.AspNetCore.Mvc;
using InvestmentPortfolioAPI.Models;
using InvestmentPortfolioAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvestmentPortfolioAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciar transações de compra e venda de produtos de investimento.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _service;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="TransactionController"/>.
        /// </summary>
        /// <param name="service">Serviço para processamento de transações.</param>
        public TransactionController(TransactionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Realiza uma compra de um produto de investimento.
        /// </summary>
        /// <param name="request">Dados da compra incluindo ID do produto, comprador, vendedor e quantidade.</param>
        /// <returns>Confirmação de compra bem-sucedida.</returns>
        /// <response code="200">Compra realizada com sucesso.</response>
        /// <response code="400">Erro de validação nos dados da compra.</response>
        [HttpPost("buy")]
        public async Task<ActionResult> Buy([FromBody] BuyRequest request)
        {
            try
            {
                await _service.ProcessBuyAsync(request.ProductId, request.BuyerId, request.SellerId, request.Quantity);
                return Ok("Compra realizada com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Realiza uma venda de um produto de investimento.
        /// </summary>
        /// <param name="request">Dados da venda incluindo ID do produto, comprador, vendedor e quantidade.</param>
        /// <returns>Confirmação de venda bem-sucedida.</returns>
        /// <response code="200">Venda realizada com sucesso.</response>
        /// <response code="400">Erro de validação nos dados da venda.</response>
        [HttpPost("sell")]
        public async Task<ActionResult> Sell([FromBody] SellRequest request)
        {
            try
            {
                await _service.ProcessSellAsync(request.ProductId, request.BuyerId, request.SellerId, request.Quantity);
                return Ok("Venda realizada com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recupera o extrato de transações de um cliente.
        /// </summary>
        /// <param name="customerId">ID do cliente para o qual o extrato é solicitado.</param>
        /// <returns>Lista de transações do cliente.</returns>
        /// <response code="200">Retorna o extrato de transações do cliente.</response>
        [HttpGet("statement/{customerId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetStatement(int customerId)
        {
            var transactions = await _service.GetCustomerTransactionsAsync(customerId);
            return Ok(transactions);
        }
    }
}

public class BuyRequest
{
    public int ProductId { get; set; }
    public int BuyerId { get; set; }
    public int SellerId { get; set; }
    public int Quantity { get; set; }
}

public class SellRequest
{
    public int ProductId { get; set; }
    public int BuyerId { get; set; }
    public int SellerId { get; set; }
    public int Quantity { get; set; }
}
