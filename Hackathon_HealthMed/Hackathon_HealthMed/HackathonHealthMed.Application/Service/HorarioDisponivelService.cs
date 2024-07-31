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

        public HorarioDisponivelService(IHorarioDisponivelRepository horarioDisponivelRepository)
        {
            _horarioDisponivelRepository = horarioDisponivelRepository;
        }

        public async Task<int> ObterHorariosPorMedicoAsync(int horarioDisponivel)
        {
            return await _horarioDisponivelRepository.ObterPorMedicoAsync(horarioDisponivel);
        }

        public async Task<bool> AdicionarHorarioAsync(HorarioDisponivelDto horarioDisponivelDto)
        {
            var disponibilidade = await _horarioDisponivelRepository.VerificarDisponibilidadeAsync(
                horarioDisponivelDto.MedicoId, horarioDisponivelDto.Data, horarioDisponivelDto.HoraInicio);

            if (!disponibilidade)
            {
                return false; // Horário já ocupado
            }

            var horarioDisponivel = new HorarioDisponivel
            {
                MedicoId = horarioDisponivelDto.MedicoId,
                Data = horarioDisponivelDto.Data,
                HoraInicio = horarioDisponivelDto.HoraInicio,
                HoraFim = horarioDisponivelDto.HoraFim,
                EstaDisponivel = true
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

            horario.Data = horarioDisponivelDto.Data;
            horario.HoraInicio = horarioDisponivelDto.HoraInicio;
            horario.HoraFim = horarioDisponivelDto.HoraFim;
            horario.EstaDisponivel = horarioDisponivelDto.Disponivel;

            await _horarioDisponivelRepository.AtualizarAsync(horario);
        }
    }
}
