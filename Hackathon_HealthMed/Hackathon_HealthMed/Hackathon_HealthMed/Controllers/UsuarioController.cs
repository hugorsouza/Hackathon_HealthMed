using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Application;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        // Endpoint para autenticação (login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Chama o serviço de autenticação com as credenciais do request
                var medico = await _medicoService.AutenticarAsync(request.Email, request.Senha);
                if (medico != null)
                {
                    _logger.LogInformation("Login bem-sucedido.");
                    return Ok(new { token = "BearerTokenGerado" }); // Implementar a geração do token aqui.
                }
                return Unauthorized(new { success = false, message = "Credenciais inválidas." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar login.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
