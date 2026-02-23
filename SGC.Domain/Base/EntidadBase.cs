using System;

namespace SGC.Domain.Base
{

    // Clase base para todas las entidades del sistema, proporcionando propiedades comunes
    public abstract class EntidadBase
    {
        public int Id { get; set; } // Identificador único para cada entidad
        public DateTime FechaCreacion { get; set; } = DateTime.Now; // Fecha de creación de la entidad
    }
}