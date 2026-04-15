"use client";
import { useState } from "react";
import {
  X,
  Loader2,
  CheckCircle2,
} from "lucide-react";
import { MedicoDTO } from "@/types/api.types";
import { CitaService } from "@/services/cita.service";
import { PagoService } from "@/services/pago.service";
import { useAuth } from "@/components/providers/AuthProvider";
import { UsuarioService } from "@/services/usuario.service";
import { DisponibilidadService } from "@/services/disponibilidad.service";
import { toast } from "sonner";
import { useRouter } from "next/navigation";

interface Props {
  medico: MedicoDTO | null;
  isOpen: boolean;
  onClose: () => void;
}

export function AgendarCitaModal({ medico, isOpen, onClose }: Props) {
  const { user } = useAuth();
  const [motivo, setMotivo] = useState("");
  const [fechaHora, setFechaHora] = useState("");
  const [notas, setNotas] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [citaCreadaId, setCitaCreadaId] = useState<number | null>(null);
  const [pagoLoading, setPagoLoading] = useState(false);
  const [pagoCreado, setPagoCreado] = useState(false);
  const [pacienteActualId, setPacienteActualId] = useState<number | null>(null);
  const router = useRouter();
  const fechaHoraId = "agendar-fecha-hora";
  const motivoId = "agendar-motivo";
  const notasId = "agendar-notas";

  if (!isOpen || !medico) return null;

  const toLocalIso = (value: string) => (value.length === 16 ? `${value}:00` : value);
  const toLocalDate = (value: string) => value.split("T")[0];

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    let pacienteId = user?.id || 0;
    if (!pacienteId) {
      try {
        const email = user?.email;
        const rol = user?.rol;
        if (email && rol) {
          const usuarios = await UsuarioService.obtenerTodos(rol);
          const encontrado = usuarios.find(
            (u) => u.email?.toLowerCase() === email.toLowerCase()
          );
          if (encontrado) pacienteId = encontrado.id;
        }
      } catch {
        // Si falla, mostramos el mensaje de sesion.
      }
    }
    if (!pacienteId) {
      setError("Debes iniciar sesion para agendar.");
      toast.error("Inicia sesion para agendar tu cita");
      return;
    }
    if (!fechaHora) {
      setError("Selecciona fecha y hora.");
      return;
    }
    setIsLoading(true);
    setError(null);
    try {
      const fechaParam = toLocalDate(fechaHora);
      const disponibilidades = await DisponibilidadService.obtenerPorMedico(
        medico.id,
        fechaParam
      );
      if (!disponibilidades.length) {
        setError("No hay disponibilidad para ese dia.");
        return;
      }
      const disponibilidadId = disponibilidades[0].id;
      const citaCreada = await CitaService.crearCita({
        pacienteId,
        medicoId: medico.id,
        fechaHora: toLocalIso(fechaHora),
        disponibilidadId,
        motivo,
        notas,
      });
      setPacienteActualId(pacienteId);
      setCitaCreadaId(citaCreada.id);
      setIsSuccess(true);
    } catch (err: any) {
      const mensaje =
        err?.response?.data?.mensaje ||
        err?.response?.data?.message ||
        "Ocurrio un error al agendar la cita.";
      setError(mensaje);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-slate-900/50 backdrop-blur-sm">
      <div className="bg-slate-950/95 rounded-2xl shadow-xl w-full max-w-md border border-slate-800 overflow-hidden">
        <div className="flex justify-between items-center p-4 border-b border-slate-800 bg-slate-950/90">
          <h2 className="text-lg font-semibold text-white">Agendar con {medico.nombre}</h2>
          <button type="button" onClick={onClose} aria-label="Cerrar modal" className="text-slate-400 hover:text-white transition-colors">
            <X aria-hidden="true" className="w-5 h-5" />
          </button>
        </div>
        <div className="p-6 text-slate-100">
          {isSuccess ? (
            <div className="text-center py-8">
              <CheckCircle2 className="w-16 h-16 text-emerald-500 mx-auto" />
              <h3 className="text-lg font-semibold text-white">Cita Agendada</h3>
              <p className="text-slate-400 text-sm mt-2">
                Puedes crear el pago ahora o verlo luego en Pagos.
              </p>
              <div className="mt-6 flex flex-col gap-3">
                <button
                  disabled={pagoLoading || pagoCreado || !citaCreadaId || !pacienteActualId}
                  onClick={async () => {
                    if (!citaCreadaId || !pacienteActualId) return;
                    try {
                      setPagoLoading(true);
                      await PagoService.crearIntento({
                        citaId: citaCreadaId,
                        pacienteId: pacienteActualId,
                        monto: 1000,
                        moneda: "DOP",
                      });
                      setPagoCreado(true);
                      toast.success("Intento de pago creado.");
                    } catch (e: any) {
                      toast.error("No se pudo crear el pago.");
                    } finally {
                      setPagoLoading(false);
                    }
                  }}
                  className="w-full bg-emerald-600 text-white py-3 rounded-xl hover:bg-emerald-500 transition-colors disabled:opacity-60"
                >
                  {pagoLoading ? "Creando pago..." : pagoCreado ? "Pago creado" : "Pagar ahora"}
                </button>
                <button
                  onClick={() => {
                    onClose();
                    router.push("/paciente/pagos");
                  }}
                  className="w-full bg-slate-800 text-slate-200 py-3 rounded-xl hover:bg-slate-700 transition-colors"
                >
                  Ver mis pagos
                </button>
              </div>
            </div>
          ) : (
            <form onSubmit={handleSubmit} className="space-y-4">
              <input
                id={fechaHoraId}
                aria-label="Fecha y hora"
                type="datetime-local"
                required
                className="w-full px-4 py-2 border border-slate-800 bg-slate-950/70 text-slate-100 rounded-xl focus:outline-none focus:ring-2 focus:ring-emerald-500"
                value={fechaHora}
                onChange={(e) => setFechaHora(e.target.value)}
              />
              <input
                id={motivoId}
                aria-label="Motivo de la cita"
                type="text"
                required
                placeholder="Motivo..."
                className="w-full px-4 py-2 border border-slate-800 bg-slate-950/70 text-slate-100 rounded-xl placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-emerald-500"
                value={motivo}
                onChange={(e) => setMotivo(e.target.value)}
              />
              <textarea
                id={notasId}
                aria-label="Notas adicionales"
                placeholder="Notas..."
                className="w-full px-4 py-2 border border-slate-800 bg-slate-950/70 text-slate-100 rounded-xl placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-emerald-500"
                value={notas}
                onChange={(e) => setNotas(e.target.value)}
              />
              {error && <div role="alert" aria-live="polite" className="text-red-500 text-sm">{error}</div>}
              <button
                disabled={isLoading}
                className="w-full bg-emerald-600 text-white py-3 rounded-xl flex justify-center hover:bg-emerald-500 transition-colors"
              >
                {isLoading ? (
                  <Loader2 className="animate-spin" />
                ) : (
                  "Confirmar Cita"
                )}
              </button>
            </form>
          )}
        </div>
      </div>
    </div>
  );
}

