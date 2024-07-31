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
        Task<IEnumerable<HorarioDisponivel>> ObterPorMedicoAsync(int medicoId);
        Task<HorarioDisponivel> ObterPorIdAsync(int id);
        Task AdicionarAsync(HorarioDisponivel horarioDisponivel);
        Task AtualizarAsync(HorarioDisponivel horarioDisponivel);
        Task<bool> VerificarDisponibilidadeAsync(int medicoId, DateTime data, TimeSpan horaInicio);
    }
}
