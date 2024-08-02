using HackathonHealthMed.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Domain.Interfaces
{
    public interface ILoginRepository
    {

        Task<Medico> GetByEmailAsync(string email);
        Task<Usuario> GetByIdentityAsync(string token);
        Task<Usuario> GetByIdAsync(long id);
    }
}
