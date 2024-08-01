using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Domain.Interfaces
{
    public interface IPacienteRepository
    {
        Task<int> AddAsync(Paciente paciente);
        Task<int> AddAsync(CadastrarPacienteRequest paciente, EPerfil perfil, string senhaHash);
        Task<Paciente> GetByEmailAsync(string email);
        Task<Paciente> GetByIdAsync(int id);
        Task UpdateIdentity(long medicoId, string identity);
    }
}
