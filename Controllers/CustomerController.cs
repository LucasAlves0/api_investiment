using Microsoft.AspNetCore.Mvc;
using InvestmentPortfolioAPI.Models;
using InvestmentPortfolioAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvestmentPortfolioAPI.Controllers
{
    /// <summary>
    /// Gerencia as operações relacionadas aos clientes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CustomerController"/>.
        /// </summary>
        /// <param name="service">O serviço de clientes a ser usado.</param>
        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        /// <summary>
        /// Recupera todos os clientes.
        /// </summary>
        /// <returns>Uma lista de clientes.</returns>
        /// <response code="200">Retorna a lista de clientes.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), 200)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        /// <summary>
        /// Recupera um cliente específico pelo identificador único.
        /// </summary>
        /// <param name="id">O ID do cliente a ser recuperado.</param>
        /// <returns>O cliente, se encontrado.</returns>
        /// <response code="200">Retorna o cliente solicitado.</response>
        /// <response code="404">Se o cliente não for encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="customer">Os dados do cliente para criação.</param>
        /// <returns>Um cliente recém-criado.</returns>
        /// <response code="201">Retorna o cliente recém-criado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Customer), 201)]
        public async Task<ActionResult> Create([FromBody] Customer customer)
        {
            await _service.AddAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="id">O ID do cliente a ser atualizado.</param>
        /// <param name="customer">Os dados atualizados do cliente.</param>
        /// <returns>Um ActionResult indicando o resultado da operação.</returns>
        /// <response code="204">Se a atualização for bem-sucedida.</response>
        /// <response code="400">Se o ID fornecido não corresponder ao ID do cliente.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(customer);
            return NoContent();
        }

        /// <summary>
        /// Exclui um cliente específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do cliente a ser excluído.</param>
        /// <returns>Um ActionResult indicando o resultado da operação.</returns>
        /// <response code="204">Se a exclusão for bem-sucedida.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
