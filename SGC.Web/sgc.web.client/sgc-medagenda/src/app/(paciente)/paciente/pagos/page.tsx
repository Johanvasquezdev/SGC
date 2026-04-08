"use client";

import { useEffect, useState } from "react";
import { CreditCard, Search, ArrowDownCircle } from "lucide-react";
import dayjs from "dayjs";
import { toast } from "sonner";
import { PagoResponse } from "@/types/api.types";
import { PagoService } from "@/services/pago.service";
import { useAuth } from "@/components/providers/AuthProvider";

export default function AdminPagosPage() {
  const { user } = useAuth();
  const [pagos, setPagos] = useState<PagoResponse[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPagos = async () => {
      if (!user?.id) return;
      try {
        const data = await PagoService.obtenerPorPaciente(user.id);
        setPagos(data);
      } catch (e) {
        setPagos([]);
      } finally {
        setLoading(false);
      }
    };
    fetchPagos();
  }, [user?.id]);

  // Simula el reembolso en UI hasta implementar el endpoint real.
  const handleReembolso = (id: number) => {
    if (confirm("¿Estas seguro de procesar este reembolso?")) {
      setPagos(pagos.map(p => p.id === id ? { ...p, estado: "Reembolsado" } : p));
      toast.success("Reembolso procesado exitosamente");
    }
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold flex items-center gap-2 text-slate-900 dark:text-white">
            <CreditCard className="w-7 h-7 text-emerald-500" />
            Transacciones Financieras
          </h1>
          <p className="text-slate-500 mt-1">Supervisa los pagos y procesa reembolsos.</p>
        </div>
      </header>

      <div className="bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800 overflow-hidden shadow-sm">
        <div className="p-4 border-b border-slate-200 dark:border-slate-800 bg-slate-50 dark:bg-slate-900/50">
          <div className="relative max-w-sm">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
            <input 
              type="text" placeholder="Buscar por ID..."
              className="w-full pl-9 pr-4 py-2 rounded-lg border border-slate-200 focus:ring-2 focus:ring-emerald-500 outline-none text-sm"
            />
          </div>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="bg-slate-50 dark:bg-slate-800/80 border-b border-slate-200 text-xs uppercase text-slate-500">
              <tr>
                <th className="p-4 font-medium">ID Transaccion</th>
                <th className="p-4 font-medium">Fecha</th>
                <th className="p-4 font-medium text-right">Monto</th>
                <th className="p-4 font-medium text-center">Estado</th>
                <th className="p-4 font-medium text-center">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-200">
              {loading ? (
                <tr><td colSpan={5} className="p-4 text-center text-slate-500">Cargando...</td></tr>
              ) : pagos.map(pago => (
                <tr key={pago.id} className="hover:bg-slate-50 transition-colors">
                  <td className="p-4 font-mono text-slate-500">#{pago.id}</td>
                  <td className="p-4 text-slate-500">{dayjs(pago.fechaCreacion).format("DD MMM YYYY")}</td>
                  <td className="p-4 text-right font-semibold">{pago.moneda} {pago.monto.toLocaleString()}</td>
                  <td className="p-4 text-center">
                    <span className={`px-2 py-1 rounded-full text-xs font-medium ${
                      pago.estado === "Completado" ? "bg-emerald-100 text-emerald-700" : "bg-rose-100 text-rose-700"
                    }`}>
                      {pago.estado}
                    </span>
                  </td>
                  <td className="p-4 text-center">
                    {pago.estado === "Completado" && (
                      <button 
                        onClick={() => handleReembolso(pago.id)}
                        className="text-xs flex items-center gap-1 mx-auto text-rose-600 hover:text-rose-700 font-medium"
                      >
                        <ArrowDownCircle className="w-4 h-4" /> Reembolsar
                      </button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
