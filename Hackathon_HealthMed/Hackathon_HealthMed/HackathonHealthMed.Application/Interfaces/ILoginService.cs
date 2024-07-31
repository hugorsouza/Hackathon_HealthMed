using HackathonHealthMed.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Interfaces
{
    public interface ILoginService
    {
        Task<string> AutenticarAsync(string email, string password);
        Task<Usuario> IdentityUserAsync(string token);
    }
}
