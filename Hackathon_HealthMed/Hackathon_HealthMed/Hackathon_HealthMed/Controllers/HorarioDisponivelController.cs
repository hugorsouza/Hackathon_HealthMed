
using HackathonHealthMed.Application.DTO;
using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Service;
using HackathonHealthMed.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioDisponivelController : ControllerBase
    {
        private readonly IHorarioDisponivelService _horarioDisponivelService;

        public HorarioDisponivelController(IHorarioDisponivelService horarioDisponivelService)
        {
            _horarioDisponivelService = horarioDisponivelService;
        }

        [HttpGet("ObterHorariosPorMedico")]
        public async Task<IActionResult> ObterHorariosPorMedico(int medicoId)
        {
            var horarios = await _horarioDisponivelService.ObterHorariosPorMedicoAsync(medicoId);
            return Ok(horarios);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarHorario([FromBody] HorarioDisponivelDto horarioDisponivelDto)
        {
            var resultado = await _horarioDisponivelService.AdicionarHorarioAsync(horarioDisponivelDto);
            if (resultado == null)
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
