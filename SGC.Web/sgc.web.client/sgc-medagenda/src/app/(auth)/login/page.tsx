"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { Loader2, Mail, Lock } from "lucide-react";
import { AuthService } from "@/services/auth.service";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true); setError("");

    try {
      await AuthService.login({ email, password });
      const raw = localStorage.getItem("medagenda_user");
      const usuario = raw ? JSON.parse(raw) : null;

      if (usuario?.rol === "Administrador") {
        router.push("/admin/dashboard");
      } else {
        router.push("/paciente/dashboard");
      }
    } catch (err: any) {
      setError("Credenciales invalidas. Por favor, intenta de nuevo.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="w-full max-w-md bg-white dark:bg-slate-900 p-8 rounded-3xl shadow-xl border border-slate-100 dark:border-slate-800 animate-in slide-in-from-bottom-4 duration-500">
      <h2 className="text-2xl font-bold text-slate-900 dark:text-white mb-6">Iniciar Sesion</h2>
      
      <form onSubmit={handleLogin} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1">Correo Electronico</label>
          <div className="relative">
            <Mail className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-400" />
            <input 
              type="email" required 
              className="w-full pl-10 pr-4 py-3 rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 focus:ring-2 focus:ring-emerald-500 outline-none"
              value={email} onChange={(e) => setEmail(e.target.value)}
            />
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1">Contrasena</label>
          <div className="relative">
            <Lock className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-400" />
            <input 
              type="password" required 
              className="w-full pl-10 pr-4 py-3 rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 focus:ring-2 focus:ring-emerald-500 outline-none"
              value={password} onChange={(e) => setPassword(e.target.value)}
            />
          </div>
        </div>

        {error && <div className="text-red-500 text-sm bg-red-50 dark:bg-red-900/20 p-3 rounded-lg">{error}</div>}

        <button disabled={isLoading} className="w-full mt-6 bg-emerald-600 hover:bg-emerald-700 text-white font-medium py-3 rounded-xl transition-all shadow-md shadow-emerald-500/20 flex justify-center">
          {isLoading ? <Loader2 className="animate-spin w-5 h-5" /> : "Ingresar a mi cuenta"}
        </button>
      </form>

      <div className="mt-4 text-center">
        <a href="/registro" className="text-emerald-600 font-medium hover:underline">
          Registrarme
        </a>
      </div>
    </div>
  );
}
