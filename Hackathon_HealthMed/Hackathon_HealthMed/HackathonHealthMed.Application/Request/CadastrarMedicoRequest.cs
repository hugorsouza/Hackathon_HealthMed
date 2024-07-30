using HackathonHealthMed.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Request
{
    public class CadastrarMedicoRequest
    {
        public Medico Medico { get; set; }
        public string Senha { get; set; }
    }
}
