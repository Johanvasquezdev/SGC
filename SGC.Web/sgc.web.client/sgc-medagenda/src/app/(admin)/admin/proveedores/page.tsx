"use client";

import { useEffect, useState } from "react";
import { Building2, Plus, Loader2, X } from "lucide-react";
import { ProveedorSaludDTO, ProveedorSaludService } from "@/services/proveedor.service";

export default function ProveedoresPage() {
  const [proveedores, setProveedores] = useState<ProveedorSaludDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [proveedorSeleccionado, setProveedorSeleccionado] = useState<ProveedorSaludDTO | null>(null);
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

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <header className="flex justify-between items-center">
        <h1 className="text-2xl font-bold flex items-center gap-2 text-white">
          <Building2 className="text-indigo-400" /> Proveedores de Salud (ARS)
        </h1>
        <button
          onClick={abrirCrear}
          className="bg-indigo-600 text-white px-4 py-2 rounded-lg flex items-center gap-2 text-sm hover:bg-indigo-500"
        >
          <Plus className="w-4 h-4" /> Registrar ARS
        </button>
      </header>

      <div className="bg-slate-900/60 border border-slate-800/80 rounded-xl overflow-hidden">
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
            ) : (
              proveedores.map((prov) => (
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
