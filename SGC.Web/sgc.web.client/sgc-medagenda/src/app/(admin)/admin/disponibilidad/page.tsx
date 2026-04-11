"use client";

import { useEffect, useMemo, useState } from "react";
import { CalendarClock, Loader2, Plus } from "lucide-react";
import { MedicoDTO } from "@/types/medico.types";
import { DisponibilidadDTO, CreateDisponibilidadRequest } from "@/types/api.types";
import { MedicoService } from "@/services/medico.service";
import { DisponibilidadService } from "@/services/disponibilidad.service";

const DIAS_SEMANA = [
  { label: "Lunes", value: 0 },
  { label: "Martes", value: 1 },
  { label: "Miercoles", value: 2 },
  { label: "Jueves", value: 3 },
  { label: "Viernes", value: 4 },
  { label: "Sabado", value: 5 },
  { label: "Domingo", value: 6 },
];

const normalizeTime = (value: string) => {
  if (!value) return value;
  if (value.length === 5) return `${value}:00`;
  return value;
};

export default function AdminDisponibilidadPage() {
  const [medicos, setMedicos] = useState<MedicoDTO[]>([]);
  const [medicoId, setMedicoId] = useState<number | "">("");
  const [disponibilidades, setDisponibilidades] = useState<DisponibilidadDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [mensaje, setMensaje] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const [form, setForm] = useState<Omit<CreateDisponibilidadRequest, "medicoId">>({
    diaSemana: 0,
    horaInicio: "08:00",
    horaFin: "12:00",
    duracionCitaMin: 30,
    esRecurrente: true,
  });

  const medicoSeleccionado = useMemo(
    () => medicos.find((m) => m.id === medicoId),
    [medicos, medicoId]
  );

  useEffect(() => {
    const cargar = async () => {
      setIsLoading(true);
      try {
        const data = await MedicoService.obtenerTodos();
        setMedicos(data);
      } catch (err) {
        console.error("Error cargando medicos:", err);
        setError("No se pudieron cargar los medicos.");
      } finally {
        setIsLoading(false);
      }
    };
    cargar();
  }, []);

  useEffect(() => {
    const cargarDisponibilidad = async () => {
      if (!medicoId) {
        setDisponibilidades([]);
        return;
      }
      try {
        const data = await DisponibilidadService.obtenerPorMedico(Number(medicoId));
        setDisponibilidades(data);
      } catch (err) {
        console.error("Error cargando disponibilidad:", err);
        setError("No se pudo cargar la disponibilidad del medico.");
      }
    };
    cargarDisponibilidad();
  }, [medicoId]);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setMensaje(null);
    setError(null);

    if (!medicoId) {
      setError("Selecciona un medico.");
      return;
    }

    const payload: CreateDisponibilidadRequest = {
      medicoId: Number(medicoId),
      diaSemana: Number(form.diaSemana),
      horaInicio: normalizeTime(form.horaInicio),
      horaFin: normalizeTime(form.horaFin),
      duracionCitaMin: Number(form.duracionCitaMin),
      esRecurrente: Boolean(form.esRecurrente),
    };

    setIsSaving(true);
    try {
      await DisponibilidadService.crear(payload);
      setMensaje("Disponibilidad creada correctamente.");
      const data = await DisponibilidadService.obtenerPorMedico(Number(medicoId));
      setDisponibilidades(data);
    } catch (err: any) {
      console.error("Error creando disponibilidad:", err);
      setError(err?.message || "No se pudo crear la disponibilidad.");
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="p-6 max-w-6xl mx-auto space-y-6 animate-in fade-in duration-500">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-slate-900 dark:text-white flex items-center gap-2">
            <CalendarClock className="w-7 h-7 text-indigo-500" />
            Disponibilidad de Medicos
          </h1>
          <p className="text-slate-500 dark:text-slate-400 text-sm mt-1">
            Asigna horarios a los doctores para habilitar el agendamiento de citas.
          </p>
        </div>
      </header>

      <div className="grid gap-6 lg:grid-cols-[360px_1fr]">
        <form
          onSubmit={onSubmit}
          className="bg-slate-950/70 border border-slate-800 rounded-2xl p-5 space-y-4 text-slate-200"
        >
          <div>
            <label className="text-xs text-slate-400">Medico</label>
            <select
              className="mt-2 w-full px-3 py-2 rounded-xl bg-slate-950 border border-slate-800 text-slate-100"
              value={medicoId}
              onChange={(e) => setMedicoId(e.target.value ? Number(e.target.value) : "")}
            >
              <option value="">Selecciona un medico</option>
              {medicos.map((m) => (
                <option key={m.id} value={m.id}>
                  {m.nombre} {m.especialidadNombre ? `- ${m.especialidadNombre}` : ""}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="text-xs text-slate-400">Dia de la semana</label>
            <select
              className="mt-2 w-full px-3 py-2 rounded-xl bg-slate-950 border border-slate-800 text-slate-100"
              value={form.diaSemana}
              onChange={(e) => setForm({ ...form, diaSemana: Number(e.target.value) })}
            >
              {DIAS_SEMANA.map((d) => (
                <option key={d.value} value={d.value}>
                  {d.label}
                </option>
              ))}
            </select>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="text-xs text-slate-400">Hora inicio</label>
              <input
                type="time"
                className="mt-2 w-full px-3 py-2 rounded-xl bg-slate-950 border border-slate-800 text-slate-100"
                value={form.horaInicio}
                onChange={(e) => setForm({ ...form, horaInicio: e.target.value })}
              />
            </div>
            <div>
              <label className="text-xs text-slate-400">Hora fin</label>
              <input
                type="time"
                className="mt-2 w-full px-3 py-2 rounded-xl bg-slate-950 border border-slate-800 text-slate-100"
                value={form.horaFin}
                onChange={(e) => setForm({ ...form, horaFin: e.target.value })}
              />
            </div>
          </div>

          <div>
            <label className="text-xs text-slate-400">Duracion (minutos)</label>
            <input
              type="number"
              min={10}
              step={5}
              className="mt-2 w-full px-3 py-2 rounded-xl bg-slate-950 border border-slate-800 text-slate-100"
              value={form.duracionCitaMin}
              onChange={(e) => setForm({ ...form, duracionCitaMin: Number(e.target.value) })}
            />
          </div>

          <label className="flex items-center gap-2 text-sm">
            <input
              type="checkbox"
              checked={form.esRecurrente}
              onChange={(e) => setForm({ ...form, esRecurrente: e.target.checked })}
            />
            Repetir semanalmente
          </label>

          {mensaje && <p className="text-emerald-300 text-sm">{mensaje}</p>}
          {error && <p className="text-rose-300 text-sm">{error}</p>}

          <button
            type="submit"
            disabled={isSaving}
            className="w-full flex items-center justify-center gap-2 rounded-xl bg-indigo-600 hover:bg-indigo-500 text-white px-4 py-2 disabled:opacity-60"
          >
            {isSaving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Plus className="w-4 h-4" />}
            Crear disponibilidad
          </button>
        </form>

        <div className="bg-slate-900/60 rounded-2xl border border-slate-800/80 p-5 min-h-[320px]">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold text-white">Disponibilidad actual</h2>
            {medicoSeleccionado && (
              <span className="text-xs text-slate-400">
                {medicoSeleccionado.nombre}
              </span>
            )}
          </div>

          {isLoading ? (
            <div className="flex items-center gap-2 text-slate-400">
              <Loader2 className="w-4 h-4 animate-spin" />
              Cargando medicos...
            </div>
          ) : !medicoId ? (
            <p className="text-slate-400">Selecciona un medico para ver su disponibilidad.</p>
          ) : disponibilidades.length === 0 ? (
            <p className="text-slate-400">No hay horarios registrados para este medico.</p>
          ) : (
            <div className="space-y-3">
              {disponibilidades.map((item) => (
                <div
                  key={item.id}
                  className="flex items-center justify-between bg-slate-950/70 border border-slate-800 rounded-xl px-4 py-3"
                >
                  <div>
                    <p className="text-white text-sm font-medium">{item.diaSemana}</p>
                    <p className="text-xs text-slate-400">
                      {item.horaInicio} - {item.horaFin} · {item.duracionCitaMin} min
                    </p>
                  </div>
                  <span className="text-xs text-emerald-300">
                    {item.esRecurrente ? "Recurrente" : "Unica"}
                  </span>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
