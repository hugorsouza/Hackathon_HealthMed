using HackathonHealthMed.Application.DTO;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Interfaces
{
    public interface IHorarioDisponivelService
    {
        Task<int> ObterHorariosPorMedicoAsync(int horarioDisponivel);
        Task<bool> AdicionarHorarioAsync(HorarioDisponivelDto horarioDisponivel);
        Task AtualizarHorarioAsync(HorarioDisponivelDto horarioDisponivel);
        Task<bool> DeletarHorarioAsync(int id);
    }
}
