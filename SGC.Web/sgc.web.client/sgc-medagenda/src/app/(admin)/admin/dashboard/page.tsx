"use client";

import { useEffect, useState } from "react";
import {
  Users,
  Stethoscope,
  ClipboardList,
  Building2,
  ArrowUpRight,
  ShieldAlert,
  Activity,
  CalendarDays,
  UserCog,
  BadgeCheck,
  ChevronRight,
} from "lucide-react";
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
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState<StatCard[]>([
    {
      title: "Pacientes Activos",
      value: "--",
      icon: Users,
      color: "text-blue-400",
      bg: "bg-blue-500/10",
      trend: "Cargando",
    },
    {
      title: "Médicos Activos",
      value: "--",
      icon: Stethoscope,
      color: "text-emerald-400",
      bg: "bg-emerald-500/10",
      trend: "Cargando",
    },
    {
      title: "Especialidades",
      value: "--",
      icon: ClipboardList,
      color: "text-indigo-400",
      bg: "bg-indigo-500/10",
      trend: "Cargando",
    },
    {
      title: "Proveedores",
      value: "--",
      icon: Building2,
      color: "text-amber-400",
      bg: "bg-amber-500/10",
      trend: "Cargando",
    },
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
          {
            title: "Pacientes Activos",
            value: pacientes.length.toString(),
            icon: Users,
            color: "text-blue-400",
            bg: "bg-blue-500/10",
            trend: "Total",
          },
          {
            title: "Médicos Activos",
            value: medicosActivos.toString(),
            icon: Stethoscope,
            color: "text-emerald-400",
            bg: "bg-emerald-500/10",
            trend: "Activos",
          },
          {
            title: "Especialidades",
            value: especialidades.length.toString(),
            icon: ClipboardList,
            color: "text-indigo-400",
            bg: "bg-indigo-500/10",
            trend: "Registradas",
          },
          {
            title: "Proveedores",
            value: proveedores.length.toString(),
            icon: Building2,
            color: "text-amber-400",
            bg: "bg-amber-500/10",
            trend: "ARS",
          },
        ]);
      } catch {
        // Si falla, dejamos los placeholders.
      } finally {
        setLoading(false);
      }
    };

    cargarStats();
  }, []);

  return (
    <div className="mx-auto max-w-7xl space-y-8 animate-in fade-in duration-500">
      <header className="relative overflow-hidden rounded-3xl border border-indigo-500/20 bg-gradient-to-br from-indigo-500/15 via-slate-900 to-emerald-500/15 p-6 md:p-8">
        <div className="absolute -right-24 -top-24 h-64 w-64 rounded-full bg-indigo-500/20 blur-3xl" />
        <div className="absolute -bottom-24 -left-24 h-64 w-64 rounded-full bg-emerald-500/20 blur-3xl" />

        <div className="relative z-10 flex flex-col gap-6 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.2em] text-indigo-300/90">
              Administracion
            </p>
            <h1 className="mt-2 text-3xl font-bold tracking-tight text-white md:text-4xl">
              Panel de Control General
            </h1>
            <p className="mt-2 max-w-2xl text-slate-300">
              Resumen operativo de MedAgenda para supervision clinica, usuarios y servicios.
            </p>
          </div>

          <div className="flex w-full flex-col gap-2 rounded-2xl border border-white/10 bg-white/5 p-4 backdrop-blur-sm md:w-auto md:min-w-72">
            <span className="text-xs font-medium text-slate-300">Estado del sistema</span>
            <div className="flex items-center gap-2 text-sm font-semibold text-emerald-300">
              <BadgeCheck className="h-4 w-4" />
              Servicios principales operativos
            </div>
            <span className="text-xs text-slate-400">API, Base de Datos y SignalR en linea</span>
          </div>
        </div>
      </header>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat, i) => (
          <div
            key={i}
            className="group rounded-2xl border border-slate-800/80 bg-slate-900/70 p-6 shadow-sm backdrop-blur-sm transition-all hover:border-slate-700 hover:bg-slate-900"
          >
            <div className="flex justify-between items-start mb-4">
              <div className={`p-3 rounded-xl ${stat.bg}`}>
                <stat.icon className={`w-6 h-6 ${stat.color}`} />
              </div>
              <span className="rounded-full bg-emerald-500/10 px-2 py-1 text-xs font-medium text-emerald-300 flex items-center gap-1">
                {stat.trend} <ArrowUpRight className="w-3 h-3" />
              </span>
            </div>
            <p className="text-sm font-medium text-slate-400">{stat.title}</p>
            <p className="mt-1 text-3xl font-bold text-white">{loading ? "--" : stat.value}</p>
          </div>
        ))}
      </div>

      <div className="grid gap-6 lg:grid-cols-5">
        <div className="space-y-4 lg:col-span-3">
          <h3 className="text-lg font-semibold text-white">Accesos Frecuentes</h3>
          <div className="grid gap-4 sm:grid-cols-2">
            <Link
              href="/admin/usuarios"
              className="group rounded-2xl border border-slate-800 bg-slate-900/60 p-5 transition-colors hover:border-indigo-500/40 hover:bg-slate-900"
            >
              <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-xl bg-indigo-500/15 text-indigo-300">
                <UserCog className="h-5 w-5" />
              </div>
              <p className="font-semibold text-white">Gestion de usuarios</p>
              <p className="mt-1 text-sm text-slate-400">Pacientes, administradores y roles</p>
              <div className="mt-4 flex items-center gap-1 text-sm font-medium text-indigo-300">
                Abrir modulo <ChevronRight className="h-4 w-4" />
              </div>
            </Link>

            <Link
              href="/admin/auditoria"
              className="group rounded-2xl border border-slate-800 bg-slate-900/60 p-5 transition-colors hover:border-rose-500/40 hover:bg-slate-900"
            >
              <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-xl bg-rose-500/15 text-rose-300">
                <ShieldAlert className="h-5 w-5" />
              </div>
              <p className="font-semibold text-white">Auditoria y seguridad</p>
              <p className="mt-1 text-sm text-slate-400">Trazabilidad completa del sistema</p>
              <div className="mt-4 flex items-center gap-1 text-sm font-medium text-rose-300">
                Revisar eventos <ChevronRight className="h-4 w-4" />
              </div>
            </Link>

            <Link
              href="/admin/citas"
              className="group rounded-2xl border border-slate-800 bg-slate-900/60 p-5 transition-colors hover:border-emerald-500/40 hover:bg-slate-900"
            >
              <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-xl bg-emerald-500/15 text-emerald-300">
                <CalendarDays className="h-5 w-5" />
              </div>
              <p className="font-semibold text-white">Agenda global</p>
              <p className="mt-1 text-sm text-slate-400">Control de citas, estados y turnos</p>
              <div className="mt-4 flex items-center gap-1 text-sm font-medium text-emerald-300">
                Ver calendario <ChevronRight className="h-4 w-4" />
              </div>
            </Link>

            <Link
              href="/admin/medicos"
              className="group rounded-2xl border border-slate-800 bg-slate-900/60 p-5 transition-colors hover:border-cyan-500/40 hover:bg-slate-900"
            >
              <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-xl bg-cyan-500/15 text-cyan-300">
                <Stethoscope className="h-5 w-5" />
              </div>
              <p className="font-semibold text-white">Directorio medico</p>
              <p className="mt-1 text-sm text-slate-400">Gestion de medicos y especialidades</p>
              <div className="mt-4 flex items-center gap-1 text-sm font-medium text-cyan-300">
                Gestionar perfiles <ChevronRight className="h-4 w-4" />
              </div>
            </Link>
          </div>
        </div>

        <div className="lg:col-span-2 rounded-2xl border border-indigo-500/30 bg-gradient-to-br from-indigo-500/20 via-indigo-500/10 to-emerald-500/15 p-6 text-white shadow-md">
          <div className="mb-4 flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white/10">
              <Activity className="h-5 w-5 text-emerald-300" />
            </div>
            <h3 className="text-xl font-bold">Monitoreo Operativo</h3>
          </div>

          <p className="mb-5 text-sm text-indigo-100/90">
            Supervisa la salud general de la plataforma y detecta incidentes antes de que impacten la experiencia clinica.
          </p>

          <div className="space-y-3">
            <div className="flex items-center justify-between rounded-xl border border-white/10 bg-white/5 px-4 py-3">
              <span className="text-sm text-slate-100">Disponibilidad API</span>
              <span className="text-sm font-semibold text-emerald-300">99.98%</span>
            </div>
            <div className="flex items-center justify-between rounded-xl border border-white/10 bg-white/5 px-4 py-3">
              <span className="text-sm text-slate-100">Latencia promedio</span>
              <span className="text-sm font-semibold text-emerald-300">142 ms</span>
            </div>
            <div className="flex items-center justify-between rounded-xl border border-white/10 bg-white/5 px-4 py-3">
              <span className="text-sm text-slate-100">Alertas criticas hoy</span>
              <span className="text-sm font-semibold text-indigo-200">0</span>
            </div>
          </div>

          <div className="mt-5 w-fit rounded-lg bg-white/15 px-4 py-2 text-sm font-medium backdrop-blur-sm flex items-center gap-2">
            <div className="h-2 w-2 rounded-full bg-emerald-400 animate-pulse" />
            Estado: Optimo
          </div>
        </div>
      </div>
    </div>
  );
}
