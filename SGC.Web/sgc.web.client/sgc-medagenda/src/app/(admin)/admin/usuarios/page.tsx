"use client";

import { useState, useEffect } from "react";
import { Users, Loader2 } from "lucide-react";
import { UsuarioResponse } from "@/types/api.types";
import { UsuarioService } from "@/services/usuario.service";

export default function UsuariosPage() {
  const [usuarios, setUsuarios] = useState<UsuarioResponse[]>([]);
  const [loading, setLoading] = useState(true);

  const cargar = async () => {
    setLoading(true);
    setUsuarios(await UsuarioService.obtenerTodos());
    setLoading(false);
  };

  useEffect(() => {
    queueMicrotask(() => {
      void cargar();
    });
  }, []);

  const toggleActivo = async (id: number, activo: boolean) => {
    setUsuarios((u) => u.map((x) => (x.id === id ? { ...x, activo: !activo } : x)));
    try {
      activo ? await UsuarioService.desactivar(id) : await UsuarioService.activar(id);
    } catch {
      cargar();
    }
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <h1 className="text-2xl font-bold flex gap-2 text-white">
        <Users /> Gestión de Usuarios
      </h1>

      <div className="border border-slate-800/80 rounded-xl overflow-hidden bg-slate-900/60">
        <table className="w-full text-left text-sm">
          <thead className="bg-slate-950/70 border-b border-slate-800">
            <tr>
              <th className="p-4 text-slate-300">Nombre</th>
              <th className="p-4 text-slate-300">Rol</th>
              <th className="p-4 text-slate-300">Estado</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={3} className="p-6 text-center">
                  <Loader2 className="animate-spin mx-auto text-emerald-400" />
                </td>
              </tr>
            ) : (
              usuarios.map((u) => (
                <tr key={u.id} className="border-b border-slate-800/80">
                  <td className="p-4 text-slate-100">
                    {u.nombre}
                    <br />
                    <span className="text-xs text-slate-400">{u.email}</span>
                  </td>
                  <td className="p-4 text-slate-200">{u.rol}</td>
                  <td className="p-4">
                    <button
                      onClick={() => toggleActivo(u.id, u.activo)}
                      className={`px-3 py-1 rounded-full text-xs border ${
                        u.activo
                          ? "bg-emerald-500/10 text-emerald-300 border-emerald-500/30"
                          : "bg-rose-500/10 text-rose-300 border-rose-500/30"
                      }`}
                    >
                      {u.activo ? "Activo" : "Inactivo"}
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
