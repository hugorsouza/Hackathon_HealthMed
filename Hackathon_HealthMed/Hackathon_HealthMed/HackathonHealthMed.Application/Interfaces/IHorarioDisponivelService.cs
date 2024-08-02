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
        Task<HorarioDisponivel> ObterHorariosPorMedicoAsync(int medicoId);
        Task<bool> AdicionarHorarioAsync(HorarioDisponivelDto horarioDisponivel);
        Task AtualizarHorarioAsync(HorarioDisponivelDto horarioDisponivel);
        Task<bool> DeletarHorarioAsync(int id);
        Task<IEnumerable<HorarioDisponivelDto>> ObterHorariosPorNomeMedicoAsync(string nome);
        Task<bool> AgendarConsultaAsync(HorarioDisponivelDto consultaDto);
        Task<bool> DesmarcarConsultaAsync(int id, int pacienteId);
        Task<HorarioDisponivel> ExibirConsultasAsync(int pacienteId);
        Task<IEnumerable<HorarioDisponivel>> ObterMedicosComHorariosDisponiveisAsync();
    }
}
