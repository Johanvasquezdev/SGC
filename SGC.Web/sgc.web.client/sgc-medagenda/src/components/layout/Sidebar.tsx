"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import {
  LayoutDashboard,
  Users,
  Calendar,
  CreditCard,
  Bell,
  MessageSquare,
  Stethoscope,
  ShieldAlert,
  LogOut,
  Building2,
  Settings,
} from "lucide-react";
import { AuthService } from "@/services/auth.service";

export const Sidebar = () => {
  const pathname = usePathname();

  // Detectamos si estamos en la zona de administrador o de paciente
  const isAdmin = pathname.startsWith("/admin");

  // Rutas para el Paciente
  const pacienteRoutes = [
    {
      label: "Dashboard",
      icon: LayoutDashboard,
      href: "/paciente/dashboard",
      color: "text-emerald-500",
    },
    {
      label: "Directorio Médico",
      icon: Stethoscope,
      href: "/paciente/medicos",
      color: "text-teal-500",
    },
    {
      label: "Mis Citas",
      icon: Calendar,
      href: "/paciente/citas",
      color: "text-blue-500",
    },
    {
      label: "Pagos y Facturación",
      icon: CreditCard,
      href: "/paciente/pagos",
      color: "text-emerald-500",
    },
    {
      label: "Notificaciones",
      icon: Bell,
      href: "/paciente/notificaciones",
      color: "text-amber-500",
    },
    {
      label: "Asistente IA",
      icon: MessageSquare,
      href: "/paciente/chatbot",
      color: "text-purple-500",
    },
    {
      label: "Configuración",
      icon: Settings,
      href: "/paciente/settings",
      color: "text-slate-400",
    },
  ];

  // Rutas para el Administrador
  const adminRoutes = [
    {
      label: "Panel General",
      icon: LayoutDashboard,
      href: "/admin/dashboard",
      color: "text-indigo-500",
    },
    {
      label: "Citas",
      icon: Calendar,
      href: "/admin/citas",
      color: "text-emerald-500",
    },
    {
      label: "Usuarios",
      icon: Users,
      href: "/admin/usuarios",
      color: "text-blue-500",
    },
    {
      label: "Médicos",
      icon: Stethoscope,
      href: "/admin/medicos",
      color: "text-teal-500",
    },
    {
      label: "Disponibilidad",
      icon: Calendar,
      href: "/admin/disponibilidad",
      color: "text-emerald-500",
    },
    {
      label: "Especialidades",
      icon: Stethoscope,
      href: "/admin/especialidades",
      color: "text-emerald-500",
    },
    {
      label: "Proveedores (ARS)",
      icon: Building2,
      href: "/admin/proveedores",
      color: "text-emerald-500",
    },
    {
      label: "Transacciones",
      icon: CreditCard,
      href: "/admin/pagos",
      color: "text-amber-500",
    },
    {
      label: "Auditoría",
      icon: ShieldAlert,
      href: "/admin/auditoria",
      color: "text-rose-500",
    },
    {
      label: "Configuración",
      icon: Settings,
      href: "/admin/settings",
      color: "text-slate-400",
    },
  ];

  const routes = isAdmin ? adminRoutes : pacienteRoutes;

  const handleLogout = () => {
    AuthService.logout();
  };

  const accentClasses = isAdmin
    ? {
        soft: "bg-indigo-500/15 text-indigo-200",
        marker: "bg-indigo-400",
        logo: "bg-indigo-500",
        logoDot: "text-indigo-300",
        hover: "hover:bg-white/10",
      }
    : {
        soft: "bg-emerald-500/15 text-emerald-200",
        marker: "bg-emerald-400",
        logo: "bg-emerald-500",
        logoDot: "text-emerald-300",
        hover: "hover:bg-white/10",
      };

  return (
    <div className="flex h-full flex-col border-r border-white/10 bg-black/35 text-white backdrop-blur-xl transition-all">
      {/* Logo */}
      <div className="flex h-16 items-center border-b border-white/10 px-6">
        <Link
          href={isAdmin ? "/admin/dashboard" : "/paciente/dashboard"}
          className="flex items-center gap-2"
        >
          <div
            className={`flex h-9 w-9 items-center justify-center rounded-xl text-base font-bold text-white ${accentClasses.logo}`}
          >
            M
          </div>
          <h1 className="text-xl font-bold tracking-tight text-white">
            MedAgenda
            <span className={accentClasses.logoDot}>
              .
            </span>
          </h1>
        </Link>
      </div>

      {/* Menú de Navegación */}
      <div className="flex-1 space-y-1.5 overflow-y-auto px-4 py-6">
        <p className="mb-2 px-4 text-xs font-semibold uppercase tracking-wider text-white/40">
          {isAdmin ? "Administración" : "Menú Principal"}
        </p>

        {routes.map((route) => {
          const isActive = pathname === route.href;

          return (
            <Link
              key={route.href}
              href={route.href}
              className={`group relative flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium transition-all duration-200 ${
                isActive
                  ? `${accentClasses.soft}`
                  : `text-white/65 ${accentClasses.hover} hover:text-white`
              }`}
            >
              {isActive && (
                <span
                  className={`absolute left-0 top-1/2 h-6 w-1 -translate-y-1/2 rounded-r-full ${accentClasses.marker}`}
                />
              )}
              <route.icon
                className={`h-5 w-5 ${isActive ? "text-current" : "text-white/55 group-hover:text-white"}`}
              />
              {route.label}
            </Link>
          );
        })}
      </div>

      {/* Botón de Cerrar Sesión (Fijo abajo) */}
      <div className="border-t border-white/10 p-4">
        <button
          onClick={handleLogout}
          className="group flex w-full items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium text-white/65 transition-all duration-200 hover:bg-rose-500/10 hover:text-rose-300"
        >
          <LogOut className="h-5 w-5 text-white/55 group-hover:text-rose-300" />
          Cerrar Sesión
        </button>
      </div>
    </div>
  );
};

