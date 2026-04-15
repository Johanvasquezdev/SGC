import api from '@/lib/api';
import { CitaDTO, CreateCitaDTO } from '@/types/api.types';

export class CitaService {
  static async obtenerCitasPorPaciente(pacienteId: number): Promise<CitaDTO[]> {
    if (!pacienteId || pacienteId <= 0) return [];
    const response = await api.get<CitaDTO[]>(`/api/citas/paciente/${pacienteId}`);
    return response.data;
  }

  static async crearCita(citaData: CreateCitaDTO): Promise<CitaDTO> {
    const response = await api.post<CitaDTO>('/api/citas', citaData);
    return response.data;
  }
  static async obtenerCitasPorMedico(medicoId: number): Promise<CitaDTO[]> {
    const response = await api.get<CitaDTO[]>(`/api/citas/medico/${medicoId}`);
    return response.data;
  }
  static async cancelarCita(citaId: number, motivo: string): Promise<void> {
    await api.put(`/api/citas/${citaId}/cancelar`, { motivo });
  }

  static async reprogramarCita(citaId: number, fechaHora: string): Promise<void> {
    await api.put(`/api/citas/${citaId}/reprogramar`, { fechaHora });
  }

  static async confirmarCita(citaId: number): Promise<void> {
    await api.put(`/api/citas/${citaId}/confirmar`);
  }

  static async iniciarConsulta(citaId: number): Promise<void> {
    await api.put(`/api/citas/${citaId}/iniciar-consulta`);
  }

  static async marcarAsistencia(citaId: number, asistio: boolean): Promise<void> {
    await api.put(`/api/citas/${citaId}/asistencia?asistio=${asistio}`);
  }
}
