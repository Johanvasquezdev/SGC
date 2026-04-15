"use client";
import { Bell, Search } from "lucide-react";
import { Sidebar } from "@/components/layout/Sidebar";
import { useAuth } from "@/components/providers/AuthProvider";
import { ThemeToggle } from "@/components/theme-toggle";
import { PageTransition } from "@/components/layout/PageTransition";
import Link from "next/link";

export default function PacienteLayout({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();
  const initials = user?.nombre?.trim().charAt(0).toUpperCase() || "P";

  return (
    <div className="relative min-h-screen overflow-hidden bg-[#0a0f1e] text-white">
      <div className="pointer-events-none fixed inset-0 overflow-hidden">
        <div
          className="absolute -right-56 -top-56 h-[640px] w-[640px] rounded-full opacity-[0.08]"
          style={{ background: "radial-gradient(circle, #10b981 0%, transparent 70%)" }}
        />
        <div
          className="absolute -bottom-56 -left-56 h-[640px] w-[640px] rounded-full opacity-[0.08]"
          style={{ background: "radial-gradient(circle, #34d399 0%, transparent 70%)" }}
        />
        <div
          className="absolute right-1/4 top-1/3 h-[420px] w-[420px] rounded-full opacity-[0.05]"
          style={{ background: "radial-gradient(circle, #22c55e 0%, transparent 70%)" }}
        />
      </div>

      <div className="relative z-10 flex min-h-screen">
        <aside className="hidden w-72 shrink-0 lg:block">
          <div className="sticky top-0 h-screen">
            <Sidebar />
          </div>
        </aside>

        <div className="flex min-h-screen flex-1 flex-col">
          <header className="sticky top-0 z-30 border-b border-white/10 bg-black/20 backdrop-blur-xl">
            <div className="flex items-center gap-4 px-4 py-4 md:px-6">
              <div className="relative flex-1 max-w-2xl">
                <Search className="absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-white/40" />
                <input
                  placeholder="Buscar medicos, citas..."
                  className="w-full rounded-xl border border-white/10 bg-white/5 py-2.5 pl-11 pr-4 text-sm text-white placeholder:text-white/40 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-transparent"
                />
              </div>

              <ThemeToggle />

              <Link
                href="/paciente/notificaciones"
                className="relative rounded-xl border border-white/10 bg-white/5 p-2 text-white/70 transition-all hover:bg-white/10 hover:text-white"
                aria-label="Abrir notificaciones"
              >
                <Bell className="h-5 w-5" />
                <span className="absolute -right-1 -top-1 flex h-5 w-5 items-center justify-center rounded-full bg-emerald-500 text-[10px] font-bold text-white">
                  3
                </span>
              </Link>

              <Link
                href="/paciente/settings"
                aria-label="Abrir configuración de usuario"
                className="flex h-10 w-10 items-center justify-center rounded-full bg-emerald-500 text-sm font-semibold text-white transition-all hover:bg-emerald-600"
              >
                {initials}
              </Link>
            </div>
          </header>

          <main className="flex-1 px-4 py-6 md:px-6 md:py-8">
            <PageTransition>{children}</PageTransition>
          </main>
        </div>
      </div>
    </div>
  );
}
