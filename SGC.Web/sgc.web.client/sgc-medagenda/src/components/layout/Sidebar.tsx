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

  return (
    <div className="flex flex-col h-full bg-card border-r border-border transition-all">
      {/* Logo */}
      <div className="h-16 flex items-center px-6 border-b border-border">
        <Link
          href={isAdmin ? "/admin/dashboard" : "/paciente/dashboard"}
          className="flex items-center gap-2"
        >
          <div
            className={`w-8 h-8 rounded-lg flex items-center justify-center text-white font-bold ${isAdmin ? "bg-indigo-600" : "bg-emerald-600"}`}
          >
            M
          </div>
          <h1 className="text-xl font-bold text-foreground tracking-tight">
            MedAgenda
            <span className={isAdmin ? "text-indigo-500" : "text-emerald-500"}>
              .
            </span>
          </h1>
        </Link>
      </div>

      {/* Menú de Navegación */}
      <div className="flex-1 overflow-y-auto py-6 px-4 space-y-1.5">
        <p className="px-4 text-xs font-semibold text-muted-foreground uppercase tracking-wider mb-2">
          {isAdmin ? "Administración" : "Menú Principal"}
        </p>

        {routes.map((route) => {
          const isActive = pathname === route.href;

          return (
            <Link
              key={route.href}
              href={route.href}
              className={`flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 group font-medium text-sm ${
                isActive
                  ? `${isAdmin ? "bg-indigo-500/10 text-indigo-600 dark:text-indigo-300" : "bg-emerald-500/10 text-emerald-600 dark:text-emerald-300"}`
                  : "text-muted-foreground hover:bg-accent"
              }`}
            >
              <route.icon
                className={`w-5 h-5 ${isActive ? route.color : "text-muted-foreground group-hover:text-foreground"}`}
              />
              {route.label}
            </Link>
          );
        })}
      </div>

      {/* Botón de Cerrar Sesión (Fijo abajo) */}
      <div className="p-4 border-t border-border">
        <button
          onClick={handleLogout}
          className="flex items-center gap-3 px-4 py-3 w-full rounded-xl transition-all duration-200 text-muted-foreground hover:bg-rose-500/10 hover:text-rose-500 font-medium text-sm group"
        >
          <LogOut className="w-5 h-5 text-muted-foreground group-hover:text-rose-500" />
          Cerrar Sesión
        </button>
      </div>
    </div>
  );
};

