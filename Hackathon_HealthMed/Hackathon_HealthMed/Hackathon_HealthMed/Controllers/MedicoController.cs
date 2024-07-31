using HackathonHealthMed.Application;
using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoService _medicoService;
        private readonly ILogger<MedicoController> _logger;
        private readonly ILoginService _loginService;

        public MedicoController(IMedicoService medicoService, ILogger<MedicoController> logger, ILoginService loginService)
        {
            _medicoService = medicoService;
            _logger = logger;
            _loginService = loginService;
        }

        // Endpoint para cadastrar um médico
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarMedicoRequest request)
        {

            try
            {
                // Chama o serviço para cadastrar o médico com os dados do request
                var result = await _medicoService.CadastrarMedicoAsync(request);
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


        // Endpoint para abrir a agenda do médico e disponibilizar horários
        [HttpPost("AbrirAgenda")]
        public async Task<IActionResult> AbrirAgenda([FromHeader] string Token)
        {
            try
            {
                var user = await _loginService.IdentityUserAsync(Token);
                if (user.perfil == EPerfil.Medico)
                    return Ok();

                return Unauthorized();

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar login.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }


        // Endpoint para abrir o médico visualizar a agenda do dia selecionado
        [HttpGet("VisualizarAgenda")]
        public async Task<IActionResult> VisualizarAgenda([FromBody] LoginRequest request)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar login.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }       
}

