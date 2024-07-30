using HackathonHealthMed.Domain.Entities;
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
        Task<Paciente> GetByEmailAsync(string email);
        Task<Paciente> GetByIdAsync(int id);
    }
}
