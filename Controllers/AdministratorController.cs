using Microsoft.AspNetCore.Mvc;
using InvestmentPortfolioAPI.Models;
using InvestmentPortfolioAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvestmentPortfolioAPI.Controllers
{
    /// <summary>
    /// Gerencia as operações relacionadas aos administradores.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly AdministratorService _service;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="AdministratorController"/>.
        /// </summary>
        /// <param name="service">O serviço de administradores a ser usado.</param>
        public AdministratorController(AdministratorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Recupera todos os administradores.
        /// </summary>
        /// <returns>Uma lista de administradores.</returns>
        /// <response code="200">Retorna a lista de administradores.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Administrator>), 200)]
        public async Task<ActionResult<IEnumerable<Administrator>>> GetAll()
        {
            var admins = await _service.GetAllAsync();
            return Ok(admins);
        }

        /// <summary>
        /// Recupera um administrador específico pelo identificador único.
        /// </summary>
        /// <param name="id">O ID do administrador a ser recuperado.</param>
        /// <returns>O administrador, se encontrado.</returns>
        /// <response code="200">Retorna o administrador solicitado.</response>
        /// <response code="404">Se o administrador não for encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Administrator), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Administrator>> GetById(int id)
        {
            var admin = await _service.GetByIdAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }

        /// <summary>
        /// Cria um novo administrador.
        /// </summary>
        /// <param name="administrator">Os dados do administrador para criação.</param>
        /// <returns>Um administrador recém-criado.</returns>
        /// <response code="201">Retorna o administrador recém-criado.</response>
        /// <example>
        /// POST /api/Administrator
        /// {
        ///     "name": "John Doe",
        ///     "email": "johndoe@example.com"
        /// }
        /// </example>
        [HttpPost]
        [ProducesResponseType(typeof(Administrator), 201)]
        public async Task<ActionResult> Create([FromBody] Administrator administrator)
        {
            await _service.AddAsync(administrator);
            return CreatedAtAction(nameof(GetById), new { id = administrator.Id }, administrator);
        }

        /// <summary>
        /// Atualiza um administrador existente.
        /// </summary>
        /// <param name="id">O ID do administrador a ser atualizado.</param>
        /// <param name="administrator">Os dados atualizados do administrador.</param>
        /// <returns>Um ActionResult indicando o resultado da operação.</returns>
        /// <response code="204">Se a atualização for bem-sucedida.</response>
        /// <response code="400">Se o ID fornecido não corresponder ao ID do administrador.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] Administrator administrator)
        {
            if (id != administrator.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(administrator);
            return NoContent();
        }

        /// <summary>
        /// Exclui um administrador específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do administrador a ser excluído.</param>
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
