using HackathonHealthMed.Application.DTO;
using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Service
{
    public class HorarioDisponivelService : IHorarioDisponivelService
    {
        private readonly IHorarioDisponivelRepository _horarioDisponivelRepository;
        private readonly ISendEmail _sendEmail;

        public HorarioDisponivelService(IHorarioDisponivelRepository horarioDisponivelRepository, ISendEmail sendEmail)
        {
            _horarioDisponivelRepository = horarioDisponivelRepository;
            _sendEmail = sendEmail;
        }

        public async Task<IEnumerable<HorarioDisponivel>> ObterHorariosPorMedicoAsync(int medicoId)
        {
            return (IEnumerable<HorarioDisponivel>)await _horarioDisponivelRepository.ObterPorMedicoAsync(medicoId);
        }

        public async Task<bool> AdicionarHorarioAsync(HorarioDisponivelDto horarioDisponivelDto)
        {
            var disponibilidade = await _horarioDisponivelRepository.VerificarDisponibilidadeAsync(
                horarioDisponivelDto.MedicoId, horarioDisponivelDto.Horario);

            if (!disponibilidade)
            {
                return false; // Horário já ocupado
            }

            var horarioDisponivel = new HorarioDisponivel
            {
                MedicoId = horarioDisponivelDto.MedicoId,
                Horario = horarioDisponivelDto.Horario
            };

            await _horarioDisponivelRepository.AdicionarAsync(horarioDisponivel);
            return true;
        }

        public async Task AtualizarHorarioAsync(HorarioDisponivelDto horarioDisponivelDto)
        {
            var horario = await _horarioDisponivelRepository.ObterPorIdAsync(horarioDisponivelDto.Id);
            if (horario == null)
            {
                throw new KeyNotFoundException("Horário não encontrado.");
            }

            horario.Horario = horarioDisponivelDto.Horario;

            await _horarioDisponivelRepository.AtualizarAsync(horario);
        }

        public async Task<bool> DeletarHorarioAsync(int id)
        {
            var horario = await _horarioDisponivelRepository.ObterPorIdAsync(id);
            if (horario == null)
            {
                throw new KeyNotFoundException("Horário não encontrado.");
            }

            return await _horarioDisponivelRepository.DeletarAsync(id);
        }

        public async Task<IEnumerable<HorarioDisponivelDto>> ObterHorariosPorNomeMedicoAsync(string nome)
        {
            var horarios = await _horarioDisponivelRepository.ObterPorNomeMedicoAsync(nome);

            return horarios.Select(h => new HorarioDisponivelDto
            {
                Nome = h.Nome,
                MedicoId = h.MedicoId,
                Horario = h.Horario,
                Id = h.Id
            });
        }

        public async Task<bool> AgendarConsultaAsync(HorarioDisponivelDto consultaDto)
        {
            // Verifica se o médico existe
            var medico = await _horarioDisponivelRepository.ObterPorMedicoAsync(consultaDto.MedicoId);
            if (medico == null)
                throw new KeyNotFoundException("Médico não encontrado.");

            // Verifica se o paciente existe
            var paciente = await _horarioDisponivelRepository.ObterPacientePorIdAsync(consultaDto.PacienteId);
            if (paciente == null)
                throw new KeyNotFoundException("Paciente não encontrado.");

            // Verifica se o horário está disponível
            var disponibilidade = await _horarioDisponivelRepository.VerificarDisponibilidadeConsultaAsync(consultaDto.MedicoId, consultaDto.PacienteId);
            if (!disponibilidade)
                return false; // Horário não disponível

            // Cria a consulta
            var consulta = new HorarioDisponivel
            {
                MedicoId = consultaDto.MedicoId,
                PacienteId = consultaDto.PacienteId,
                Horario = consultaDto.Horario,
                Id = consultaDto.Id
            };

            var result = await _horarioDisponivelRepository.AgendarConsultaAsync(consulta);
            if (result == true)
                await _sendEmail.Send((long)consultaDto.PacienteId, (long)consultaDto.MedicoId, consultaDto.Horario);

            return result;
        }

        public async Task<bool> DesmarcarConsultaAsync(int id, int pacienteId)
        {
            var disponibilidade = await _horarioDisponivelRepository.CancelarConsultaAsync(id, pacienteId);
            if (!disponibilidade)
                return false; // Horário não disponível

            return true;
        }

        public async Task<HorarioDisponivel> ExibirConsultasAsync(int pacienteId)
        {
            return await _horarioDisponivelRepository.ExibirConsultasAsync(pacienteId);
        }

        public async Task<IEnumerable<HorarioDisponivel>> ObterMedicosComHorariosDisponiveisAsync()
        {
            return await _horarioDisponivelRepository.ObterMedicosComHorariosDisponiveisAsync();
        }
    }
}
