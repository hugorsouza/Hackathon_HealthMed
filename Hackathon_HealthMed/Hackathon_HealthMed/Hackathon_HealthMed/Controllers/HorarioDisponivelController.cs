
using HackathonHealthMed.Application;
using HackathonHealthMed.Application.DTO;
using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Service;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Hackathon_HealthMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioDisponivelController : ControllerBase
    {
        private readonly IHorarioDisponivelService _horarioDisponivelService;
        private readonly ILoginService _loginService;

        public HorarioDisponivelController(IHorarioDisponivelService horarioDisponivelService, ILoginService loginService)
        {
            _horarioDisponivelService = horarioDisponivelService;
            _loginService = loginService;
        }

        [HttpGet("ObterHorariosPorMedico")]
        public async Task<IActionResult> ObterHorariosPorMedico(int medicoId, string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Paciente) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }

            var horarios = await _horarioDisponivelService.ObterHorariosPorMedicoAsync(medicoId);
            return Ok(horarios);

        }

        [HttpPost("AdicionarHorario")]
        public async Task<IActionResult> AdicionarHorario([FromBody] HorarioDisponivelDto horarioDisponivelDto, string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Medico) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }
            
            var resultado = await _horarioDisponivelService.AdicionarHorarioAsync(horarioDisponivelDto);
            if (!resultado)
            {
                return Conflict("Horário já está ocupado.");
            }
            return Ok("Horário adicionado!");
        }

        [HttpPut("AtualizarHorario")]
        public async Task<IActionResult> AtualizarHorario([FromBody] HorarioDisponivelDto horarioDisponivelDto, string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Medico) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }

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
        public async Task<IActionResult> ObterHorariosPorNomeMedico(string nome, string token)
        {

            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Paciente) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }
                 var horarios = await _horarioDisponivelService.ObterHorariosPorNomeMedicoAsync(nome);
                 if (horarios == null || !horarios.Any())
                 {
                     return NotFound("Nenhum horário encontrado para o médico especificado.");
                 }

                 return Ok(horarios);
            
        }

        [HttpGet("ObterMedicoDisponiveis")]
        public async Task<IActionResult> ObterMedicosComHorariosDisponiveis(string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Paciente) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }

                try
                {
                    var medicos = await _horarioDisponivelService.ObterMedicosComHorariosDisponiveisAsync();
                    return Ok(medicos);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Erro interno do servidor.");
                }
        }

        [HttpDelete("DeletarHorario/{id}")]
        public async Task<IActionResult> DeletarHorario(int id, string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Medico) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }

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
        public async Task<IActionResult> AgendarConsulta([FromBody] HorarioDisponivelDto consultaDto, string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Paciente) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }

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
        public async Task<IActionResult> DesmarcarConsulta(int id, int pacienteId, string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Paciente) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }

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
        public async Task<IActionResult> ExibirConsultas(int pacienteId, string token)
        {
            var user = await _loginService.IdentityUserAsync(token);
            if (user == null || user.perfil != EPerfil.Paciente) // Verifica se o usuário é nulo ou não é paciente
            {
                return Unauthorized();
            }

                var consultas = await _horarioDisponivelService.ExibirConsultasAsync(pacienteId);
                return Ok(consultas);
           
        }
    }
}
