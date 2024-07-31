using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Application.Util;
using HackathonHealthMed.Domain.Enums;
using HackathonHealthMed.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Service
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<PacienteService> _logger;

        public PacienteService(IPacienteRepository pacienteRepository, IPasswordHasher passwordHasher, ILogger<PacienteService> logger)
        {
            _pacienteRepository = pacienteRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<int> CadastrarPacienteAsync(CadastrarPacienteRequest paciente)
        {
            var senhaHash = _passwordHasher.HashPassword(paciente.Senha);
            var identity = IdentityGenerator.Perform(senhaHash);


            var result = await _pacienteRepository.AddAsync(paciente, EPerfil.Paciente, senhaHash);

            await _pacienteRepository.UpdateIdentity(result, identity);

            _logger.LogInformation("Paciente cadastrado com sucesso.");
            return result;
        }

    }
}
