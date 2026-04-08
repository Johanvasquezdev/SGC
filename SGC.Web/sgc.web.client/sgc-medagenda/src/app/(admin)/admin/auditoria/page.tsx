"use client";

import { useState, useEffect } from "react";
import { Shield, Loader2 } from "lucide-react";
import { AuditoriaResponse } from "@/types/api.types";
import { AuditoriaService } from "@/services/auditoria.service";

export default function AuditoriaPage() {
  const [registros, setRegistros] = useState<AuditoriaResponse[]>([]);
  const [loading, setLoading] = useState(false);
  const [entidad, setEntidad] = useState("CITA");

  useEffect(() => {
    const fetch = async () => {
      setLoading(true);
      try {
        setRegistros(await AuditoriaService.obtenerRegistros(entidad));
      } catch {
        setRegistros([]);
      } finally {
        setLoading(false);
      }
    };
    fetch();
  }, [entidad]);

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold flex gap-2 text-white">
            <Shield /> Auditoría
          </h1>
          <p className="text-slate-400 text-sm mt-1">
            Trazabilidad de operaciones por entidad.
          </p>
        </div>
        <select
          value={entidad}
          onChange={(e) => setEntidad(e.target.value)}
          className="px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100"
        >
          <option value="CITA">Citas</option>
          <option value="USUARIO">Usuarios</option>
          <option value="PAGO">Pagos</option>
        </select>
      </header>

      <div className="border border-slate-800/80 rounded-xl overflow-hidden bg-slate-900/60">
        <table className="w-full text-left text-sm">
          <thead className="bg-slate-950/70 border-b border-slate-800/80 text-slate-300">
            <tr>
              <th className="p-4">Acción</th>
              <th className="p-4">Usuario</th>
              <th className="p-4">Entidad</th>
              <th className="p-4">Cambios</th>
              <th className="p-4">Fecha</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={5} className="p-8 text-center">
                  <Loader2 className="animate-spin mx-auto text-indigo-400" />
                </td>
              </tr>
            ) : registros.length === 0 ? (
              <tr>
                <td colSpan={5} className="p-6 text-center text-slate-400">
                  No hay registros para esta entidad.
                </td>
              </tr>
            ) : (
              registros.map((r) => (
                <tr key={r.id} className="border-b border-slate-800/80">
                  <td className="p-4 text-white">{r.accion}</td>
                  <td className="p-4 text-slate-300">{r.usuarioId || "Sistema"}</td>
                  <td className="p-4 text-slate-300">{r.entidad}</td>
                  <td className="p-4 text-slate-400">
                    {r.valorAnterior ? `${r.valorAnterior} → ` : ""}
                    {r.valorNuevo || "-"}
                  </td>
                  <td className="p-4 text-slate-400">{new Date(r.fecha).toLocaleString()}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
