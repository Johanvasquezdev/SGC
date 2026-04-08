"use client";

import { useMemo } from "react";
import { Bell, Search } from "lucide-react";
import { Sidebar } from "@/components/layout/Sidebar";
import { useAuth } from "@/components/providers/AuthProvider";
import { ThemeToggle } from "@/components/theme-toggle";

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();

  const initials = useMemo(() => {
    if (!user?.nombre) return "A";
    return user.nombre.trim().charAt(0).toUpperCase();
  }, [user?.nombre]);

  return (
    <div className="min-h-screen bg-slate-950 text-white">
      <div className="flex">
        <aside className="hidden lg:block w-72 h-screen sticky top-0">
          <Sidebar />
        </aside>

        <div className="flex-1 min-h-screen">
          <header className="sticky top-0 z-30 border-b border-border bg-background/70 backdrop-blur">
            <div className="flex items-center gap-4 px-6 py-4">
              <div className="relative flex-1 max-w-2xl">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
                <input
                  placeholder="Buscar usuarios, médicos, citas..."
                  className="w-full pl-9 pr-4 py-2.5 rounded-xl bg-card border border-border text-sm text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-indigo-500"
                />
              </div>

              <ThemeToggle />

              <a
                href="/admin/auditoria"
                className="relative p-2 rounded-xl bg-card border border-border hover:bg-accent"
              >
                <Bell className="w-5 h-5 text-foreground" />
                <span className="absolute -right-1 -top-1 w-4 h-4 text-[10px] bg-indigo-500 text-slate-950 rounded-full flex items-center justify-center">
                  3
                </span>
              </a>

              <div className="w-9 h-9 rounded-full bg-indigo-500/20 border border-indigo-500/40 flex items-center justify-center text-indigo-300 font-semibold">
                {initials}
              </div>
            </div>
          </header>

          <main className="px-6 py-8">{children}</main>
        </div>
      </div>
    </div>
  );
}
