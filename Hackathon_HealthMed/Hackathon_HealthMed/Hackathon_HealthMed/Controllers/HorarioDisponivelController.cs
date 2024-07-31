
using HackathonHealthMed.Application.DTO;
using HackathonHealthMed.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioDisponivelController : ControllerBase
    {
        private readonly HorarioDisponivelService _horarioDisponivelService;

        public HorarioDisponivelController(HorarioDisponivelService horarioDisponivelService)
        {
            _horarioDisponivelService = horarioDisponivelService;
        }

        [HttpGet("{medicoId}")]
        public async Task<IActionResult> ObterHorariosPorMedico(int medicoId)
        {
            var horarios = await _horarioDisponivelService.ObterHorariosPorMedicoAsync(medicoId);
            return Ok(horarios);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarHorario([FromBody] HorarioDisponivelDto horarioDisponivelDto)
        {
            var resultado = await _horarioDisponivelService.AdicionarHorarioAsync(horarioDisponivelDto);
            if (!resultado)
            {
                return Conflict("Horário já está ocupado.");
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarHorario([FromBody] HorarioDisponivelDto horarioDisponivelDto)
        {
            try
            {
                await _horarioDisponivelService.AtualizarHorarioAsync(horarioDisponivelDto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Horário não encontrado.");
            }
        }
    }

}
