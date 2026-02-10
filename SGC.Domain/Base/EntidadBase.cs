using System;

namespace SGC.Domain.Base
{
    public abstract class EntidadBase
    {
        public int Id { get; set; } 
        public DateTime FechaCreacion { get; set; } = DateTime.Now; 
    }
}