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
        Task<int> AddAsync(HorarioDisponivel horario);
        Task<int> UpdateAsync(HorarioDisponivel horario);
        Task<IEnumerable<HorarioDisponivel>> GetByMedicoIdAsync(int medicoId);
    }
}
