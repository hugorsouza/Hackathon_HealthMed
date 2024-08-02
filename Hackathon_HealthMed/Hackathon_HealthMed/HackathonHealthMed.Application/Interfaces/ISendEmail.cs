using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.Interfaces
{
    public interface ISendEmail
    {
        Task Send(long pacienteId, long medicoId, DateTime date);
    }
}
