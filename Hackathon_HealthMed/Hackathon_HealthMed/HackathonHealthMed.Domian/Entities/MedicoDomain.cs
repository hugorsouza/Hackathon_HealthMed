﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Domain.Entities
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string NumeroCRM { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public string Identity { get; set; }
    }

}
