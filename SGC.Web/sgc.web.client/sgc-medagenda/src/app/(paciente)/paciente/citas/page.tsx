"use client";

import { useState, useEffect } from "react";
import { Calendar, Clock, MapPin, Loader2 } from "lucide-react";
import { CitaDTO, EstadoCita } from "@/types/api.types";
import { CitaService } from "@/services/cita.service";
import { useAuth } from "@/components/providers/AuthProvider";
import { useSignalR } from "@/hooks/useSignalR";
import { toast } from "sonner";
import dayjs from "dayjs";
import { usePageTransition, AnimatedCard } from "@/components/animations/Animatedcomponents";

export default function MisCitasPage() {
  const { user } = useAuth();
  const [citas, setCitas] = useState<CitaDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useSignalR({ onNuevaCita: () => toast.info("Nueva cita registrada") });

  useEffect(() => {
    const fetchCitas = async () => {
      if (!user?.id) { setIsLoading(false); return; }
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
      setCitas(prev => prev.map(c => c.id === id ? { ...c, estado: EstadoCita.Cancelada } : c));
      toast.success("Cita cancelada correctamente.");
    } catch {
      toast.error("No se pudo cancelar la cita.");
    }
  };

  const reprogramar = async (id: number) => {
    const nuevaFecha = window.prompt("Nueva fecha y hora (YYYY-MM-DD HH:mm):");
    if (!nuevaFecha) return;
    const iso = new Date(nuevaFecha.replace(" ", "T")).toISOString();
    try {
      await CitaService.reprogramarCita(id, iso);
      setCitas(prev => prev.map(c => c.id === id ? { ...c, fechaHora: iso, estado: EstadoCita.Solicitada } : c));
      toast.success("Cita reprogramada correctamente.");
    } catch {
      toast.error("No se pudo reprogramar la cita.");
    }
  };

  usePageTransition();

  function getStatusStyles(estado: string) {
    switch (estado?.toLowerCase()) {
      case "confirmada": return "bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 border border-emerald-500/20";
      case "solicitada": return "bg-amber-500/10 text-amber-600 dark:text-amber-400 border border-amber-500/20";
      case "cancelada": return "bg-rose-500/10 text-rose-600 dark:text-rose-400 border border-rose-500/20";
      case "noasistio": return "bg-muted text-muted-foreground border border-border";
      default: return "bg-muted text-muted-foreground border border-border";
    }
  }

  return (
    <div className="space-y-6 page-content animate-in fade-in duration-500">
      <header className="relative overflow-hidden rounded-3xl border border-emerald-500/20 bg-gradient-to-br from-emerald-500/15 via-white dark:via-slate-950 to-teal-500/15 p-6 md:p-7 shadow-sm">
        <div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-emerald-500/10 dark:bg-emerald-500/20 blur-3xl opacity-50" />
        <div className="relative z-10">
          <h1 className="text-3xl font-black tracking-tight text-foreground flex items-center gap-2">
            <Calendar className="h-7 w-7 text-emerald-600 dark:text-emerald-400" />
            Mis Citas Médicas
          </h1>
          <p className="text-muted-foreground font-medium mt-1">Historial y próximas consultas programadas.</p>
        </div>
      </header>

      {isLoading ? (
        <div className="py-20 flex flex-col items-center justify-center">
          <Loader2 className="w-10 h-10 animate-spin text-emerald-500" />
          <p className="mt-4 text-sm font-bold text-muted-foreground uppercase tracking-widest">Cargando historial...</p>
        </div>
      ) : citas.length === 0 ? (
        <AnimatedCard className="bg-card/50 border border-border rounded-3xl p-16 text-center shadow-sm">
          <Calendar className="w-16 h-16 text-muted-foreground/20 mx-auto mb-4" />
          <h2 className="text-xl font-bold text-foreground">No tienes citas agendadas</h2>
          <p className="text-muted-foreground mt-2 font-medium">Comienza por buscar un médico especialista.</p>
          <a href="/paciente/medicos" className="mt-6 inline-flex bg-emerald-600 hover:bg-emerald-500 text-white px-6 py-2.5 rounded-full font-bold transition-all shadow-lg shadow-emerald-500/20 active:scale-95">
            Explorar Médicos
          </a>
        </AnimatedCard>
      ) : (
        <div className="grid gap-5">
          {citas.map((cita, idx) => (
            <AnimatedCard
              key={cita.id}
              delay={idx * 50}
              className="bg-card border border-border rounded-2xl p-5 hover:shadow-xl transition-all group hover:-translate-y-1"
            >
              <div className="flex flex-col md:flex-row md:items-center justify-between gap-6">
                <div className="flex items-center gap-6">
                  <div className="w-20 h-20 rounded-2xl border-2 border-emerald-500/20 bg-emerald-500/5 flex flex-col items-center justify-center shadow-inner group-hover:scale-110 transition-transform">
                    <span className="text-emerald-600 dark:text-emerald-400 text-xs font-black uppercase tracking-tighter">
                      {dayjs(cita.fechaHora).format("MMM")}
                    </span>
                    <span className="text-emerald-600 dark:text-emerald-400 text-3xl font-black">
                      {dayjs(cita.fechaHora).format("DD")}
                    </span>
                  </div>
                  <div>
                    <h3 className="text-foreground font-black text-xl leading-tight group-hover:text-emerald-500 transition-colors">
                      Consulta Médica
                    </h3>
                    <div className="flex flex-wrap items-center gap-4 mt-2.5">
                      <div className="flex items-center gap-2 text-muted-foreground text-sm font-bold">
                        <Clock className="w-4 h-4 text-emerald-500" />
                        <span>{dayjs(cita.fechaHora).format("hh:mm A")}</span>
                      </div>
                      <div className="flex items-center gap-2 text-muted-foreground text-sm font-bold">
                        <MapPin className="w-4 h-4 text-emerald-500" />
                        <span>Consultorio Principal</span>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="flex flex-wrap items-center gap-4">
                  <span className={`px-5 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest ${getStatusStyles(cita.estado)} shadow-sm`}>
                    {cita.estado}
                  </span>
                  {(cita.estado === EstadoCita.Confirmada || cita.estado === EstadoCita.Solicitada) && (
                    <div className="flex gap-2">
                      <button
                        onClick={() => reprogramar(cita.id)}
                        className="px-5 py-2.5 border border-border text-foreground text-xs font-bold rounded-xl hover:bg-muted transition-all active:scale-95 shadow-sm"
                      >
                        Reprogramar
                      </button>
                      <button
                        onClick={() => cancelar(cita.id)}
                        className="px-5 py-2.5 bg-rose-500/10 border border-rose-500/20 text-rose-600 dark:text-rose-400 text-xs font-bold rounded-xl hover:bg-rose-500/20 transition-all active:scale-95 shadow-sm"
                      >
                        Cancelar
                      </button>
                    </div>
                  )}
                </div>
              </div>
            </AnimatedCard>
          ))}
        </div>
      )}
    </div>
  );
}