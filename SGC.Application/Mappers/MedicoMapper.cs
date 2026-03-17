using SGC.Application.DTOs.Medical;
using SGC.Domain.Entities.Medical;

namespace SGC.Application.Mappers
{
    public static class MedicoMapper
    {
        public static MedicoResponse ToResponse(Medico medico)
        {
            return new MedicoResponse
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Email = medico.Email,
                Exequatur = medico.Exequatur,
                EspecialidadId = medico.EspecialidadId,
                NombreEspecialidad = medico.Especialidad?.Nombre,
                ProveedorSaludId = medico.ProveedorSaludId,
                TelefonoConsultorio = medico.TelefonoConsultorio,
                Foto = medico.Foto,
                Activo = medico.Activo
            };
        }

        public static Medico ToEntity(CrearMedicoRequest request)
        {
            return new Medico
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = request.Password,
                Exequatur = request.Exequatur,
                EspecialidadId = request.EspecialidadId,
                ProveedorSaludId = request.ProveedorSaludId,
                TelefonoConsultorio = request.TelefonoConsultorio
            };
        }

        public static void UpdateEntity(Medico medico,
            ActualizarMedicoRequest request)
        {
            medico.Nombre = request.Nombre ?? medico.Nombre;
            medico.Exequatur = request.Exequatur ?? medico.Exequatur;
            medico.EspecialidadId = request.EspecialidadId ?? medico.EspecialidadId;
            medico.TelefonoConsultorio = request.TelefonoConsultorio ?? medico.TelefonoConsultorio;
            medico.Foto = request.Foto ?? medico.Foto;
        }
    }
}