"use client";

import { useEffect, useMemo, useState } from "react";
import { Stethoscope, Plus, Loader2, X, Search } from "lucide-react";
import { EspecialidadDTO, EspecialidadService } from "@/services/especialidad.service";

export default function EspecialidadesPage() {
  const nombreInputId = "especialidad-nombre";
  const descripcionInputId = "especialidad-descripcion";
  const busquedaInputId = "especialidades-busqueda";
  const [especialidades, setEspecialidades] = useState<EspecialidadDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [especialidadSeleccionada, setEspecialidadSeleccionada] = useState<EspecialidadDTO | null>(null);
  const [query, setQuery] = useState("");
  const [formData, setFormData] = useState({ nombre: "", descripcion: "" });

  const cargar = async () => {
    setLoading(true);
    try {
      const data = await EspecialidadService.obtenerTodas();
      setEspecialidades(data);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargar();
  }, []);

  const abrirCrear = () => {
    setModoEdicion(false);
    setEspecialidadSeleccionada(null);
    setFormData({ nombre: "", descripcion: "" });
    setIsModalOpen(true);
  };

  const abrirEditar = (esp: EspecialidadDTO) => {
    setModoEdicion(true);
    setEspecialidadSeleccionada(esp);
    setFormData({ nombre: esp.nombre, descripcion: esp.descripcion || "" });
    setIsModalOpen(true);
  };

  const cerrarModal = () => {
    setIsModalOpen(false);
    setEspecialidadSeleccionada(null);
  };

  const guardar = async (e: React.FormEvent) => {
    e.preventDefault();
    if (modoEdicion && especialidadSeleccionada) {
      await EspecialidadService.actualizar(especialidadSeleccionada.id, formData);
    } else {
      await EspecialidadService.crear(formData);
    }
    await cargar();
    cerrarModal();
  };

  const totalEspecialidades = especialidades.length;
  const activas = especialidades.filter((e) => e.activo).length;
  const inactivas = totalEspecialidades - activas;

  const especialidadesFiltradas = useMemo(() => {
    const term = query.trim().toLowerCase();
    if (!term) return especialidades;

    return especialidades.filter((e) => {
      return (
        e.nombre.toLowerCase().includes(term) ||
        (e.descripcion || "").toLowerCase().includes(term)
      );
    });
  }, [especialidades, query]);

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <header className="relative overflow-hidden rounded-3xl border border-indigo-500/20 bg-gradient-to-br from-indigo-500/15 via-slate-900 to-emerald-500/15 p-6 md:p-7">
        <div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-indigo-500/20 blur-3xl" />
        <div className="absolute -bottom-20 -left-20 h-56 w-56 rounded-full bg-emerald-500/20 blur-3xl" />

        <div className="relative z-10 flex flex-col gap-6 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.2em] text-indigo-300/90">
              Administracion
            </p>
            <h1 className="mt-2 flex items-center gap-2 text-3xl font-bold tracking-tight text-white">
              <Stethoscope className="h-7 w-7 text-indigo-300" />
              Especialidades
            </h1>
            <p className="mt-2 max-w-2xl text-slate-300">
              Mantiene actualizado el catalogo de especialidades medicas para asignacion en perfiles y agendas.
            </p>
          </div>

          <button
            type="button"
            onClick={abrirCrear}
            className="inline-flex items-center gap-2 rounded-xl bg-indigo-500/20 px-4 py-2 text-sm font-medium text-indigo-100 transition-colors hover:bg-indigo-500/30"
          >
            <Plus aria-hidden="true" className="h-4 w-4" /> Anadir nueva
          </button>
        </div>
      </header>

      <section className="grid gap-4 md:grid-cols-3">
        <article className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-5">
          <p className="text-sm text-slate-400">Total especialidades</p>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : totalEspecialidades}</p>
        </article>
        <article className="rounded-2xl border border-emerald-500/30 bg-emerald-500/10 p-5">
          <p className="text-sm text-emerald-300">Activas</p>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : activas}</p>
        </article>
        <article className="rounded-2xl border border-rose-500/30 bg-rose-500/10 p-5">
          <p className="text-sm text-rose-300">Inactivas</p>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : inactivas}</p>
        </article>
      </section>

      <section className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-4">
        <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <label htmlFor={busquedaInputId} className="relative block w-full sm:max-w-sm">
            <Search aria-hidden="true" className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-500" />
            <input
              id={busquedaInputId}
              aria-label="Buscar especialidades"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              placeholder="Buscar por nombre o descripcion"
              className="w-full rounded-xl border border-slate-800 bg-slate-950/70 py-2.5 pl-10 pr-3 text-sm text-slate-100 placeholder:text-slate-500"
            />
          </label>
          <p className="text-xs text-slate-400">
            {loading ? "Cargando..." : `${especialidadesFiltradas.length} resultado(s)`}
          </p>
        </div>
      </section>

      <div className="bg-slate-900/70 border border-slate-800/80 rounded-2xl overflow-hidden">
        <table className="w-full text-left text-sm">
          <thead className="bg-slate-950/70 border-b border-slate-800/80 text-slate-300">
            <tr>
              <th className="p-4">Nombre</th>
              <th className="p-4">Descripción</th>
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
            ) : especialidadesFiltradas.length === 0 ? (
              <tr>
                <td colSpan={4} className="p-6 text-center text-slate-400">
                  No se encontraron especialidades para el filtro actual.
                </td>
              </tr>
            ) : (
              especialidadesFiltradas.map((esp) => (
                <tr key={esp.id} className="border-b border-slate-800/80">
                  <td className="p-4 font-medium text-white">{esp.nombre}</td>
                  <td className="p-4 text-slate-400 truncate max-w-xs">{esp.descripcion}</td>
                  <td className="p-4">
                    <span
                      className={`px-2 py-1 rounded-full text-xs border ${
                        esp.activo
                          ? "bg-emerald-500/10 text-emerald-300 border-emerald-500/30"
                          : "bg-rose-500/10 text-rose-300 border-rose-500/30"
                      }`}
                    >
                      {esp.activo ? "Activo" : "Inactivo"}
                    </span>
                  </td>
                  <td className="p-4 text-right">
                    <button
                      type="button"
                      onClick={() => abrirEditar(esp)}
                      className="px-3 py-1.5 rounded-lg text-xs border border-slate-700 text-slate-200 hover:bg-slate-800"
                    >
                      Editar
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {isModalOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/70 backdrop-blur-sm p-4">
          <div className="w-full max-w-lg bg-slate-950 border border-slate-800 rounded-2xl overflow-hidden">
            <div className="flex items-center justify-between px-5 py-4 border-b border-slate-800">
              <h2 className="text-lg font-semibold text-white">
                {modoEdicion ? "Editar Especialidad" : "Registrar Especialidad"}
              </h2>
              <button type="button" onClick={cerrarModal} aria-label="Cerrar modal" className="text-slate-400 hover:text-white">
                <X aria-hidden="true" className="w-5 h-5" />
              </button>
            </div>
            <form onSubmit={guardar} className="p-5 space-y-4 text-slate-200">
              <label htmlFor={nombreInputId} className="sr-only">
                Nombre de especialidad
              </label>
              <input
                id={nombreInputId}
                required
                placeholder="Nombre"
                className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                value={formData.nombre}
                onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
              />
              <label htmlFor={descripcionInputId} className="sr-only">
                Descripcion de especialidad
              </label>
              <textarea
                id={descripcionInputId}
                placeholder="Descripción"
                className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                value={formData.descripcion}
                onChange={(e) => setFormData({ ...formData, descripcion: e.target.value })}
              />

              <div className="flex justify-end gap-2 pt-2">
                <button
                  type="button"
                  onClick={cerrarModal}
                  className="px-4 py-2 rounded-xl border border-slate-700 text-slate-200 hover:bg-slate-800"
                >
                  Cancelar
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 rounded-xl bg-indigo-600 hover:bg-indigo-500 text-white"
                >
                  {modoEdicion ? "Guardar Cambios" : "Registrar"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
