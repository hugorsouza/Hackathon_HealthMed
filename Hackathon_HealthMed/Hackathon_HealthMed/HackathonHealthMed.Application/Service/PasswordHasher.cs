using HackathonHealthMed.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Service
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // Hash a senha usando o BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            // Verifica se a senha fornecida corresponde ao hash armazenado
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}
