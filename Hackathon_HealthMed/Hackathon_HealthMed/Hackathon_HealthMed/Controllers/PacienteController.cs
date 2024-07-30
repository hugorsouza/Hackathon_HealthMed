using HackathonHealthMed.Application.Request;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : Controller
    {

        // Endpoint para abrir a agenda do médico e disponibilizar horários
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> ConsultarMedicosDisponiveis([FromBody] LoginRequest request)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar login.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }


        // Endpoint para abrir a agenda do médico e disponibilizar horários
        [HttpPost("ConsultarMedicosDisponiveis")]
        public async Task<IActionResult> ConsultarMedicosDisponiveis([FromBody] LoginRequest request)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar login.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }




        // Endpoint para abrir a agenda do médico e disponibilizar horários
        [HttpPost("AgendarConsulta")]
        public async Task<IActionResult> AgendarConsulta([FromBody] LoginRequest request)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar login.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
