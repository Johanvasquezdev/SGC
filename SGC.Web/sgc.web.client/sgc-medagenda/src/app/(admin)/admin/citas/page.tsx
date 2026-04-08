"use client";

import { useState } from "react";
import { CalendarCheck, Loader2 } from "lucide-react";
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

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <header className="flex items-center gap-2 text-white">
        <CalendarCheck className="text-indigo-400" />
        <h1 className="text-2xl font-bold">Gestión de Citas</h1>
      </header>

      <div className="flex flex-col md:flex-row gap-3 items-start md:items-end">
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

      <div className="border border-slate-800/80 rounded-xl overflow-hidden bg-slate-900/60">
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
