
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

        [HttpPost("AdicionarHorario")]
        public async Task<IActionResult> AdicionarHorario([FromBody] HorarioDisponivelDto horarioDisponivelDto)
        {
            var resultado = await _horarioDisponivelService.AdicionarHorarioAsync(horarioDisponivelDto);
            if (!resultado)
            {
                return Conflict("Horário já está ocupado.");
            }
            return Ok();
        }

        [HttpPut("AtualizarHorario")]
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

        [HttpGet("ObterHorariosPorNomeMedico/{nome}")]
        public async Task<IActionResult> ObterHorariosPorNomeMedico(string nome)
        {
            var horarios = await _horarioDisponivelService.ObterHorariosPorNomeMedicoAsync(nome);
            if (horarios == null || !horarios.Any())
            {
                return NotFound("Nenhum horário encontrado para o médico especificado.");
            }

            return Ok(horarios);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarHorario(int id)
        {
            try
            {
                var sucesso = await _horarioDisponivelService.DeletarHorarioAsync(id);
                if (sucesso)
                {
                    return NoContent(); // Retorna 204 No Content se o delete foi bem-sucedido
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor");
            }
        }

        [HttpPost]
        [Route("AgendarConsulta")]
        public async Task<IActionResult> AgendarConsulta([FromBody] HorarioDisponivelDto consultaDto)
        {
            try
            {
                var sucesso = await _horarioDisponivelService.AgendarConsultaAsync(consultaDto);
                if (!sucesso)
                    return BadRequest("Horário não disponível.");

                return Ok("Consulta agendada com sucesso.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao agendar consulta: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("DesmarcarConsulta")]
        public async Task<IActionResult> DesmarcarConsulta(int id, int pacienteId)
        {
            try
            {
                var sucesso = await _horarioDisponivelService.DesmarcarConsultaAsync(id, pacienteId);
                if (!sucesso)
                    return BadRequest("Consulta não localizada não disponível.");

                return Ok("Cancelamento realizado com sucesso.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao desmarcar consulta: {ex.Message}");
            }
        }

        [HttpGet("ExibirConsultas")]
        public async Task<IActionResult> ExibirConsultas(int pacienteId)
        {
            var consultas = await _horarioDisponivelService.ExibirConsultasAsync(pacienteId);
            return Ok(consultas);
        }
    }
}
