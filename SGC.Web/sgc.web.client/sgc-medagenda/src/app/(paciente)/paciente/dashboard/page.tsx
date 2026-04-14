"use client";

import { useEffect, useMemo, useState } from "react";
import dayjs from "dayjs";
import {
  Calendar,
  CalendarDays,
  CalendarCheck,
  Clock,
  MoreVertical,
  Stethoscope,
} from "lucide-react";
import { CitaDTO, EstadoCita, MedicoDTO } from "@/types/api.types";
import { CitaService } from "@/services/cita.service";
import { MedicoService } from "@/services/medico.service";
import { useAuth } from "@/components/providers/AuthProvider";
import { MedicoCard } from "@/components/medicos/MedicoCard";
import { AgendarCitaModal } from "@/components/citas/AgendarCita";
import { toast } from "sonner";

export default function PacienteDashboard() {
  const { user } = useAuth();
  const [citas, setCitas] = useState<CitaDTO[]>([]);
  const [medicos, setMedicos] = useState<MedicoDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedMedico, setSelectedMedico] = useState<MedicoDTO | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!user?.id) return;
      try {
        const [citasData, medicosData] = await Promise.all([
          CitaService.obtenerCitasPorPaciente(user.id),
          MedicoService.obtenerTodos(),
        ]);
        setCitas(citasData);
        setMedicos(medicosData.filter(m => m.activo));
      } catch {
        setCitas([]);
        setMedicos([]);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [user?.id]);

  const medicosById = useMemo(() => {
    const map = new Map<number, MedicoDTO>();
    medicos.forEach(m => map.set(m.id, m));
    return map;
  }, [medicos]);

  const citasHoy = citas.filter(c => dayjs(c.fechaHora).isSame(dayjs(), "day"));
  const citasPendientes = citas.filter(c => c.estado === EstadoCita.Solicitada);
  const proximasCitas = citas
    .filter(c => dayjs(c.fechaHora).isAfter(dayjs()))
    .sort((a, b) => dayjs(a.fechaHora).valueOf() - dayjs(b.fechaHora).valueOf())
    .slice(0, 3);

  const medicosDestacados = medicos.slice(0, 3);

  const getStatusStyles = (estado: EstadoCita | string) => {
    switch ((estado || "").toString().toLowerCase()) {
      case "confirmada":
        return "bg-emerald-500/15 text-emerald-300";
      case "solicitada":
        return "bg-amber-500/15 text-amber-300";
      case "cancelada":
        return "bg-rose-500/15 text-rose-300";
      case "noasistio":
        return "bg-slate-500/20 text-slate-300";
      default:
        return "bg-slate-500/20 text-slate-300";
    }
  };

  const cancelarCita = async (citaId: number) => {
    try {
      await CitaService.cancelarCita(citaId, "Cancelada por paciente");
      setCitas(citas.map(c => c.id === citaId ? { ...c, estado: EstadoCita.Cancelada } : c));
      toast.success("Cita cancelada");
    } catch {
      toast.error("No se pudo cancelar la cita");
    }
  };

  return (
    <div className="space-y-8">
      <header className="space-y-1">
        <h1 className="text-3xl font-bold tracking-tight text-foreground">
          Bienvenido, {user?.nombre || "Paciente"}
        </h1>
        <p className="text-muted-foreground">
          Gestiona tus citas y encuentra médicos especialistas
        </p>
      </header>

      <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
        <div className="rounded-2xl border border-border/60 bg-card/70 p-5 backdrop-blur-sm">
          <div className="flex items-center gap-4">
            <div className="rounded-xl bg-emerald-500/15 p-3">
              <CalendarCheck className="h-6 w-6 text-emerald-500" />
            </div>
            <div>
              <p className="text-sm text-muted-foreground">Citas Hoy</p>
              <p className="text-2xl font-bold text-foreground">{loading ? "-" : citasHoy.length}</p>
            </div>
          </div>
        </div>

        <div className="rounded-2xl border border-border/60 bg-card/70 p-5 backdrop-blur-sm">
          <div className="flex items-center gap-4">
            <div className="rounded-xl bg-emerald-500/15 p-3">
              <Clock className="h-6 w-6 text-emerald-500" />
            </div>
            <div>
              <p className="text-sm text-muted-foreground">Citas Pendientes</p>
              <p className="text-2xl font-bold text-foreground">{loading ? "-" : citasPendientes.length}</p>
            </div>
          </div>
        </div>

        <div className="rounded-2xl border border-border/60 bg-card/70 p-5 backdrop-blur-sm">
          <div className="flex items-center gap-4">
            <div className="rounded-xl bg-emerald-500/15 p-3">
              <Stethoscope className="h-6 w-6 text-emerald-500" />
            </div>
            <div>
              <p className="text-sm text-muted-foreground">Médicos Disponibles</p>
              <p className="text-2xl font-bold text-foreground">{loading ? "-" : medicos.length}</p>
            </div>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-5">
        <div className="space-y-4 lg:col-span-3">
          <div className="flex items-center justify-between">
            <h2 className="text-xl font-semibold text-foreground">Próximas Citas</h2>
            <div className="flex items-center gap-4">
              <a
                href="/paciente/medicos"
                className="text-sm font-medium text-emerald-500 hover:text-emerald-400"
              >
                Agendar cita
              </a>
              <a
                href="/paciente/citas"
                className="text-sm font-medium text-emerald-500 hover:text-emerald-400"
              >
                Ver todas
              </a>
            </div>
          </div>

          {proximasCitas.length === 0 ? (
            <div className="rounded-2xl border border-border/60 bg-card/70 p-6 text-muted-foreground backdrop-blur-sm">
              No tienes citas próximas. Agenda una con un médico disponible.
            </div>
          ) : (
            <div className="space-y-3">
              {proximasCitas.map((cita) => {
                const medico = medicosById.get(cita.medicoId);
                return (
                  <div key={cita.id} className="rounded-2xl border border-border/60 bg-card/70 p-4 backdrop-blur-sm">
                    <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
                      <div className="flex items-center gap-4">
                        <div className="rounded-xl bg-emerald-500/15 p-3">
                          <CalendarDays className="h-5 w-5 text-emerald-500" />
                        </div>
                        <div>
                          <p className="font-medium text-foreground">{medico?.nombre || "Médico"}</p>
                          <p className="text-sm text-emerald-500">
                            {medico?.especialidadNombre || "Especialidad"}
                          </p>
                        </div>
                      </div>

                      <div className="flex flex-wrap items-center gap-4 text-sm text-muted-foreground md:gap-6">
                        <div className="flex items-center gap-1.5">
                          <Calendar className="h-4 w-4" />
                          <span>{dayjs(cita.fechaHora).format("DD MMM YYYY")}</span>
                        </div>
                        <div className="flex items-center gap-1.5">
                          <Clock className="h-4 w-4" />
                          <span>{dayjs(cita.fechaHora).format("hh:mm A")}</span>
                        </div>
                        <span
                          className={`rounded-full px-3 py-1 text-xs font-semibold ${getStatusStyles(cita.estado)}`}
                        >
                          {cita.estado}
                        </span>
                        <button
                          type="button"
                          className="rounded-lg p-1.5 text-muted-foreground transition-colors hover:bg-secondary hover:text-foreground"
                          aria-label="Acciones de cita"
                        >
                          <MoreVertical className="h-4 w-4" />
                        </button>
                      </div>
                    </div>

                    {(cita.estado === EstadoCita.Confirmada || cita.estado === EstadoCita.Solicitada) && (
                      <button
                        type="button"
                        onClick={() => cancelarCita(cita.id)}
                        className="mt-3 border-t border-border/60 pt-3 text-sm text-rose-500 hover:text-rose-400"
                      >
                        Cancelar cita
                      </button>
                    )}
                  </div>
                );
              })}
            </div>
          )}
        </div>

        <div className="space-y-4 lg:col-span-2">
          <div className="flex items-center justify-between">
            <h2 className="text-xl font-semibold text-foreground">Médicos Destacados</h2>
            <a
              href="/paciente/medicos"
              className="text-sm font-medium text-emerald-500 hover:text-emerald-400"
            >
              Ver todos
            </a>
          </div>

          <div className="grid gap-4">
            {medicosDestacados.map((medico) => (
              <MedicoCard key={medico.id} medico={medico} onAgendarClick={setSelectedMedico} />
            ))}
          </div>
        </div>
      </div>

      <AgendarCitaModal medico={selectedMedico} isOpen={!!selectedMedico} onClose={() => setSelectedMedico(null)} />
    </div>
  );
}
