using HackathonHealthMed.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Domain.Interfaces
{
    public interface IHorarioDisponivelRepository
    {
        Task<int> ObterPorMedicoAsync(int horarioDisponivel);
        Task<HorarioDisponivel> ObterPorIdAsync(int id);
        Task<bool> AdicionarAsync(HorarioDisponivel horarioDisponivel);
        Task AtualizarAsync(HorarioDisponivel horarioDisponivel);
        Task<bool> VerificarDisponibilidadeAsync(int medicoId, DateTime data, TimeSpan horaInicio);
    }
}
