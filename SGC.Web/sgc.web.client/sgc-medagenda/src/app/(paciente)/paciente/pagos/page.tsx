"use client";

import { useEffect, useState } from "react";
import { CreditCard, Search, Eye, Download, ArrowDownCircle } from "lucide-react";
import dayjs from "dayjs";
import { toast } from "sonner";
import Link from "next/link";
import { PagoResponse } from "@/types/api.types";
import { PagoService } from "@/services/pago.service";
import { useAuth } from "@/components/providers/AuthProvider";
import { usePageTransition } from "@/components/animations/Animatedcomponents";

const filters = ["Todos", "Pagados", "Pendientes"];

export default function PagosPage() {
  const { user } = useAuth();
  const [pagos, setPagos] = useState<PagoResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState("");
  const [activeFilter, setActiveFilter] = useState("Todos");

  useEffect(() => {
    const fetchPagos = async () => {
      if (!user?.id) { setPagos([]); setLoading(false); return; }
      try {
        const data = await PagoService.obtenerPorPaciente(user.id);
        setPagos(data);
      } catch {
        setPagos([]);
      } finally {
        setLoading(false);
      }
    };
    fetchPagos();
  }, [user?.id]);

  const handleReembolso = (id: number) => {
    if (!confirm("¿Estás seguro de procesar este reembolso?")) return;
    setPagos(pagos.map(p => p.id === id ? { ...p, estado: "Reembolsado" } : p));
    toast.success("Reembolso procesado exitosamente");
  };

  const filtered = pagos.filter(p => {
    const matchesSearch = `#${p.id}`.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesFilter =
      activeFilter === "Todos" ||
      (activeFilter === "Pagados" && p.estado === "Completado") ||
      (activeFilter === "Pendientes" && p.estado !== "Completado");
    return matchesSearch && matchesFilter;
  });

  usePageTransition();

  return (
    <div className="space-y-8 page-content animate-in fade-in duration-500">
      <header className="relative overflow-hidden rounded-3xl border border-emerald-500/20 bg-gradient-to-br from-emerald-500/15 via-white dark:via-slate-950 to-teal-500/15 p-6 md:p-7 shadow-sm">
        <div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-emerald-500/10 dark:bg-emerald-500/20 blur-3xl opacity-50" />
        <div className="relative z-10">
          <h1 className="text-3xl font-black tracking-tight text-foreground flex items-center gap-2">
            <CreditCard className="w-8 h-8 text-emerald-600 dark:text-emerald-400" />
            Transacciones Financieras
          </h1>
          <p className="text-muted-foreground font-medium mt-1">Supervisa tus pagos y procesa posibles reembolsos.</p>
        </div>
      </header>

      <div className="bg-card/50 backdrop-blur-md border border-border rounded-2xl p-4 mb-6 shadow-sm">
        <div className="flex flex-col md:flex-row items-center gap-4">
          <div className="relative flex-1 w-full">
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-muted-foreground" />
            <input
              type="text"
              value={searchQuery}
              onChange={e => setSearchQuery(e.target.value)}
              placeholder="Buscar por ID..."
              className="w-full bg-background border border-border rounded-xl py-3 pl-12 pr-4 text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 transition-all shadow-sm"
            />
          </div>
          <div className="flex gap-2 w-full md:w-auto">
            {filters.map(filter => (
              <button
                key={filter}
                onClick={() => setActiveFilter(filter)}
                className={`flex-1 md:flex-none px-5 py-2.5 rounded-xl text-sm font-bold transition-all shadow-sm ${
                  activeFilter === filter
                    ? "bg-emerald-600 text-white shadow-emerald-500/20"
                    : "bg-muted/50 text-muted-foreground hover:bg-muted hover:text-foreground border border-border"
                }`}
              >
                {filter}
              </button>
            ))}
          </div>
        </div>
      </div>

      <div className="bg-card border border-border rounded-2xl overflow-hidden shadow-sm">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead>
              <tr className="border-b border-border bg-muted/30">
                <th className="text-left text-muted-foreground text-xs font-black uppercase tracking-widest px-6 py-4">ID Transacción</th>
                <th className="text-left text-muted-foreground text-xs font-black uppercase tracking-widest px-6 py-4">Fecha</th>
                <th className="text-left text-muted-foreground text-xs font-black uppercase tracking-widest px-6 py-4">Monto</th>
                <th className="text-left text-muted-foreground text-xs font-black uppercase tracking-widest px-6 py-4">Estado</th>
                <th className="text-right text-muted-foreground text-xs font-black uppercase tracking-widest px-6 py-4">Acciones</th>
              </tr>
            </thead>
            <tbody>
              {loading ? (
                <tr>
                  <td colSpan={5} className="p-12 text-center text-muted-foreground font-medium animate-pulse">Cargando...</td>
                </tr>
              ) : filtered.length === 0 ? (
                <tr>
                  <td colSpan={5} className="p-12 text-center">
                    <CreditCard className="w-12 h-12 text-muted-foreground/20 mx-auto mb-4" />
                    <p className="text-muted-foreground font-bold">No se encontraron transacciones.</p>
                  </td>
                </tr>
              ) : (
                filtered.map(pago => (
                  <tr key={pago.id} className="border-b border-border hover:bg-muted/30 transition-colors group">
                    <td className="px-6 py-4 text-foreground font-mono text-xs font-bold">#{pago.id}</td>
                    <td className="px-6 py-4 text-muted-foreground font-medium text-sm">{dayjs(pago.fechaCreacion).format("DD MMM YYYY")}</td>
                    <td className="px-6 py-4 text-foreground font-black">{pago.moneda} {pago.monto.toLocaleString()}</td>
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-2">
                        <span className={`px-4 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest shadow-sm ${
                          pago.estado === "Completado"
                            ? "bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 border border-emerald-500/20"
                            : "bg-amber-500/10 text-amber-600 dark:text-amber-400 border border-amber-500/20"
                        }`}>
                          {pago.estado}
                        </span>
                        {pago.estado === "Pendiente" && (
                          <Link
                            href={`/paciente/checkout/${pago.citaId}`}
                            className="px-4 py-1.5 ml-2 bg-emerald-600 text-white hover:bg-emerald-500 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all shadow-lg shadow-emerald-500/20 active:scale-95"
                          >
                            Completar
                          </Link>
                        )}
                      </div>
                    </td>
                    <td className="px-6 py-4 text-right">
                      <div className="flex items-center justify-end gap-2">
                        <button className="w-9 h-9 rounded-xl bg-muted/50 hover:bg-muted flex items-center justify-center text-muted-foreground hover:text-foreground transition-all border border-border">
                          <Eye className="w-4 h-4" />
                        </button>
                        <button className="w-9 h-9 rounded-xl bg-muted/50 hover:bg-muted flex items-center justify-center text-muted-foreground hover:text-foreground transition-all border border-border">
                          <Download className="w-4 h-4" />
                        </button>
                        {pago.estado === "Completado" && (
                          <button
                            onClick={() => handleReembolso(pago.id)}
                            className="w-9 h-9 rounded-xl bg-rose-500/10 hover:bg-rose-500/20 flex items-center justify-center text-rose-600 dark:text-rose-400 transition-all border border-rose-500/20"
                            title="Reembolsar"
                          >
                            <ArrowDownCircle className="w-4 h-4" />
                          </button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}