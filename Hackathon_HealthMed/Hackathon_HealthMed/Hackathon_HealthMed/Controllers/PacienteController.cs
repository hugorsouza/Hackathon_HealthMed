using HackathonHealthMed.Application;
using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : Controller
    {
        private readonly ILogger<MedicoController> _logger;
        private readonly IPacienteService _pacienteService;
        public PacienteController(ILogger<MedicoController> logger, IPacienteService pacienteService)
        {
            _logger = logger;
            _pacienteService = pacienteService;
        }


        // Endpoint para abrir a agenda do médico e disponibilizar horários
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarPacienteRequest request)
        {
            try
            {
                // Chama o serviço para cadastrar o médico com os dados do request
                var result = await _pacienteService.CadastrarPacienteAsync(request);
                if (result > 0)
                {
                    _logger.LogInformation("Paciente cadastrado com sucesso.");
                    return Ok(new { success = true });
                }
                return BadRequest(new { success = false, message = "Erro ao cadastrar Paciente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar Paciente.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
