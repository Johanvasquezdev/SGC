import { ShieldPlus } from "lucide-react";

export default function AuthLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50 dark:bg-slate-950 p-4">
      <div className="mb-8 flex flex-col items-center">
        <div className="w-16 h-16 bg-emerald-600 rounded-2xl flex items-center justify-center shadow-lg shadow-emerald-500/30 mb-4">
          <ShieldPlus className="w-8 h-8 text-white" />
        </div>
        <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight">MedAgenda</h1>
        <p className="text-slate-500 dark:text-slate-400 mt-1">Portal de Gestion Clinica</p>
      </div>
      {children}
    </div>
  );
}
