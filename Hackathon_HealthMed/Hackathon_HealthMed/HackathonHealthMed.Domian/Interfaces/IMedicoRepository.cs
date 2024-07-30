using HackathonHealthMed.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Domain.Interfaces
{
    public interface IMedicoRepository
    {
        Task<int> AddAsync(Medico medico);
        Task<Medico> GetByEmailAsync(string email);
        Task<Medico> GetByIdAsync(int id);
        Task<int> UpdateAsync(Medico medico);
    }
}
