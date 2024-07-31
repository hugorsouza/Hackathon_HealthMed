using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Application.DTO
{
    public class HorarioDisponivelDto
    {
        public int Id { get; set; } 
        public int MedicoId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public bool Disponivel { get; set; }  
    }
}
