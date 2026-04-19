"use client";

import Link from "next/link";
import { Bell, Search } from "lucide-react";
import { Sidebar } from "@/components/layout/Sidebar";
import { useAuth } from "@/components/providers/AuthProvider";
import { ThemeToggle } from "@/components/theme-toggle";
import { PageTransition } from "@/components/layout/PageTransition";

export default function PacienteLayout({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();
  const initials = user?.nombre?.trim().charAt(0).toUpperCase() || "P";

  return (
    <div className="relative min-h-screen overflow-hidden bg-background text-foreground transition-colors duration-300">
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
          <header className="sticky top-0 z-30 border-b border-border bg-background/50 backdrop-blur-xl">
            <div className="px-4 py-4 md:px-6">
              <div className="mx-auto flex w-full max-w-4xl items-center justify-center gap-4">
                <div className="relative min-w-0 max-w-2xl flex-1">
                  <Search className="absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                  <input
                    placeholder="Buscar médicos, citas..."
                    className="w-full rounded-xl border border-border bg-secondary py-2.5 pl-11 pr-4 text-sm text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-transparent"
                  />
                </div>

                <div className="flex shrink-0 items-center gap-3">
                  <ThemeToggle />

                  <Link
                    href="/paciente/notificaciones"
                    className="relative rounded-xl border border-border bg-secondary p-2 text-muted-foreground transition-all hover:bg-secondary/80 hover:text-foreground"
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
              </div>
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
