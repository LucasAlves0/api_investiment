using Microsoft.AspNetCore.Mvc;
using InvestmentPortfolioAPI.Models;
using InvestmentPortfolioAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentPortfolioAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciar os produtos de investimento.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentProductController : ControllerBase
    {
        private readonly InvestmentProductService _service;

        /// <summary>
        /// Construtor para InvestmentProductController.
        /// </summary>
        /// <param name="service">O serviço de produtos de investimento a ser usado.</param>
        public InvestmentProductController(InvestmentProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Recupera todos os produtos de investimento.
        /// </summary>
        /// <returns>Uma lista de produtos de investimento.</returns>
        /// <response code="200">Retorna todos os produtos de investimento.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestmentProduct>>> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// Recupera um produto de investimento específico pelo seu identificador único.
        /// </summary>
        /// <param name="id">O ID do produto de investimento a ser recuperado.</param>
        /// <returns>O produto de investimento, se encontrado.</returns>
        /// <response code="200">Retorna o produto de investimento solicitado.</response>
        /// <response code="404">Se o produto de investimento não for encontrado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<InvestmentProduct>> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Recupera todas as transações de um produto de investimento específico.
        /// </summary>
        /// <param name="id">O ID do produto para o qual as transações serão recuperadas.</param>
        /// <returns>Uma lista de detalhes de transações.</returns>
        /// <response code="200">Retorna as transações do produto de investimento.</response>
        /// <response code="404">Se não forem encontradas transações para o produto.</response>
        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<TransactionDetail>>> GetProductTransactions(int id)
        {
            var transactions = await _service.GetProductTransactionsAsync(id);
            if (transactions == null || !transactions.Any())
            {
                return NotFound("Nenhuma transação encontrada para este produto.");
            }
            return Ok(transactions);
        }

        /// <summary>
        /// Cria um novo produto de investimento.
        /// </summary>
        /// <param name="product">Os dados do produto de investimento para criação.</param>
        /// <returns>O produto de investimento criado.</returns>
        /// <response code="201">Retorna o produto de investimento recém-criado.</response>
        [HttpPost]
        public async Task<ActionResult<InvestmentProduct>> Create(InvestmentProduct product)
        {
            var createdProduct = await _service.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Atualiza um produto de investimento existente.
        /// </summary>
        /// <param name="id">O ID do produto de investimento a ser atualizado.</param>
        /// <param name="product">Os dados atualizados do produto de investimento.</param>
        /// <returns>Um ActionResult indicando o resultado da operação.</returns>
        /// <response code="204">Se a atualização for bem-sucedida.</response>
        /// <response code="400">Se o ID fornecido não corresponder ao ID do produto.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InvestmentProduct product)
        {
            if (id != product.Id)
            {
                return BadRequest("O ID fornecido não corresponde ao ID do produto.");
            }
            await _service.UpdateAsync(product);
            return NoContent();
        }

        /// <summary>
        /// Exclui um produto de investimento específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do produto de investimento a ser excluído.</param>
        /// <returns>Um ActionResult indicando o resultado da operação.</returns>
        /// <response code="204">Se a exclusão for bem-sucedida.</response>
        /// <response code="404">Se o produto de investimento não for encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
