using SGC.Domain.Entities.Security;
using SGC.Domain.Entities.Appointments;
using System.Collections.Generic;

namespace SGC.Domain.Entities.Medical
{

    public class Medico : Usuario // El médico es un tipo de usuario 
    {
        public string Exequatur { get; set; }
        public int EspecialidadId { get; set; }
        public string TelefonoConsultorio { get; set; } 
        public List<Disponibilidad> Horarios { get; set; } 
    }
}