using HackathonHealthMed.Application.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<int> CadastrarPacienteAsync(CadastrarPacienteRequest paciente);
    }
}
