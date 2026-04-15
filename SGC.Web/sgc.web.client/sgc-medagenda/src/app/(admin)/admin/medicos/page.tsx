"use client";

import { useState, useEffect, useMemo } from "react";
import { Stethoscope, Search, Plus, CheckCircle2, XCircle, Loader2, Phone, X } from "lucide-react";
import Image from "next/image";
import { MedicoDTO, CreateMedicoRequest, UpdateMedicoRequest } from "@/types/medico.types";
import { MedicoService } from "@/services/medico.service";
import { EspecialidadService, EspecialidadDTO } from "@/services/especialidad.service";
import { ProveedorSaludService, ProveedorSaludDTO } from "@/services/proveedor.service";

export default function AdminMedicosPage() {
  const [medicos, setMedicos] = useState<MedicoDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [busqueda, setBusqueda] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [especialidades, setEspecialidades] = useState<EspecialidadDTO[]>([]);
  const [proveedores, setProveedores] = useState<ProveedorSaludDTO[]>([]);
  const [formData, setFormData] = useState<CreateMedicoRequest>({
    nombre: "",
    email: "",
    password: "",
    exequatur: "",
    especialidadId: null,
    proveedorSaludId: null,
    telefonoConsultorio: "",
    foto: "",
  });
  const [medicoSeleccionado, setMedicoSeleccionado] = useState<MedicoDTO | null>(null);

  const cargarMedicos = async () => {
    setIsLoading(true);
    try {
      const data = await MedicoService.obtenerTodos();
      setMedicos(data);
    } catch (error) {
      console.error("Error cargando médicos:", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    cargarMedicos();
    const cargarCatalogos = async () => {
      const [esp, prov] = await Promise.all([
        EspecialidadService.obtenerTodas(),
        ProveedorSaludService.obtenerTodos(),
      ]);
      setEspecialidades(esp.filter((e) => e.activo));
      setProveedores(prov.filter((p) => p.activo));
    };
    cargarCatalogos();
  }, []);

  // Filtrado dinámico en el cliente
  const medicosFiltrados = useMemo(() => {
    const term = busqueda.trim().toLowerCase();
    if (!term) return medicos;

    return medicos.filter((m) => {
      return (
        m.nombre.toLowerCase().includes(term) ||
        (m.especialidadNombre || "").toLowerCase().includes(term) ||
        m.exequatur.toLowerCase().includes(term) ||
        (m.email || "").toLowerCase().includes(term)
      );
    });
  }, [medicos, busqueda]);

  const abrirCrear = () => {
    setModoEdicion(false);
    setMedicoSeleccionado(null);
    setFormData({
      nombre: "",
      email: "",
      password: "",
      exequatur: "",
      especialidadId: null,
      proveedorSaludId: null,
      telefonoConsultorio: "",
      foto: "",
    });
    setIsModalOpen(true);
  };

  const abrirEditar = (medico: MedicoDTO) => {
    setModoEdicion(true);
    setMedicoSeleccionado(medico);
    setFormData({
      nombre: medico.nombre,
      email: medico.email,
      password: "",
      exequatur: medico.exequatur,
      especialidadId: medico.especialidadId ?? null,
      proveedorSaludId: medico.proveedorSaludId ?? null,
      telefonoConsultorio: medico.telefonoConsultorio || "",
      foto: medico.foto || "",
    });
    setIsModalOpen(true);
  };

  const cerrarModal = () => {
    setIsModalOpen(false);
    setMedicoSeleccionado(null);
  };

  const guardarMedico = async (e: React.FormEvent) => {
    e.preventDefault();
    if (modoEdicion && medicoSeleccionado) {
      const updateData: UpdateMedicoRequest = {
        nombre: formData.nombre,
        email: formData.email,
        exequatur: formData.exequatur,
        especialidadId: formData.especialidadId,
        proveedorSaludId: formData.proveedorSaludId,
        telefonoConsultorio: formData.telefonoConsultorio,
        foto: formData.foto,
      };
      await MedicoService.actualizar(medicoSeleccionado.id, updateData);
    } else {
      await MedicoService.crear(formData);
    }
    await cargarMedicos();
    cerrarModal();
  };

  const toggleActivo = async (medico: MedicoDTO) => {
    if (medico.activo) {
      await MedicoService.desactivar(medico.id);
    } else {
      await MedicoService.activar(medico.id);
    }
    await cargarMedicos();
  };

  const totalMedicos = medicos.length;
  const medicosActivos = medicos.filter((m) => m.activo).length;
  const medicosInactivos = totalMedicos - medicosActivos;

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6 animate-in fade-in duration-500">
      <header className="relative overflow-hidden rounded-3xl border border-cyan-500/20 bg-gradient-to-br from-cyan-500/15 via-slate-900 to-indigo-500/20 p-6 md:p-7">
        <div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-cyan-500/20 blur-3xl" />
        <div className="absolute -bottom-20 -left-20 h-56 w-56 rounded-full bg-indigo-500/20 blur-3xl" />

        <div className="relative z-10 flex flex-col gap-6 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.2em] text-cyan-300/90">
              Administracion
            </p>
            <h1 className="mt-2 flex items-center gap-2 text-3xl font-bold tracking-tight text-white">
              <Stethoscope className="h-7 w-7 text-cyan-300" />
              Directorio de Medicos
            </h1>
            <p className="mt-2 max-w-2xl text-slate-300">
              Gestiona perfiles profesionales, especialidades y estado operativo del personal medico.
            </p>
          </div>

          <button
            type="button"
            onClick={abrirCrear}
            className="inline-flex items-center gap-2 rounded-xl bg-cyan-500/20 px-4 py-2 text-sm font-medium text-cyan-100 transition-colors hover:bg-cyan-500/30"
          >
            <Plus aria-hidden="true" className="h-4 w-4" /> Registrar medico
          </button>
        </div>
      </header>

      <section className="grid gap-4 md:grid-cols-3">
        <article className="rounded-2xl border border-slate-800/80 bg-slate-900/70 p-5">
          <p className="text-sm text-slate-400">Total medicos</p>
          <p className="mt-1 text-2xl font-bold text-white">{isLoading ? "--" : totalMedicos}</p>
        </article>
        <article className="rounded-2xl border border-emerald-500/30 bg-emerald-500/10 p-5">
          <p className="text-sm text-emerald-300">Activos</p>
          <p className="mt-1 text-2xl font-bold text-white">{isLoading ? "--" : medicosActivos}</p>
        </article>
        <article className="rounded-2xl border border-rose-500/30 bg-rose-500/10 p-5">
          <p className="text-sm text-rose-300">Inactivos</p>
          <p className="mt-1 text-2xl font-bold text-white">{isLoading ? "--" : medicosInactivos}</p>
        </article>
      </section>

      <div className="bg-slate-900/60 rounded-xl shadow-sm border border-slate-800/80 overflow-hidden">
        <div className="p-4 border-b border-slate-800/80 flex flex-col gap-3 bg-slate-950/70 sm:flex-row sm:items-center sm:justify-between">
          <div className="relative w-full max-w-sm">
            <Search aria-hidden="true" className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
            <input 
              id="medicos-busqueda"
              aria-label="Buscar medicos"
              type="text"
              placeholder="Buscar por nombre, especialidad, exequatur o email"
              className="w-full pl-9 pr-4 py-2 rounded-lg border border-slate-800 bg-slate-950/70 text-sm focus:ring-2 focus:ring-indigo-500 outline-none transition-all text-white"
              value={busqueda}
              onChange={(e) => setBusqueda(e.target.value)}
            />
          </div>
          <p className="text-xs text-slate-400">
            {isLoading ? "Cargando..." : `${medicosFiltrados.length} resultado(s)`}
          </p>
        </div>

        {/* Tabla de Datos */}
        <div className="overflow-x-auto">
          <table className="w-full text-sm text-left">
            <thead className="text-xs text-slate-400 uppercase bg-slate-950/70 border-b border-slate-800/80">
              <tr>
                <th className="px-6 py-4 font-medium">Médico</th>
                <th className="px-6 py-4 font-medium">Especialidad</th>
                <th className="px-6 py-4 font-medium">Credenciales</th>
                <th className="px-6 py-4 font-medium text-center">Estado</th>
                <th className="px-6 py-4 font-medium text-right">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-800/80">
              {isLoading ? (
                <tr>
                  <td colSpan={5} className="px-6 py-12 text-center">
                    <Loader2 className="w-6 h-6 animate-spin text-indigo-400 mx-auto" />
                    <p className="text-slate-400 mt-2">Cargando directorio...</p>
                  </td>
                </tr>
              ) : medicosFiltrados.length === 0 ? (
                <tr>
                  <td colSpan={5} className="px-6 py-8 text-center text-slate-400">
                    No se encontraron medicos para el filtro actual.
                  </td>
                </tr>
              ) : (
                medicosFiltrados.map((medico) => (
                  <tr key={medico.id} className="hover:bg-slate-950/60 transition-colors">
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-3">
                        <Image
                          src={medico.foto || `https://ui-avatars.com/api/?name=${encodeURIComponent(medico.nombre)}&background=e0e7ff&color=4338ca`}
                          alt="Avatar"
                          width={40}
                          height={40}
                          unoptimized
                          className="w-10 h-10 rounded-full object-cover border border-slate-700"
                        />
                        <div>
                          <div className="font-semibold text-white">{medico.nombre}</div>
                          <div className="text-slate-400 text-xs mt-0.5">{medico.email}</div>
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <span className="bg-emerald-500/10 text-emerald-300 px-2.5 py-1 rounded-md text-xs font-medium border border-emerald-500/30">
                        {medico.especialidadNombre || "No asignada"}
                      </span>
                    </td>
                    <td className="px-6 py-4">
                      <div className="text-white text-xs font-mono font-medium mb-1">
                        Exq: {medico.exequatur}
                      </div>
                      <div className="text-slate-400 text-xs flex items-center gap-1">
                        <Phone className="w-3 h-3" /> {medico.telefonoConsultorio}
                      </div>
                    </td>
                    <td className="px-6 py-4 text-center">
                       <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium border ${
                          medico.activo 
                            ? 'bg-emerald-500/10 border-emerald-500/30 text-emerald-300' 
                            : 'bg-rose-500/10 border-rose-500/30 text-rose-300'
                        }`}>
                        {medico.activo ? <CheckCircle2 className="w-3.5 h-3.5" /> : <XCircle className="w-3.5 h-3.5" />}
                        {medico.activo ? "Activo" : "Inactivo"}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-right">
                      <div className="flex items-center justify-end gap-2">
                        <button
                          type="button"
                          onClick={() => abrirEditar(medico)}
                          className="px-3 py-1.5 rounded-lg text-xs border border-slate-700 text-slate-200 hover:bg-slate-800"
                        >
                          Editar
                        </button>
                        <button
                          type="button"
                          onClick={() => toggleActivo(medico)}
                          className={`px-3 py-1.5 rounded-lg text-xs border ${
                            medico.activo
                              ? "border-rose-500/40 text-rose-300 hover:bg-rose-500/10"
                              : "border-emerald-500/40 text-emerald-300 hover:bg-emerald-500/10"
                          }`}
                        >
                          {medico.activo ? "Desactivar" : "Activar"}
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

      {isModalOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/70 backdrop-blur-sm p-4">
          <div className="w-full max-w-lg bg-slate-950 border border-slate-800 rounded-2xl overflow-hidden">
            <div className="flex items-center justify-between px-5 py-4 border-b border-slate-800">
              <h2 className="text-lg font-semibold text-white">
                {modoEdicion ? "Editar Médico" : "Registrar Médico"}
              </h2>
              <button type="button" onClick={cerrarModal} aria-label="Cerrar modal" className="text-slate-400 hover:text-white">
                <X aria-hidden="true" className="w-5 h-5" />
              </button>
            </div>
            <form onSubmit={guardarMedico} className="p-5 space-y-4 text-slate-200">
              <div className="grid gap-4">
                <input
                  aria-label="Nombre completo"
                  required
                  placeholder="Nombre completo"
                  className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                  value={formData.nombre}
                  onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
                />
                <input
                  aria-label="Correo electronico"
                  required
                  type="email"
                  placeholder="Email"
                  className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                  value={formData.email}
                  onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                />
                {!modoEdicion && (
                  <input
                    aria-label="Contrasena"
                    required
                    type="password"
                    placeholder="Contraseña"
                    className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                    value={formData.password}
                    onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                  />
                )}
                <input
                  aria-label="Exequatur"
                  placeholder="Exequatur"
                  className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                  value={formData.exequatur || ""}
                  onChange={(e) => setFormData({ ...formData, exequatur: e.target.value })}
                />
                <select
                  aria-label="Seleccionar especialidad"
                  className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100"
                  value={formData.especialidadId ?? ""}
                  onChange={(e) =>
                    setFormData({ ...formData, especialidadId: e.target.value ? Number(e.target.value) : null })
                  }
                >
                  <option value="">Selecciona especialidad</option>
                  {especialidades.map((esp) => (
                    <option key={esp.id} value={esp.id}>
                      {esp.nombre}
                    </option>
                  ))}
                </select>
                <select
                  aria-label="Seleccionar proveedor de salud"
                  className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100"
                  value={formData.proveedorSaludId ?? ""}
                  onChange={(e) =>
                    setFormData({ ...formData, proveedorSaludId: e.target.value ? Number(e.target.value) : null })
                  }
                >
                  <option value="">Selecciona proveedor</option>
                  {proveedores.map((prov) => (
                    <option key={prov.id} value={prov.id}>
                      {prov.nombre}
                    </option>
                  ))}
                </select>
                <input
                  aria-label="Telefono de consultorio"
                  placeholder="Teléfono consultorio"
                  className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                  value={formData.telefonoConsultorio || ""}
                  onChange={(e) => setFormData({ ...formData, telefonoConsultorio: e.target.value })}
                />
                <input
                  aria-label="URL de foto"
                  placeholder="Foto (URL)"
                  className="w-full px-4 py-2 rounded-xl bg-slate-950/70 border border-slate-800 text-slate-100 placeholder:text-slate-500"
                  value={formData.foto || ""}
                  onChange={(e) => setFormData({ ...formData, foto: e.target.value })}
                />
              </div>

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
