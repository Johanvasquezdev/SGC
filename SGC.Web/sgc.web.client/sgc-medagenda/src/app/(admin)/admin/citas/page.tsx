"use client";

import { useState } from "react";
import { CalendarCheck, Loader2, Search, Clock3, Activity } from "lucide-react";
import { CitaDTO, EstadoCita } from "@/types/api.types";
import { CitaService } from "@/services/cita.service";
import dayjs from "dayjs";

export default function AdminCitasPage() {
  const [pacienteId, setPacienteId] = useState("");
  const [medicoId, setMedicoId] = useState("");
  const [fecha, setFecha] = useState("");
  const [citas, setCitas] = useState<CitaDTO[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const cargar = async () => {
    if (!pacienteId) {
      setError("Ingrese un pacienteId válido.");
      return;
    }
    setError("");
    setLoading(true);
    try {
      const data = await CitaService.obtenerCitasPorPaciente(Number(pacienteId));
      setCitas(data);
    } catch {
      setCitas([]);
      setError("No se pudieron cargar las citas.");
    } finally {
      setLoading(false);
    }
  };

  const confirmar = async (id: number) => {
    await CitaService.confirmarCita(id);
    setCitas((prev) => prev.map((c) => (c.id === id ? { ...c, estado: EstadoCita.Confirmada } : c)));
  };

  const iniciar = async (id: number) => {
    await CitaService.iniciarConsulta(id);
    setCitas((prev) => prev.map((c) => (c.id === id ? { ...c, estado: EstadoCita.EnProgreso } : c)));
  };

  const asistencia = async (id: number, asistio: boolean) => {
    await CitaService.marcarAsistencia(id, asistio);
    setCitas((prev) =>
      prev.map((c) =>
        c.id === id ? { ...c, estado: asistio ? EstadoCita.Completada : EstadoCita.NoAsistio } : c
      )
    );
  };

  const cancelar = async (id: number) => {
    const motivo = window.prompt("Motivo de cancelación:");
    if (!motivo) return;
    await CitaService.cancelarCita(id, motivo);
    setCitas((prev) => prev.map((c) => (c.id === id ? { ...c, estado: EstadoCita.Cancelada } : c)));
  };

  const reprogramar = async (id: number) => {
    const nuevaFecha = window.prompt("Nueva fecha y hora (YYYY-MM-DD HH:mm):");
    if (!nuevaFecha) return;
    const iso = new Date(nuevaFecha.replace(" ", "T")).toISOString();
    await CitaService.reprogramarCita(id, iso);
    setCitas((prev) => prev.map((c) => (c.id === id ? { ...c, fechaHora: iso, estado: EstadoCita.Solicitada } : c)));
  };

  const citasFiltradas = citas.filter((c) => {
    if (medicoId && c.medicoId !== Number(medicoId)) return false;
    if (fecha) {
      const fechaCita = dayjs(c.fechaHora).format("YYYY-MM-DD");
      if (fechaCita !== fecha) return false;
    }
    return true;
  });

  const totalFiltradas = citasFiltradas.length;
  const pendientes = citasFiltradas.filter((c) => c.estado === EstadoCita.Solicitada).length;
  const confirmadas = citasFiltradas.filter((c) => c.estado === EstadoCita.Confirmada).length;
  const enProgreso = citasFiltradas.filter((c) => c.estado === EstadoCita.EnProgreso).length;

  return (
    <div className="mx-auto max-w-7xl space-y-6 p-6">
      <header className="relative overflow-hidden rounded-3xl border border-indigo-500/20 bg-gradient-to-br from-indigo-500/15 via-slate-900 to-emerald-500/15 p-6 md:p-7">
        <div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-indigo-500/20 blur-3xl" />
        <div className="absolute -bottom-20 -left-20 h-56 w-56 rounded-full bg-emerald-500/20 blur-3xl" />

        <div className="relative z-10 flex flex-col gap-6 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.2em] text-indigo-300/90">
              Administracion
            </p>
            <h1 className="mt-2 flex items-center gap-2 text-3xl font-bold tracking-tight text-white">
              <CalendarCheck className="h-7 w-7 text-indigo-300" />
              Gestion de Citas
            </h1>
            <p className="mt-2 max-w-2xl text-slate-300">
              Consulta, filtra y opera citas de pacientes en una sola vista operativa.
            </p>
          </div>

          <div className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 backdrop-blur-sm">
            <p className="text-xs text-slate-300">Resultados actuales</p>
            <p className="text-2xl font-bold text-white">{loading ? "--" : totalFiltradas}</p>
          </div>
        </div>
      </header>

      <section className="grid gap-4 md:grid-cols-4">
        <article className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-5">
          <p className="text-sm text-slate-400">Total filtradas</p>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : totalFiltradas}</p>
        </article>
        <article className="rounded-2xl border border-amber-500/30 bg-amber-500/10 p-5">
          <div className="flex items-center gap-2 text-amber-300">
            <Clock3 className="h-4 w-4" />
            <p className="text-sm">Solicitadas</p>
          </div>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : pendientes}</p>
        </article>
        <article className="rounded-2xl border border-emerald-500/30 bg-emerald-500/10 p-5">
          <div className="flex items-center gap-2 text-emerald-300">
            <CalendarCheck className="h-4 w-4" />
            <p className="text-sm">Confirmadas</p>
          </div>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : confirmadas}</p>
        </article>
        <article className="rounded-2xl border border-indigo-500/30 bg-indigo-500/10 p-5">
          <div className="flex items-center gap-2 text-indigo-300">
            <Activity className="h-4 w-4" />
            <p className="text-sm">En progreso</p>
          </div>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : enProgreso}</p>
        </article>
      </section>

      <section className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-4">
        <div className="mb-4 flex items-center gap-2 text-sm text-slate-300">
          <Search className="h-4 w-4" />
          Filtros de consulta
        </div>

        <div className="flex flex-col items-start gap-3 md:flex-row md:items-end">
        <div className="flex flex-col gap-2">
          <label className="text-sm text-slate-300">Paciente ID</label>
          <input
            value={pacienteId}
            onChange={(e) => setPacienteId(e.target.value)}
            className="w-56 px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
            placeholder="Ej: 17"
          />
        </div>
        <div className="flex flex-col gap-2">
          <label className="text-sm text-slate-300">Médico ID</label>
          <input
            value={medicoId}
            onChange={(e) => setMedicoId(e.target.value)}
            className="w-56 px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
            placeholder="Ej: 16"
          />
        </div>
        <div className="flex flex-col gap-2">
          <label className="text-sm text-slate-300">Fecha</label>
          <input
            type="date"
            value={fecha}
            onChange={(e) => setFecha(e.target.value)}
            className="w-56 px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100"
          />
        </div>
        <button onClick={cargar} className="px-4 py-2 rounded-xl bg-indigo-600 hover:bg-indigo-500 text-white">
          Buscar
        </button>
        {error && <span className="text-rose-400 text-sm">{error}</span>}
      </div>
      </section>

      <div className="overflow-hidden rounded-2xl border border-slate-800/80 bg-slate-900/70">
        <table className="w-full text-left text-sm">
          <thead className="bg-slate-950/70 border-b border-slate-800/80 text-slate-300">
            <tr>
              <th className="p-4">ID</th>
              <th className="p-4">Fecha</th>
              <th className="p-4">Estado</th>
              <th className="p-4 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={4} className="p-8 text-center">
                  <Loader2 className="animate-spin mx-auto text-indigo-400" />
                </td>
              </tr>
            ) : citasFiltradas.length === 0 ? (
              <tr>
                <td colSpan={4} className="p-6 text-center text-slate-400">
                  No hay citas para este paciente.
                </td>
              </tr>
            ) : (
              citasFiltradas.map((c) => (
                <tr key={c.id} className="border-b border-slate-800/80">
                  <td className="p-4 text-slate-200">{c.id}</td>
                  <td className="p-4 text-slate-200">{dayjs(c.fechaHora).format("DD/MM/YYYY HH:mm")}</td>
                  <td className="p-4 text-slate-200">{c.estado}</td>
                  <td className="p-4 text-right">
                    <div className="flex flex-wrap justify-end gap-2">
                      <button onClick={() => confirmar(c.id)} className="px-3 py-1.5 rounded-lg text-xs border border-emerald-500/40 text-emerald-300 hover:bg-emerald-500/10">
                        Confirmar
                      </button>
                      <button onClick={() => iniciar(c.id)} className="px-3 py-1.5 rounded-lg text-xs border border-indigo-500/40 text-indigo-300 hover:bg-indigo-500/10">
                        Iniciar
                      </button>
                      <button onClick={() => asistencia(c.id, true)} className="px-3 py-1.5 rounded-lg text-xs border border-emerald-500/40 text-emerald-300 hover:bg-emerald-500/10">
                        Asistió
                      </button>
                      <button onClick={() => asistencia(c.id, false)} className="px-3 py-1.5 rounded-lg text-xs border border-amber-500/40 text-amber-300 hover:bg-amber-500/10">
                        No asistió
                      </button>
                      <button onClick={() => reprogramar(c.id)} className="px-3 py-1.5 rounded-lg text-xs border border-slate-500/40 text-slate-300 hover:bg-slate-500/10">
                        Reprogramar
                      </button>
                      <button onClick={() => cancelar(c.id)} className="px-3 py-1.5 rounded-lg text-xs border border-rose-500/40 text-rose-300 hover:bg-rose-500/10">
                        Cancelar
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
