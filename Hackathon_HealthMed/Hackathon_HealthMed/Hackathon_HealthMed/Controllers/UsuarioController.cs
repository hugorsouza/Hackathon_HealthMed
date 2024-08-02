using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Application;
using Microsoft.AspNetCore.Mvc;
using HackathonHealthMed.Application.Interfaces;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly ILoginService _loginService;
        public UsuarioController(ILogger<UsuarioController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        // Endpoint para autenticação (login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
               
               var token = await _loginService.AutenticarAsync(request.Email, request.Senha);
                if (token != null)
                {
                    _logger.LogInformation("Login bem-sucedido.");
                    return Ok(@$"Token: {token}"); // Implementar a geração do token aqui.
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
