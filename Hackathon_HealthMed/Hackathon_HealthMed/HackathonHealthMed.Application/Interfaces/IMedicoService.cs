using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Interfaces
{
    public interface IMedicoService
    {
        Task<int> CadastrarMedicoAsync(CadastrarMedicoRequest medico);
    }
}
