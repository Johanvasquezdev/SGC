"use client";

import { useEffect, useMemo, useState } from "react";
import { Building2, Plus, Loader2, X, Search, CheckCircle2, XCircle } from "lucide-react";
import { ProveedorSaludDTO, ProveedorSaludService } from "@/services/proveedor.service";

export default function ProveedoresPage() {
  const [proveedores, setProveedores] = useState<ProveedorSaludDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [proveedorSeleccionado, setProveedorSeleccionado] = useState<ProveedorSaludDTO | null>(null);
  const [query, setQuery] = useState("");
  const [formData, setFormData] = useState({
    nombre: "",
    tipo: "",
    telefono: "",
    email: "",
  });

  const cargar = async () => {
    setLoading(true);
    try {
      const data = await ProveedorSaludService.obtenerTodos();
      setProveedores(data);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargar();
  }, []);

  const abrirCrear = () => {
    setModoEdicion(false);
    setProveedorSeleccionado(null);
    setFormData({ nombre: "", tipo: "", telefono: "", email: "" });
    setIsModalOpen(true);
  };

  const abrirEditar = (prov: ProveedorSaludDTO) => {
    setModoEdicion(true);
    setProveedorSeleccionado(prov);
    setFormData({
      nombre: prov.nombre,
      tipo: prov.tipo || "",
      telefono: prov.telefono || "",
      email: prov.email || "",
    });
    setIsModalOpen(true);
  };

  const cerrarModal = () => {
    setIsModalOpen(false);
    setProveedorSeleccionado(null);
  };

  const guardar = async (e: React.FormEvent) => {
    e.preventDefault();
    if (modoEdicion && proveedorSeleccionado) {
      await ProveedorSaludService.actualizar(proveedorSeleccionado.id, formData);
    } else {
      await ProveedorSaludService.crear(formData);
    }
    await cargar();
    cerrarModal();
  };

  const totalProveedores = proveedores.length;
  const proveedoresActivos = useMemo(() => proveedores.filter((p) => p.activo).length, [proveedores]);
  const proveedoresInactivos = totalProveedores - proveedoresActivos;

  const proveedoresFiltrados = useMemo(() => {
    const term = query.trim().toLowerCase();
    if (!term) return proveedores;

    return proveedores.filter((p) => {
      return (
        p.nombre.toLowerCase().includes(term) ||
        (p.tipo || "").toLowerCase().includes(term) ||
        (p.email || "").toLowerCase().includes(term) ||
        (p.telefono || "").toLowerCase().includes(term)
      );
    });
  }, [proveedores, query]);

  return (
    <div className="mx-auto max-w-7xl space-y-6 p-6">
      <header className="relative overflow-hidden rounded-3xl border border-indigo-500/20 bg-gradient-to-br from-indigo-500/15 via-slate-900 to-cyan-500/15 p-6 md:p-7">
        <div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-indigo-500/20 blur-3xl" />
        <div className="absolute -bottom-20 -left-20 h-56 w-56 rounded-full bg-cyan-500/20 blur-3xl" />

        <div className="relative z-10 flex flex-col gap-6 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.2em] text-indigo-300/90">
              Administracion
            </p>
            <h1 className="mt-2 flex items-center gap-2 text-3xl font-bold tracking-tight text-white">
              <Building2 className="h-7 w-7 text-indigo-300" />
              Proveedores de Salud
            </h1>
            <p className="mt-2 max-w-2xl text-slate-300">
              Gestiona ARS, clinicas y hospitales para mantener actualizado el ecosistema de convenios.
            </p>
          </div>

          <button
            onClick={abrirCrear}
            className="inline-flex items-center gap-2 rounded-xl bg-indigo-500/20 px-4 py-2 text-sm font-medium text-indigo-100 transition-colors hover:bg-indigo-500/30"
          >
            <Plus className="h-4 w-4" /> Registrar proveedor
          </button>
        </div>
      </header>

      <section className="grid gap-4 md:grid-cols-3">
        <article className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-5">
          <p className="text-sm text-slate-400">Total proveedores</p>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : totalProveedores}</p>
        </article>

        <article className="rounded-2xl border border-emerald-500/30 bg-emerald-500/10 p-5">
          <div className="flex items-center gap-2 text-emerald-300">
            <CheckCircle2 className="h-4 w-4" />
            <p className="text-sm">Activos</p>
          </div>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : proveedoresActivos}</p>
        </article>

        <article className="rounded-2xl border border-rose-500/30 bg-rose-500/10 p-5">
          <div className="flex items-center gap-2 text-rose-300">
            <XCircle className="h-4 w-4" />
            <p className="text-sm">Inactivos</p>
          </div>
          <p className="mt-1 text-2xl font-bold text-white">{loading ? "--" : proveedoresInactivos}</p>
        </article>
      </section>

      <section className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-4">
        <label className="relative block">
          <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-500" />
          <input
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Buscar por nombre, tipo, telefono o email"
            className="w-full rounded-xl border border-slate-800 bg-slate-950/70 py-2.5 pl-10 pr-3 text-sm text-slate-100 placeholder:text-slate-500"
          />
        </label>
      </section>

      <div className="overflow-hidden rounded-2xl border border-slate-800/80 bg-slate-900/70">
        <table className="w-full text-left text-sm">
          <thead className="bg-slate-950/70 border-b border-slate-800/80 text-slate-300">
            <tr>
              <th className="p-4">Nombre</th>
              <th className="p-4">Tipo</th>
              <th className="p-4">Contacto</th>
              <th className="p-4">Estado</th>
              <th className="p-4 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={5} className="p-8 text-center">
                  <Loader2 className="animate-spin mx-auto text-indigo-400" />
                </td>
              </tr>
            ) : proveedoresFiltrados.length === 0 ? (
              <tr>
                <td colSpan={5} className="p-6 text-center text-slate-400">
                  No se encontraron proveedores para el filtro actual.
                </td>
              </tr>
            ) : (
              proveedoresFiltrados.map((prov) => (
                <tr key={prov.id} className="border-b border-slate-800/80">
                  <td className="p-4 font-medium text-white">{prov.nombre}</td>
                  <td className="p-4 text-slate-400">{prov.tipo || "-"}</td>
                  <td className="p-4 text-slate-400">
                    <div>{prov.telefono || "-"}</div>
                    <div className="text-xs">{prov.email || "-"}</div>
                  </td>
                  <td className="p-4">
                    <span
                      className={`px-2 py-1 rounded-full text-xs border ${
                        prov.activo
                          ? "bg-emerald-500/10 text-emerald-300 border-emerald-500/30"
                          : "bg-rose-500/10 text-rose-300 border-rose-500/30"
                      }`}
                    >
                      {prov.activo ? "Activo" : "Inactivo"}
                    </span>
                  </td>
                  <td className="p-4 text-right">
                    <button
                      onClick={() => abrirEditar(prov)}
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
                {modoEdicion ? "Editar Proveedor" : "Registrar Proveedor"}
              </h2>
              <button onClick={cerrarModal} className="text-slate-400 hover:text-white">
                <X className="w-5 h-5" />
              </button>
            </div>
            <form onSubmit={guardar} className="p-5 space-y-4 text-slate-200">
              <input
                required
                placeholder="Nombre"
                className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                value={formData.nombre}
                onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
              />
              <input
                placeholder="Tipo (ARS, Clínica, Hospital)"
                className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                value={formData.tipo}
                onChange={(e) => setFormData({ ...formData, tipo: e.target.value })}
              />
              <input
                placeholder="Teléfono"
                className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                value={formData.telefono}
                onChange={(e) => setFormData({ ...formData, telefono: e.target.value })}
              />
              <input
                type="email"
                placeholder="Email"
                className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
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
