"use client";

import { useState } from "react";
import { CreditCard, Loader2 } from "lucide-react";
import { PagoResponse } from "@/types/api.types";
import { PagoService } from "@/services/pago.service";

export default function PagosAdminPage() {
  const [pacienteId, setPacienteId] = useState("");
  const [pagos, setPagos] = useState<PagoResponse[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const cargarPagos = async () => {
    if (!pacienteId) {
      setError("Ingrese un pacienteId válido.");
      return;
    }
    setError("");
    setLoading(true);
    try {
      const data = await PagoService.obtenerPorPaciente(Number(pacienteId));
      setPagos(data);
    } catch {
      setPagos([]);
      setError("No se pudo obtener pagos para ese paciente.");
    } finally {
      setLoading(false);
    }
  };

  const reembolsar = async (id: number) => {
    await PagoService.reembolsar(id);
    await cargarPagos();
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold flex items-center gap-2 text-white">
            <CreditCard className="text-indigo-400" /> Pagos
          </h1>
          <p className="text-slate-400 text-sm mt-1">
            Consulta pagos por paciente y procesa reembolsos.
          </p>
        </div>
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
        <button
          onClick={cargarPagos}
          className="px-4 py-2 rounded-xl bg-indigo-600 hover:bg-indigo-500 text-white"
        >
          Buscar
        </button>
        {error && <span className="text-rose-400 text-sm">{error}</span>}
      </div>

      <div className="border border-slate-800/80 rounded-xl overflow-hidden bg-slate-900/60">
        <table className="w-full text-left text-sm">
          <thead className="bg-slate-950/70 border-b border-slate-800/80 text-slate-300">
            <tr>
              <th className="p-4">ID</th>
              <th className="p-4">Cita</th>
              <th className="p-4">Monto</th>
              <th className="p-4">Estado</th>
              <th className="p-4">Fecha</th>
              <th className="p-4 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={6} className="p-8 text-center">
                  <Loader2 className="animate-spin mx-auto text-indigo-400" />
                </td>
              </tr>
            ) : pagos.length === 0 ? (
              <tr>
                <td colSpan={6} className="p-6 text-center text-slate-400">
                  No hay pagos para este paciente.
                </td>
              </tr>
            ) : (
              pagos.map((p) => (
                <tr key={p.id} className="border-b border-slate-800/80">
                  <td className="p-4 text-slate-200">{p.id}</td>
                  <td className="p-4 text-slate-200">{p.citaId}</td>
                  <td className="p-4 text-slate-200">
                    {p.moneda} {p.monto}
                  </td>
                  <td className="p-4 text-slate-200">{p.estado}</td>
                  <td className="p-4 text-slate-400">
                    {new Date(p.fechaCreacion).toLocaleDateString()}
                  </td>
                  <td className="p-4 text-right">
                    <button
                      onClick={() => reembolsar(p.id)}
                      className="px-3 py-1.5 rounded-lg text-xs border border-rose-500/40 text-rose-300 hover:bg-rose-500/10"
                    >
                      Reembolsar
                    </button>
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
