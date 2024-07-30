using HackathonHealthMed.Application;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly MedicoService _medicoService;
        private readonly ILogger<MedicoController> _logger;

        public MedicoController(MedicoService medicoService, ILogger<MedicoController> logger)
        {
            _medicoService = medicoService;
            _logger = logger;
        }

        // Endpoint para cadastrar um médico
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarMedicoRequest request)
        {

            try
            {
                // Chama o serviço para cadastrar o médico com os dados do request
                var result = await _medicoService.CadastrarMedicoAsync(request.Medico, request.Senha);
                if (result > 0)
                {
                    _logger.LogInformation("Médico cadastrado com sucesso.");
                    return Ok(new { success = true });
                }
                return BadRequest(new { success = false, message = "Erro ao cadastrar médico." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar médico.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

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

