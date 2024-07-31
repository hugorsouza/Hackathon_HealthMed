using HackathonHealthMed.Application.DTO;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Service
{
    public class HorarioDisponivelService
    {
        private readonly IHorarioDisponivelRepository _horarioDisponivelRepository;

        public HorarioDisponivelService(IHorarioDisponivelRepository horarioDisponivelRepository)
        {
            _horarioDisponivelRepository = horarioDisponivelRepository;
        }

        public async Task<IEnumerable<HorarioDisponivel>> ObterHorariosPorMedicoAsync(int medicoId)
        {
            return await _horarioDisponivelRepository.ObterPorMedicoAsync(medicoId);
        }

        public async Task<bool> AdicionarHorarioAsync(HorarioDisponivelDto horarioDto)
        {
            var disponibilidade = await _horarioDisponivelRepository.VerificarDisponibilidadeAsync(
                horarioDto.MedicoId, horarioDto.Data, horarioDto.HoraInicio);

            if (!disponibilidade)
            {
                return false; // Horário já ocupado
            }

            var horarioDisponivel = new HorarioDisponivel
            {
                MedicoId = horarioDto.MedicoId,
                Data = horarioDto.Data,
                HoraInicio = horarioDto.HoraInicio,
                HoraFim = horarioDto.HoraFim,
                EstaDisponivel = true
            };

            await _horarioDisponivelRepository.AdicionarAsync(horarioDisponivel);
            return true;
        }

        public async Task AtualizarHorarioAsync(HorarioDisponivelDto horarioDto)
        {
            var horario = await _horarioDisponivelRepository.ObterPorIdAsync(horarioDto.Id);
            if (horario == null)
            {
                throw new KeyNotFoundException("Horário não encontrado.");
            }

            horario.Data = horarioDto.Data;
            horario.HoraInicio = horarioDto.HoraInicio;
            horario.HoraFim = horarioDto.HoraFim;
            horario.EstaDisponivel = horarioDto.Disponivel;

            await _horarioDisponivelRepository.AtualizarAsync(horario);
        }
    }

}
