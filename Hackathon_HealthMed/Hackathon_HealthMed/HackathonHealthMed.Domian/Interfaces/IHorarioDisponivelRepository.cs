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

        Task<HorarioDisponivel> ObterPorMedicoAsync(int medicoId);
        Task<HorarioDisponivel> ObterPorIdAsync(int id);
        Task<bool> AdicionarAsync(HorarioDisponivel horarioDisponivel);
        Task AtualizarAsync(HorarioDisponivel horarioDisponivel);
        Task<bool> VerificarDisponibilidadeAsync(int medicoId, DateTime horario);
        Task<bool> DeletarAsync(int id);
        Task<IEnumerable<HorarioDisponivel>> ObterPorNomeMedicoAsync(string nome);
        Task<bool> AgendarConsultaAsync(HorarioDisponivel consulta);
        Task<HorarioDisponivel> ObterPacientePorIdAsync(int pacienteId);
        Task<bool> VerificarDisponibilidadeConsultaAsync(int medicoId, int pacienteId);
        Task<bool> CancelarConsultaAsync(int id, int pacienteId);
        Task<HorarioDisponivel> ExibirConsultasAsync(int pacienteId);

    }
}
