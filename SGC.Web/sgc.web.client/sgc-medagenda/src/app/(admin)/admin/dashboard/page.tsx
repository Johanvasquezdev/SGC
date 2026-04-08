"use client";

import { useEffect, useState } from "react";
import { Users, Stethoscope, ClipboardList, Building2, ArrowUpRight, ShieldAlert } from "lucide-react";
import Link from "next/link";
import { UsuarioService } from "@/services/usuario.service";
import { MedicoService } from "@/services/medico.service";
import { EspecialidadService } from "@/services/especialidad.service";
import { ProveedorSaludService } from "@/services/proveedor.service";

interface StatCard {
  title: string;
  value: string;
  icon: React.ElementType;
  color: string;
  bg: string;
  trend: string;
}

export default function AdminDashboard() {
  const [stats, setStats] = useState<StatCard[]>([
    { title: "Pacientes Activos", value: "--", icon: Users, color: "text-blue-400", bg: "bg-blue-500/10", trend: "..." },
    { title: "Médicos Activos", value: "--", icon: Stethoscope, color: "text-emerald-400", bg: "bg-emerald-500/10", trend: "..." },
    { title: "Especialidades", value: "--", icon: ClipboardList, color: "text-indigo-400", bg: "bg-indigo-500/10", trend: "..." },
    { title: "Proveedores", value: "--", icon: Building2, color: "text-amber-400", bg: "bg-amber-500/10", trend: "..." },
  ]);

  useEffect(() => {
    const cargarStats = async () => {
      try {
        const [pacientes, medicos, especialidades, proveedores] = await Promise.all([
          UsuarioService.obtenerTodos("Paciente"),
          MedicoService.obtenerTodos(),
          EspecialidadService.obtenerTodas(),
          ProveedorSaludService.obtenerTodos(),
        ]);

        const medicosActivos = medicos.filter((m) => m.activo).length;

        setStats([
          { title: "Pacientes Activos", value: pacientes.length.toString(), icon: Users, color: "text-blue-400", bg: "bg-blue-500/10", trend: "Total" },
          { title: "Médicos Activos", value: medicosActivos.toString(), icon: Stethoscope, color: "text-emerald-400", bg: "bg-emerald-500/10", trend: "Activos" },
          { title: "Especialidades", value: especialidades.length.toString(), icon: ClipboardList, color: "text-indigo-400", bg: "bg-indigo-500/10", trend: "Registradas" },
          { title: "Proveedores", value: proveedores.length.toString(), icon: Building2, color: "text-amber-400", bg: "bg-amber-500/10", trend: "ARS" },
        ]);
      } catch {
        // Si falla, dejamos los placeholders.
      }
    };

    cargarStats();
  }, []);

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-500">
      <header>
        <h1 className="text-3xl font-bold tracking-tight text-white">Panel de Control General</h1>
        <p className="text-slate-400 mt-1">Resumen de actividad del sistema MedAgenda.</p>
      </header>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat, i) => (
          <div key={i} className="bg-slate-900/60 p-6 rounded-2xl border border-slate-800/80 shadow-sm">
            <div className="flex justify-between items-start mb-4">
              <div className={`p-3 rounded-xl ${stat.bg}`}>
                <stat.icon className={`w-6 h-6 ${stat.color}`} />
              </div>
              <span className="text-xs font-medium text-emerald-300 bg-emerald-500/10 px-2 py-1 rounded-full flex items-center gap-1">
                {stat.trend} <ArrowUpRight className="w-3 h-3" />
              </span>
            </div>
            <p className="text-sm font-medium text-slate-400">{stat.title}</p>
            <p className="text-2xl font-bold text-white mt-1">{stat.value}</p>
          </div>
        ))}
      </div>

      <div className="grid md:grid-cols-2 gap-6">
        <div className="bg-slate-900/60 p-6 rounded-2xl border border-slate-800/80 shadow-sm">
          <h3 className="text-lg font-semibold mb-4 text-white">Accesos Frecuentes</h3>
          <div className="grid grid-cols-2 gap-4">
            <Link href="/admin/usuarios" className="p-4 rounded-xl border border-slate-800 bg-slate-950/70 hover:bg-slate-900 transition-colors flex flex-col items-center justify-center gap-2 text-slate-200">
              <Users className="w-6 h-6 text-indigo-400" /> Gestión Usuarios
            </Link>
            <Link href="/admin/auditoria" className="p-4 rounded-xl border border-slate-800 bg-slate-950/70 hover:bg-slate-900 transition-colors flex flex-col items-center justify-center gap-2 text-slate-200">
              <ShieldAlert className="w-6 h-6 text-rose-400" /> Ver Auditoría
            </Link>
          </div>
        </div>

        <div className="bg-gradient-to-br from-indigo-500 to-purple-600 p-6 rounded-2xl text-white shadow-md flex flex-col justify-center">
          <h3 className="text-xl font-bold mb-2">Sistema Operativo</h3>
          <p className="text-indigo-100 mb-4">Todos los servicios (API, Base de Datos, SignalR) están funcionando correctamente.</p>
          <div className="bg-white/20 w-fit px-4 py-2 rounded-lg backdrop-blur-sm flex items-center gap-2 font-medium">
            <div className="w-2 h-2 bg-emerald-400 rounded-full animate-pulse"></div>
            Estado: Óptimo
          </div>
        </div>
      </div>
    </div>
  );
}
