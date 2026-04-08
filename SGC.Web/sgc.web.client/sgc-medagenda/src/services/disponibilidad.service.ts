import api from '@/lib/api';
import { DisponibilidadDTO } from '@/types/api.types';

export class DisponibilidadService {
  static async obtenerPorMedico(medicoId: number, fecha?: string): Promise<DisponibilidadDTO[]> {
    const params = fecha ? `?fecha=${fecha}` : '';
    const response = await api.get<DisponibilidadDTO[]>(`/api/disponibilidad/medico/${medicoId}${params}`);
    return response.data;
  }
}
