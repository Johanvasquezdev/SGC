"use client";

import { useState, useEffect, useMemo } from "react";
import { Users, Loader2, Search, UserCheck, UserX } from "lucide-react";
import { UsuarioResponse } from "@/types/api.types";
import { UsuarioService } from "@/services/usuario.service";

export default function UsuariosPage() {
  const [usuarios, setUsuarios] = useState<UsuarioResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [query, setQuery] = useState("");

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

  const totalUsuarios = usuarios.length;
  const usuariosActivos = useMemo(() => usuarios.filter((u) => u.activo).length, [usuarios]);
  const usuariosInactivos = totalUsuarios - usuariosActivos;

  const usuariosFiltrados = useMemo(() => {
    const term = query.trim().toLowerCase();
    if (!term) return usuarios;

    return usuarios.filter((u) => {
      return (
        u.nombre.toLowerCase().includes(term) ||
        u.email.toLowerCase().includes(term) ||
        u.rol.toLowerCase().includes(term)
      );
    });
  }, [usuarios, query]);

  return (
    <div className="mx-auto max-w-7xl space-y-6 p-6">
      <header className="relative overflow-hidden rounded-3xl border border-cyan-500/20 bg-gradient-to-br from-cyan-500/15 via-slate-900 to-indigo-500/20 p-6 md:p-7">
        <div className="absolute -right-16 -top-16 h-52 w-52 rounded-full bg-cyan-500/20 blur-3xl" />
        <div className="absolute -bottom-20 -left-20 h-56 w-56 rounded-full bg-indigo-500/20 blur-3xl" />

        <div className="relative z-10 flex flex-col gap-6 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.2em] text-cyan-300/90">
              Administracion
            </p>
            <h1 className="mt-2 flex items-center gap-2 text-3xl font-bold tracking-tight text-white">
              <Users className="h-7 w-7 text-cyan-300" />
              Gestion de Usuarios
            </h1>
            <p className="mt-2 max-w-2xl text-slate-300">
              Administra activacion de cuentas y controla el acceso general de la plataforma.
            </p>
          </div>

          <div className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 backdrop-blur-sm">
            <p className="text-xs text-slate-300">Registros totales</p>
            <p className="text-2xl font-bold text-white">{loading ? "--" : totalUsuarios}</p>
          </div>
        </div>
      </header>

      <section className="grid gap-4 md:grid-cols-3">
        <article className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-5">
          <p className="text-sm text-slate-400">Total usuarios</p>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : totalUsuarios}</p>
        </article>

        <article className="rounded-2xl border border-emerald-500/30 bg-emerald-500/10 p-5">
          <div className="flex items-center gap-2 text-emerald-300">
            <UserCheck className="h-4 w-4" />
            <p className="text-sm">Activos</p>
          </div>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : usuariosActivos}</p>
        </article>

        <article className="rounded-2xl border border-rose-500/30 bg-rose-500/10 p-5">
          <div className="flex items-center gap-2 text-rose-300">
            <UserX className="h-4 w-4" />
            <p className="text-sm">Inactivos</p>
          </div>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : usuariosInactivos}</p>
        </article>
      </section>

      <section className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-4">
        <label className="relative block">
          <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-500" />
          <input
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Buscar por nombre, email o rol"
            className="w-full rounded-xl border border-slate-800 bg-slate-950/70 py-2.5 pl-10 pr-3 text-sm text-slate-100 placeholder:text-slate-500"
          />
        </label>
      </section>

      <div className="overflow-hidden rounded-2xl border border-slate-800/80 bg-slate-900/70">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-slate-800 bg-slate-950/70">
            <tr>
              <th className="p-4 text-slate-300">Usuario</th>
              <th className="p-4 text-slate-300">Rol</th>
              <th className="p-4 text-slate-300">Estado</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={3} className="p-8 text-center">
                  <Loader2 className="mx-auto h-5 w-5 animate-spin text-cyan-300" />
                </td>
              </tr>
            ) : usuariosFiltrados.length === 0 ? (
              <tr>
                <td colSpan={3} className="p-6 text-center text-slate-400">
                  No se encontraron usuarios para el filtro actual.
                </td>
              </tr>
            ) : (
              usuariosFiltrados.map((u) => (
                <tr key={u.id} className="border-b border-slate-800/80">
                  <td className="p-4 text-slate-100">
                    <p className="font-medium text-white">{u.nombre}</p>
                    <p className="text-xs text-slate-400">{u.email}</p>
                  </td>
                  <td className="p-4 text-slate-200">{u.rol}</td>
                  <td className="p-4">
                    <button
                      onClick={() => toggleActivo(u.id, u.activo)}
                      className={`rounded-full border px-3 py-1 text-xs font-medium transition-colors ${
                        u.activo
                          ? "border-emerald-500/30 bg-emerald-500/10 text-emerald-300 hover:bg-emerald-500/20"
                          : "border-rose-500/30 bg-rose-500/10 text-rose-300 hover:bg-rose-500/20"
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
