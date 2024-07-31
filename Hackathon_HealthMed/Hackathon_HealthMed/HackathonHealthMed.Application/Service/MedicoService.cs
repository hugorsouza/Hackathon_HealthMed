using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Application.Util;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Enums;
using HackathonHealthMed.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<MedicoService> _logger;

        public MedicoService(IMedicoRepository medicoRepository, IPasswordHasher passwordHasher, ILogger<MedicoService> logger)
        {
            _medicoRepository = medicoRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

                

        public async Task<int> AtualizarMedicoAsync(Medico medico)
        {
            var result = await _medicoRepository.UpdateAsync(medico);
            _logger.LogInformation("Dados do médico atualizados com sucesso.");
            return result;
        }

        public async Task<int> CadastrarMedicoAsync(CadastrarMedicoRequest medico)
        {
            var senhaHash = _passwordHasher.HashPassword(medico.Senha);
            var identity = IdentityGenerator.Perform(senhaHash);

            
            var result = await _medicoRepository.AddAsync(medico, EPerfil.Medico, senhaHash );

            await _medicoRepository.UpdateIdentity(result, identity);

            _logger.LogInformation("Médico cadastrado com sucesso.");
            return result;
        }
    }

 

    public class HorarioDisponivelService
    {
       
    }
}
