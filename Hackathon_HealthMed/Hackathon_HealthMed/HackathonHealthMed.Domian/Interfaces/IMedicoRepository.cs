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
    public interface IMedicoRepository
    {
        Task<int> AddAsync(CadastrarMedicoRequest medico, EPerfil perfil, string senhaHash);
        Task<Medico> GetByEmailAsync(string email);
        Task<Medico> GetByIdAsync(int id);
        Task<int> UpdateAsync(Medico medico);
        Task UpdateIdentity(long medicoId, string identity);
    }
}
