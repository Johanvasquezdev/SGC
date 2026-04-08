"use client";

import { useEffect, useMemo, useState } from "react";
import dayjs from "dayjs";
import { CalendarCheck, Clock, Stethoscope } from "lucide-react";
import { CitaDTO, EstadoCita, MedicoDTO } from "@/types/api.types";
import { CitaService } from "@/services/cita.service";
import { MedicoService } from "@/services/medico.service";
import { useAuth } from "@/components/providers/AuthProvider";
import { CitaCard } from "@/components/citas/CitaCard";
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
        <h1 className="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
          Bienvenido, {user?.nombre || "Paciente"}
        </h1>
        <p className="text-muted-foreground">Gestiona tus citas y encuentra médicos especialistas</p>
      </header>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="bg-card p-5 rounded-2xl border border-border flex items-center gap-4">
          <div className="p-3 rounded-xl bg-emerald-500/10">
            <CalendarCheck className="w-6 h-6 text-emerald-400" />
          </div>
          <div>
            <p className="text-sm text-muted-foreground">Citas Hoy</p>
            <p className="text-2xl font-bold">{loading ? "-" : citasHoy.length}</p>
          </div>
        </div>
        <div className="bg-card p-5 rounded-2xl border border-border flex items-center gap-4">
          <div className="p-3 rounded-xl bg-emerald-500/10">
            <Clock className="w-6 h-6 text-emerald-400" />
          </div>
          <div>
            <p className="text-sm text-muted-foreground">Citas Pendientes</p>
            <p className="text-2xl font-bold">{loading ? "-" : citasPendientes.length}</p>
          </div>
        </div>
        <div className="bg-card p-5 rounded-2xl border border-border flex items-center gap-4">
          <div className="p-3 rounded-xl bg-emerald-500/10">
            <Stethoscope className="w-6 h-6 text-emerald-400" />
          </div>
          <div>
            <p className="text-sm text-muted-foreground">Médicos Disponibles</p>
            <p className="text-2xl font-bold">{loading ? "-" : medicos.length}</p>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="space-y-4">
          <div className="flex items-center justify-between">
            <h2 className="text-xl font-semibold">Próximas Citas</h2>
            <div className="flex items-center gap-4">
              <a href="/paciente/medicos" className="text-emerald-600 text-sm font-medium hover:underline">Agendar cita</a>
              <a href="/paciente/citas" className="text-emerald-600 text-sm font-medium hover:underline">Ver todas</a>
            </div>
          </div>

          {proximasCitas.length === 0 ? (
            <div className="bg-card p-6 rounded-2xl border border-border text-muted-foreground">
              No tienes citas próximas. Agenda una con un médico disponible.
            </div>
          ) : (
            <div className="space-y-3">
              {proximasCitas.map((cita) => {
                const medico = medicosById.get(cita.medicoId);
                return (
                  <div key={cita.id} className="space-y-2">
                    <CitaCard
                      doctorNombre={medico?.nombre || "Medico"}
                      especialidad={medico?.especialidadNombre || "Especialidad"}
                      fecha={dayjs(cita.fechaHora).format("DD MMM YYYY")}
                      hora={dayjs(cita.fechaHora).format("hh:mm A")}
                      estado={cita.estado}
                    />
                    {(cita.estado === EstadoCita.Confirmada || cita.estado === EstadoCita.Solicitada) && (
                      <button
                        onClick={() => cancelarCita(cita.id)}
                        className="text-sm text-rose-600 hover:text-rose-700"
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

        <div className="space-y-4">
          <div className="flex items-center justify-between">
            <h2 className="text-xl font-semibold">Médicos Destacados</h2>
            <a href="/paciente/medicos" className="text-emerald-600 text-sm font-medium hover:underline">Ver todos</a>
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
