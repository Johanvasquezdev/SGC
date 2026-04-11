"use client";

import { useState, useEffect } from "react";
import { Calendar as CalendarIcon, Clock, MapPin, Loader2 } from "lucide-react";
import { CitaDTO, EstadoCita } from "@/types/api.types";
import { CitaService } from "@/services/cita.service";
import { useAuth } from "@/components/providers/AuthProvider";
import { useSignalR } from "@/hooks/useSignalR";
import { toast } from "sonner";
import dayjs from "dayjs";

export default function MisCitasPage() {
  const { user } = useAuth();
  const [citas, setCitas] = useState<CitaDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // Escucha eventos del hub para notificar cambios en tiempo real.
  const signalRBaseUrl =
    process.env.NEXT_PUBLIC_API_URL || process.env.NEXT_PUBLIC_API_BASE_URL;

  useSignalR({
    hubUrl: signalRBaseUrl ? `${signalRBaseUrl.replace(/\/$/, "")}/citahub` : "",
    onNuevaCita: () => {
      toast.info("Nueva cita registrada");
    },
  });

  useEffect(() => {
    const fetchCitas = async () => {
      if (!user?.id) {
        setIsLoading(false);
        return;
      }
      try {
        const data = await CitaService.obtenerCitasPorPaciente(user.id);
        setCitas(data);
      } catch (error) {
        console.error(error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchCitas();
  }, [user?.id]);

  const cancelar = async (id: number) => {
    const motivo = window.prompt("Motivo de cancelación:");
    if (!motivo) return;
    try {
      await CitaService.cancelarCita(id, motivo);
      setCitas((prev) => prev.map((c) => (c.id === id ? { ...c, estado: EstadoCita.Cancelada } : c)));
    } catch (e) {
      toast.error("No se pudo cancelar la cita.");
    }
  };

  const reprogramar = async (id: number) => {
    const nuevaFecha = window.prompt("Nueva fecha y hora (YYYY-MM-DD HH:mm):");
    if (!nuevaFecha) return;
    const iso = new Date(nuevaFecha.replace(" ", "T")).toISOString();
    try {
      await CitaService.reprogramarCita(id, iso);
      setCitas((prev) =>
        prev.map((c) => (c.id === id ? { ...c, fechaHora: iso, estado: EstadoCita.Solicitada } : c))
      );
    } catch (e) {
      toast.error("No se pudo reprogramar la cita.");
    }
  };

  return (
    <div className="p-6 max-w-5xl mx-auto space-y-6">
      <header>
        <h1 className="text-3xl font-bold tracking-tight text-foreground">Mis Citas</h1>
        <p className="text-muted-foreground mt-1">Historial y próximas consultas programadas.</p>
      </header>

      {isLoading ? (
        <div className="py-12 flex justify-center"><Loader2 className="w-8 h-8 animate-spin text-emerald-600" /></div>
      ) : citas.length === 0 ? (
        <div className="text-center py-12 bg-card rounded-2xl border border-border">
          <CalendarIcon className="w-12 h-12 text-muted-foreground mx-auto mb-3" />
          <p className="text-muted-foreground">No tienes citas agendadas.</p>
        </div>
      ) : (
        <div className="grid gap-4">
          {citas.map(cita => (
            <div key={cita.id} className="bg-card p-6 rounded-2xl border border-border flex flex-col md:flex-row gap-6 justify-between items-center hover:shadow-md transition-shadow">
              <div className="flex gap-4 items-center">
                <div className="bg-emerald-500/10 w-16 h-16 rounded-2xl flex flex-col items-center justify-center text-emerald-600 dark:text-emerald-400 font-bold border border-emerald-500/20">
                  <span className="text-sm">{dayjs(cita.fechaHora).format('MMM')}</span>
                  <span className="text-xl leading-none">{dayjs(cita.fechaHora).format('DD')}</span>
                </div>
                <div>
                  <h3 className="font-semibold text-lg text-foreground">Consulta Médica</h3>
                  <div className="flex gap-3 text-sm text-muted-foreground mt-1">
                    <span className="flex items-center gap-1"><Clock className="w-4 h-4" /> {dayjs(cita.fechaHora).format('hh:mm A')}</span>
                    <span className="flex items-center gap-1"><MapPin className="w-4 h-4" /> Consultorio Principal</span>
                  </div>
                </div>
              </div>
              
              <div className="flex items-center gap-4 w-full md:w-auto justify-end">
                <span className={`px-3 py-1 rounded-full text-xs font-semibold ${
                  cita.estado === EstadoCita.Confirmada ? 'bg-emerald-100 text-emerald-700' :
                  cita.estado === EstadoCita.Solicitada ? 'bg-amber-100 text-amber-700' : 'bg-slate-100 text-slate-700'
                }`}>
                  {cita.estado}
                </span>
                {(cita.estado === EstadoCita.Confirmada || cita.estado === EstadoCita.Solicitada) && (
                  <div className="flex gap-2">
                    <button
                      onClick={() => reprogramar(cita.id)}
                      className="text-sm font-medium text-emerald-300 hover:text-emerald-200 bg-emerald-500/10 hover:bg-emerald-500/20 px-4 py-2 rounded-lg transition-colors border border-emerald-500/30"
                    >
                      Reprogramar
                    </button>
                    <button
                      onClick={() => cancelar(cita.id)}
                      className="text-sm font-medium text-rose-300 hover:text-rose-200 bg-rose-500/10 hover:bg-rose-500/20 px-4 py-2 rounded-lg transition-colors border border-rose-500/30"
                    >
                      Cancelar
                    </button>
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

