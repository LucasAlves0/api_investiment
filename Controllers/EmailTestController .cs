using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InvestmentPortfolioAPI.Services;

namespace InvestmentPortfolioAPI.Controllers
{
    /// <summary>
    /// Controlador para testar o envio de e-mails e notificar sobre produtos com vencimento próximo.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTestController : ControllerBase
    {
        private readonly SmtpEmailService _emailService;
        private readonly InvestmentProductService _productService;

        /// <summary>
        /// Construtor para o controlador de teste de e-mail.
        /// </summary>
        /// <param name="emailService">Serviço de e-mail para envio de mensagens.</param>
        /// <param name="productService">Serviço de produtos para notificar sobre produtos expirando.</param>
        public EmailTestController(SmtpEmailService emailService, InvestmentProductService productService)
        {
            _emailService = emailService;
            _productService = productService;
        }

        /// <summary>
        /// Envia um e-mail de teste.
        /// </summary>
        /// <returns>Resultado da operação de enviar e-mail.</returns>
        /// <response code="200">E-mail enviado com sucesso.</response>
        /// <response code="500">Erro ao enviar e-mail.</response>
        [HttpGet("send-test-email")]
        public async Task<IActionResult> SendTestEmail()
        {
            try
            {
                // Substitua 'seu-email@dominio.com' por um endereço de e-mail real e válido
                await _emailService.SendEmailAsync("l2000coffe@gmail.com", "Teste de E-mail", "Este é um e-mail de teste.");
                return Ok("E-mail enviado com sucesso.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar e-mail: {ex.Message}");
            }
        }

        /// <summary>
        /// Notifica sobre produtos com vencimento próximo.
        /// </summary>
        /// <returns>Resultado da operação de notificação.</returns>
        /// <response code="200">Notificações enviadas com sucesso.</response>
        /// <response code="500">Erro ao enviar notificações.</response>
        [HttpGet("notify-expiring-products")]
        public async Task<IActionResult> NotifyExpiringProducts()
        {
            try
            {
                await _productService.NotifyExpiringProductsAsync();
                return Ok("Notificações enviadas com sucesso.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar notificações: {ex.Message}");
            }
        }
    }
}
