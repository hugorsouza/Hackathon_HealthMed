using HackathonHealthMed.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Domain.Entities
{
    public class Usuario
    {
        public long Id { get; set; }
        public string identity { get; set; }
        public EPerfil perfil { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
    }
}
