"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { Loader2, Mail, Lock, ShieldPlus } from "lucide-react";
import { AuthService } from "@/services/auth.service";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

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
      setError("Credenciales inválidas. Por favor, intenta de nuevo.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="relative min-h-screen flex flex-col items-center justify-center bg-[var(--med-navy)] text-slate-200 overflow-hidden px-4 sm:px-6 selection:bg-[var(--med-emerald)] selection:text-white font-sans">
      {/* Background Ambient Glow */}
      <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[800px] h-[600px] bg-[var(--med-emerald-glow)] blur-[120px] rounded-[100%] pointer-events-none opacity-60" />

      {/* Main Content Wrapper */}
      <div className="relative z-10 w-full max-w-[420px] flex flex-col items-center animate-fade-in-up">
        {/* Logo Section */}
        <div className="mb-8 flex flex-col items-center">
          <div className="relative mb-5">
            <div className="absolute inset-0 scale-150 rounded-2xl blur-xl opacity-40 bg-gradient-to-br from-[var(--med-emerald)] to-[var(--med-emerald-hover)]" />
            <div className="relative flex h-14 w-14 items-center justify-center rounded-[var(--radius-lg)] bg-gradient-to-br from-[var(--med-emerald)] to-[var(--med-emerald-hover)] shadow-lg shadow-[var(--med-emerald-glow)]">
              <ShieldPlus className="h-7 w-7 text-white" strokeWidth={2} />
            </div>
          </div>
          <h1 className="text-3xl sm:text-4xl font-bold tracking-tight text-white">
            MedAgenda
          </h1>
          <p className="mt-2 text-sm text-slate-400 font-medium">
            Portal de Gestión Clínica
          </p>
        </div>

        {/* Glass-lite Auth Card */}
        <div className="w-full bg-[var(--glass-bg)] border border-[var(--glass-border)] rounded-[var(--radius-lg)] p-8 shadow-[var(--shadow-premium)] backdrop-blur-xl">
          <h2 className="mb-6 text-xl font-semibold text-white tracking-tight">
            Iniciar Sesión
          </h2>

          <form onSubmit={handleLogin} className="space-y-5" noValidate>
            {/* Email Input */}
            <div className="space-y-1.5">
              <label
                htmlFor="email"
                className="block text-sm font-medium text-slate-300"
              >
                Correo Electrónico
              </label>
              <div className="relative group">
                <Mail className="absolute left-4 top-1/2 h-5 w-5 -translate-y-1/2 text-slate-500 transition-colors duration-200 group-focus-within:text-[var(--med-emerald)]" />
                <input
                  id="email"
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="dr.smith@ejemplo.com"
                  className="w-full bg-[var(--med-navy-surface)] border border-slate-700/50 rounded-[var(--radius-md)] py-2.5 pl-11 pr-4 text-white placeholder-slate-500 transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-[var(--med-emerald)]/40 focus:border-[var(--med-emerald)] hover:border-slate-600 motion-reduce:transition-none"
                  required
                />
              </div>
            </div>

            {/* Password Input */}
            <div className="space-y-1.5">
              <div className="flex items-center justify-between">
                <label
                  htmlFor="password"
                  className="block text-sm font-medium text-slate-300"
                >
                  Contraseña
                </label>
                <button
                  type="button"
                  className="text-sm font-medium text-[var(--med-emerald)] hover:text-white transition-colors duration-200"
                >
                  ¿Olvidaste tu contraseña?
                </button>
              </div>
              <div className="relative group">
                <Lock className="absolute left-4 top-1/2 h-5 w-5 -translate-y-1/2 text-slate-500 transition-colors duration-200 group-focus-within:text-[var(--med-emerald)]" />
                <input
                  id="password"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  placeholder="••••••••"
                  className="w-full bg-[var(--med-navy-surface)] border border-slate-700/50 rounded-[var(--radius-md)] py-2.5 pl-11 pr-4 text-white placeholder-slate-500 transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-[var(--med-emerald)]/40 focus:border-[var(--med-emerald)] hover:border-slate-600 motion-reduce:transition-none"
                  required
                />
              </div>
            </div>

            {/* Error Message */}
            {error && (
              <div className="animate-fade-in-up rounded-[var(--radius-md)] border border-red-500/20 bg-red-500/10 p-3 text-sm text-red-400">
                {error}
              </div>
            )}

            {/* Submit Button */}
            <button
              type="submit"
              disabled={isLoading}
              className="w-full flex items-center justify-center gap-2 bg-[var(--med-emerald)] hover:bg-[var(--med-emerald-hover)] text-white font-medium rounded-[var(--radius-md)] px-4 py-2.5 mt-2 transition-all duration-200 active:scale-[0.98] disabled:active:scale-100 disabled:opacity-70 disabled:cursor-not-allowed focus:outline-none focus:ring-2 focus:ring-[var(--med-emerald)] focus:ring-offset-2 focus:ring-offset-[var(--med-navy)] shadow-md shadow-[var(--med-emerald-glow)] motion-reduce:transition-none"
            >
              {isLoading ? (
                <>
                  <Loader2 className="h-5 w-5 animate-spin" />
                  <span>Ingresando...</span>
                </>
              ) : (
                "Ingresar a mi cuenta"
              )}
            </button>
          </form>

          {/* Register Link */}
          <div className="mt-8 text-center text-sm text-slate-400">
            ¿No tienes una cuenta?{" "}
            <Link
              href="/registro"
              className="font-medium text-white hover:text-[var(--med-emerald)] transition-colors duration-200"
            >
              Registrarme
            </Link>
          </div>
        </div>

        {/* Footer Copyright */}
        <div
          className="mt-8 text-xs text-slate-500 text-center animate-fade-in-up"
          style={{ animationDelay: "150ms" }}
        >
          &copy; {new Date().getFullYear()} MedAgenda. Todos los derechos
          reservados. <br className="sm:hidden" />
          Seguro y en cumplimiento HIPAA.
        </div>
      </div>
    </div>
  );
}
