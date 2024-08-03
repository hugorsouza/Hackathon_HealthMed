using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Service
{
    public class LoginService : ILoginService
    {
        
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<LoginService> _logger;
        private readonly ILoginRepository _loginRepository;
        private readonly ISendEmail _sendEmail;


        public LoginService(IPasswordHasher passwordHasher, ILogger<LoginService> logger, ILoginRepository loginRepository, ISendEmail sendEmail)
        {           
            _passwordHasher = passwordHasher;
            _logger = logger;
            _loginRepository = loginRepository;
            _sendEmail = sendEmail;
        }
        public async Task<string> AutenticarAsync(string email, string password)
        {
            var medico = await _loginRepository.GetByEmailAsync(email);
            if (medico != null && _passwordHasher.VerifyHashedPassword(medico.SenhaHash, password))
            {
                _logger.LogInformation("Autenticação de médico bem-sucedida.");
                 return medico.Identity;
            }

            _logger.LogWarning("Falha na autenticação do médico.");
            return null;
        }

        public async Task<Usuario> IdentityUserAsync(string token)
        {
            token = token.Substring(token.Length - 60, 60);

            return await _loginRepository.GetByIdentityAsync(token);
        }
    }
}
