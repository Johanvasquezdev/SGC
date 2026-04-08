"use client";

import { useEffect, useState } from "react";
import { Bell, CalendarClock, CreditCard, Info, CheckCircle2 } from "lucide-react";
import dayjs from "dayjs";
import { NotificacionResponse } from "@/types/api.types";
import { NotificacionService } from "@/services/notificacion.service";
import { useAuth } from "@/components/providers/AuthProvider";

export default function NotificacionesPage() {
  const { user } = useAuth();
  const [notificaciones, setNotificaciones] = useState<NotificacionResponse[]>([]);
  const [loading, setLoading] = useState(true);

  const marcarComoLeida = (id: number) => {
    setNotificaciones(notificaciones.map(n => n.id === id ? { ...n, leida: true } : n));
  };

  const marcarTodasLeidas = () => {
    setNotificaciones(notificaciones.map(n => ({ ...n, leida: true })));
  };

  const getIcon = (tipo: string) => {
    switch(tipo?.toLowerCase()) {
      case 'email': return <CalendarClock className="w-5 h-5 text-emerald-600" />;
      case 'sms': return <CreditCard className="w-5 h-5 text-emerald-600" />;
      default: return <Info className="w-5 h-5 text-blue-600" />;
    }
  };

  useEffect(() => {
    const fetchNotificaciones = async () => {
      if (!user?.id) {
        setLoading(false);
        return;
      }
      try {
        const data = await NotificacionService.obtenerPorUsuario(user.id);
        setNotificaciones(data);
      } catch (e) {
        setNotificaciones([]);
      } finally {
        setLoading(false);
      }
    };
    fetchNotificaciones();
  }, [user?.id]);

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <header className="flex justify-between items-end">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-slate-900 dark:text-white flex items-center gap-2">
            <Bell className="w-8 h-8 text-emerald-500" /> Mis Notificaciones
          </h1>
          <p className="text-slate-500 mt-1">Mantente al tanto de tus citas y pagos.</p>
        </div>
        <button onClick={marcarTodasLeidas} className="text-sm text-emerald-600 font-medium flex items-center gap-1 hover:bg-emerald-50 px-3 py-1.5 rounded-lg transition-colors">
          <CheckCircle2 className="w-4 h-4" /> Marcar todas leidas
        </button>
      </header>

      <div className="space-y-4">
        {loading ? (
          <div className="text-center py-12 text-slate-400 bg-slate-900/60 rounded-xl border border-slate-800/80">
            Cargando notificaciones...
          </div>
        ) : notificaciones.length === 0 ? (
          <div className="text-center py-12 text-slate-400 bg-slate-900/60 rounded-xl border border-slate-800/80">
            No tienes notificaciones.
          </div>
        ) : (
          notificaciones.map((notificacion) => (
            <div 
              key={notificacion.id} 
              onClick={() => marcarComoLeida(notificacion.id)}
              className={`p-5 rounded-2xl border transition-all cursor-pointer flex gap-4 ${
                notificacion.leida 
                  ? 'bg-slate-900/60 border-slate-800/80' 
                  : 'bg-emerald-500/5 border-emerald-500/20 shadow-sm'
              }`}
            >
              <div className="p-3 rounded-xl h-fit bg-emerald-500/10">
                {getIcon(notificacion.tipo)}
              </div>
              <div className="flex-1">
                <div className="flex justify-between items-start">
                  <h3 className={`font-semibold ${notificacion.leida ? 'text-slate-300' : 'text-white'}`}>
                    {notificacion.tipo}
                  </h3>
                  <span className="text-xs text-slate-500">{dayjs(notificacion.fechaEnvio).format('DD MMM, HH:mm')}</span>
                </div>
                <p className={`text-sm mt-1 ${notificacion.leida ? 'text-slate-400' : 'text-slate-300'}`}>
                  {notificacion.mensaje}
                </p>
              </div>
              {!notificacion.leida && (
                <div className="w-2 h-2 rounded-full bg-emerald-500 self-center"></div>
              )}
            </div>
          ))
        )}
      </div>
    </div>
  );
}

