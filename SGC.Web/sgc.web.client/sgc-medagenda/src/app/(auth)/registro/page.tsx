"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import {
  Loader2, Mail, Lock, ShieldPlus, User, Phone, Calendar,
  CreditCard, FileText, Eye, EyeOff, CheckCircle2, ArrowRight
} from "lucide-react";
import { toast } from "sonner";
import { PacienteService } from "@/services/paciente.service";

export default function RegistroPage() {
  const router = useRouter();
  const [formData, setFormData] = useState({
    nombre: "", email: "", password: "", confirmPassword: "",
    cedula: "", telefono: "", fechaNacimiento: "", tipoSeguro: "", numeroSeguro: "",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [step, setStep] = useState(1);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const passwordStrength = () => {
    const { password } = formData;
    if (password.length === 0) return { strength: 0, label: "", color: "" };
    if (password.length < 6) return { strength: 25, label: "Débil", color: "bg-red-500" };
    if (password.length < 8) return { strength: 50, label: "Regular", color: "bg-yellow-500" };
    if (password.length < 12) return { strength: 75, label: "Buena", color: "bg-blue-500" };
    return { strength: 100, label: "Fuerte", color: "bg-emerald-500" };
  };

  const { strength, label, color } = passwordStrength();

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    if (formData.password !== formData.confirmPassword) {
      setError("Las contraseñas no coinciden.");
      setIsLoading(false);
      return;
    }

    try {
      await PacienteService.crear({
        nombre: formData.nombre,
        email: formData.email,
        password: formData.password,
        cedula: formData.cedula || undefined,
        telefono: formData.telefono || undefined,
        fechaNacimiento: formData.fechaNacimiento || undefined,
        tipoSeguro: formData.tipoSeguro || undefined,
        numeroSeguro: formData.numeroSeguro || undefined,
      });
      setIsSuccess(true);
      toast.success("Cuenta creada correctamente");
      setTimeout(() => router.push("/login"), 2000);
    } catch (err: any) {
      setError("Ocurrió un error al crear la cuenta. Por favor, intenta de nuevo.");
      toast.error("No se pudo crear la cuenta");
    } finally {
      setIsLoading(false);
    }
  };

  if (isSuccess) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center relative overflow-hidden bg-[#0a0f1e]">
        <div className="absolute inset-0 overflow-hidden">
          <div className="absolute -top-40 -right-40 w-[700px] h-[700px] rounded-full opacity-[0.12]"
            style={{ background: "radial-gradient(circle, #10b981 0%, transparent 70%)" }} />
          <div className="absolute -bottom-40 -left-40 w-[700px] h-[700px] rounded-full opacity-[0.12]"
            style={{ background: "radial-gradient(circle, #6366f1 0%, transparent 70%)" }} />
        </div>
        <div className="relative z-10 w-full max-w-md px-6">
          <div className="backdrop-blur-2xl bg-white/[0.03] border border-white/[0.08] rounded-3xl p-8 text-center text-white shadow-2xl shadow-black/20">
            <CheckCircle2 className="mx-auto mb-4 h-16 w-16 text-emerald-400" />
            <h2 className="text-2xl font-bold">¡Cuenta creada!</h2>
            <p className="mt-2 text-white/50">Serás redirigido al inicio de sesión.</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen w-full flex items-center justify-center relative overflow-hidden bg-[#0a0f1e] py-12 px-4">
      {/* Background blobs */}
      <div className="absolute inset-0 overflow-hidden">
        <div className="absolute -top-40 -right-40 w-[700px] h-[700px] rounded-full opacity-[0.12] animate-pulse"
          style={{ background: "radial-gradient(circle, #10b981 0%, transparent 70%)", animationDuration: "4s" }} />
        <div className="absolute -bottom-40 -left-40 w-[700px] h-[700px] rounded-full opacity-[0.12] animate-pulse"
          style={{ background: "radial-gradient(circle, #6366f1 0%, transparent 70%)", animationDuration: "5s" }} />
        <div className="absolute top-1/3 right-1/4 w-[400px] h-[400px] rounded-full opacity-[0.08]"
          style={{ background: "radial-gradient(circle, #10b981 0%, transparent 60%)" }} />
        <div className="absolute inset-0 opacity-[0.03]"
          style={{
            backgroundImage: `linear-gradient(rgba(255,255,255,0.1) 1px, transparent 1px),
                              linear-gradient(90deg, rgba(255,255,255,0.1) 1px, transparent 1px)`,
            backgroundSize: "50px 50px",
          }} />
      </div>

      <div className="relative z-10 w-full max-w-lg">
        {/* Logo */}
        <div className="flex flex-col items-center mb-8">
          <div className="relative mb-5">
            <div className="absolute inset-0 blur-2xl opacity-50 scale-[2]"
              style={{ background: "linear-gradient(135deg, #10b981, #059669)" }} />
            <div className="relative w-16 h-16 rounded-2xl flex items-center justify-center shadow-2xl"
              style={{ background: "linear-gradient(135deg, #10b981 0%, #059669 50%, #047857 100%)" }}>
              <ShieldPlus className="w-9 h-9 text-white" strokeWidth={1.5} />
            </div>
          </div>
          <h1 className="text-3xl font-bold text-white tracking-tight">MedAgenda</h1>
          <p className="text-white/50 text-sm mt-1 tracking-wide">Portal de Gestión Clínica</p>
        </div>

        {/* Progress steps */}
        <div className="flex items-center justify-center gap-3 mb-8">
          {[1, 2].map((s) => (
            <button key={s} onClick={() => setStep(s)} className="flex items-center gap-2">
              <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-medium transition-all duration-300 ${
                step >= s
                  ? "bg-gradient-to-r from-emerald-500 to-emerald-600 text-white shadow-lg shadow-emerald-500/25"
                  : "bg-white/5 text-white/40 border border-white/10"
              }`}>
                {step > s ? <CheckCircle2 className="w-4 h-4" /> : s}
              </div>
              {s < 2 && (
                <div className={`w-16 h-0.5 rounded-full transition-all duration-300 ${step > s ? "bg-emerald-500" : "bg-white/10"}`} />
              )}
            </button>
          ))}
        </div>

        {/* Card */}
        <div className="backdrop-blur-2xl bg-white/[0.03] border border-white/[0.08] rounded-3xl p-8 shadow-2xl shadow-black/20">
          <div className="relative mb-8">
            <div className="absolute -top-8 left-1/2 -translate-x-1/2 w-24 h-1 rounded-full bg-gradient-to-r from-emerald-500 to-teal-400" />
            <h2 className="text-2xl font-semibold text-white">
              {step === 1 ? "Crear Cuenta" : "Información Adicional"}
            </h2>
            <p className="text-white/40 text-sm mt-1">
              {step === 1 ? "Ingresa tus datos personales" : "Completa tu perfil médico"}
            </p>
          </div>

          <form onSubmit={handleRegister} className="space-y-5">
            {step === 1 ? (
              <>
                {/* Nombre */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <User className="w-3.5 h-3.5 text-emerald-500" /> Nombre Completo
                  </label>
                  <div className="relative group">
                    <input name="nombre" type="text" value={formData.nombre} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 text-white placeholder-white/25 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300"
                      placeholder="Juan Pérez" required />
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                </div>

                {/* Email */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <Mail className="w-3.5 h-3.5 text-emerald-500" /> Correo Electrónico
                  </label>
                  <div className="relative group">
                    <input name="email" type="email" value={formData.email} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 text-white placeholder-white/25 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300"
                      placeholder="correo@ejemplo.com" required />
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                </div>

                {/* Password */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <Lock className="w-3.5 h-3.5 text-emerald-500" /> Contraseña
                  </label>
                  <div className="relative group">
                    <input name="password" type={showPassword ? "text" : "password"} value={formData.password} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 pr-12 text-white placeholder-white/25 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300"
                      placeholder="••••••••" required minLength={6} />
                    <button type="button" onClick={() => setShowPassword(!showPassword)}
                      className="absolute right-4 top-1/2 -translate-y-1/2 text-white/40 hover:text-white/70 transition-colors">
                      {showPassword ? <EyeOff className="w-5 h-5" /> : <Eye className="w-5 h-5" />}
                    </button>
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                  {formData.password && (
                    <div className="flex items-center gap-3 mt-2">
                      <div className="flex-1 h-1 bg-white/10 rounded-full overflow-hidden">
                        <div className={`h-full ${color} transition-all duration-300`} style={{ width: `${strength}%` }} />
                      </div>
                      <span className={`text-xs ${color.replace("bg-", "text-")}`}>{label}</span>
                    </div>
                  )}
                </div>

                {/* Confirm Password */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <Lock className="w-3.5 h-3.5 text-emerald-500" /> Confirmar Contraseña
                  </label>
                  <div className="relative group">
                    <input name="confirmPassword" type={showConfirmPassword ? "text" : "password"} value={formData.confirmPassword} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 pr-12 text-white placeholder-white/25 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300"
                      placeholder="••••••••" required minLength={6} />
                    <button type="button" onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                      className="absolute right-4 top-1/2 -translate-y-1/2 text-white/40 hover:text-white/70 transition-colors">
                      {showConfirmPassword ? <EyeOff className="w-5 h-5" /> : <Eye className="w-5 h-5" />}
                    </button>
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                  {formData.confirmPassword && formData.password !== formData.confirmPassword && (
                    <p className="text-xs text-red-400 mt-1">Las contraseñas no coinciden</p>
                  )}
                </div>

                <button type="button" onClick={() => setStep(2)}
                  className="w-full mt-6 bg-gradient-to-r from-emerald-500 to-emerald-600 hover:from-emerald-400 hover:to-emerald-500 text-white font-semibold py-3.5 rounded-xl transition-all duration-300 flex items-center justify-center gap-2 shadow-lg shadow-emerald-500/20 hover:shadow-emerald-500/30 hover:scale-[1.01] active:scale-[0.99]">
                  Continuar <ArrowRight className="w-5 h-5" />
                </button>
              </>
            ) : (
              <>
                {/* Cédula */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <FileText className="w-3.5 h-3.5 text-emerald-500" /> Cédula de Identidad
                  </label>
                  <div className="relative group">
                    <input name="cedula" type="text" value={formData.cedula} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 text-white placeholder-white/25 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300"
                      placeholder="000-0000000-0" />
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                </div>

                {/* Teléfono */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <Phone className="w-3.5 h-3.5 text-emerald-500" /> Teléfono
                  </label>
                  <div className="relative group">
                    <input name="telefono" type="tel" value={formData.telefono} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 text-white placeholder-white/25 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300"
                      placeholder="(809) 000-0000" />
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                </div>

                {/* Fecha de Nacimiento */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <Calendar className="w-3.5 h-3.5 text-emerald-500" /> Fecha de Nacimiento
                  </label>
                  <div className="relative group">
                    <input name="fechaNacimiento" type="date" value={formData.fechaNacimiento} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 text-white focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300 [color-scheme:dark]" />
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                </div>

                {/* Tipo de Seguro */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <CreditCard className="w-3.5 h-3.5 text-emerald-500" /> Tipo de Seguro
                  </label>
                  <div className="relative group">
                    <select name="tipoSeguro" value={formData.tipoSeguro} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 text-white focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300 appearance-none cursor-pointer">
                      <option value="" className="bg-[#0a0f1e]">Seleccionar...</option>
                      <option value="ars-humano" className="bg-[#0a0f1e]">ARS Humano</option>
                      <option value="ars-universal" className="bg-[#0a0f1e]">ARS Universal</option>
                      <option value="ars-senasa" className="bg-[#0a0f1e]">SENASA</option>
                      <option value="ars-reservas" className="bg-[#0a0f1e]">ARS Reservas</option>
                      <option value="privado" className="bg-[#0a0f1e]">Seguro Privado</option>
                      <option value="ninguno" className="bg-[#0a0f1e]">Sin Seguro</option>
                    </select>
                    <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none">
                      <svg className="w-4 h-4 text-white/40" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                      </svg>
                    </div>
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                </div>

                {/* Número de Seguro */}
                <div className="space-y-2">
                  <label className="text-sm text-white/70 font-medium flex items-center gap-2">
                    <FileText className="w-3.5 h-3.5 text-emerald-500" /> Número de Seguro
                  </label>
                  <div className="relative group">
                    <input name="numeroSeguro" type="text" value={formData.numeroSeguro} onChange={handleChange}
                      className="w-full bg-white/[0.03] border border-white/[0.08] rounded-xl py-3.5 px-4 text-white placeholder-white/25 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 focus:border-emerald-500/50 focus:bg-white/[0.05] transition-all duration-300"
                      placeholder="Número de afiliación" />
                    <div className="absolute inset-0 rounded-xl bg-gradient-to-r from-emerald-500/20 to-teal-500/20 opacity-0 group-focus-within:opacity-100 -z-10 blur-xl transition-opacity duration-300" />
                  </div>
                </div>

                {error && (
                  <div role="alert" className="rounded-xl border border-red-400/30 bg-red-500/10 p-3 text-sm text-red-300">
                    {error}
                  </div>
                )}

                <div className="flex gap-3 mt-6">
                  <button type="button" onClick={() => setStep(1)}
                    className="flex-1 bg-white/5 hover:bg-white/10 border border-white/10 text-white font-medium py-3.5 rounded-xl transition-all duration-300">
                    Atrás
                  </button>
                  <button type="submit" disabled={isLoading}
                    className="flex-[2] bg-gradient-to-r from-emerald-500 to-emerald-600 hover:from-emerald-400 hover:to-emerald-500 text-white font-semibold py-3.5 rounded-xl transition-all duration-300 flex items-center justify-center gap-2 shadow-lg shadow-emerald-500/20 hover:scale-[1.01] active:scale-[0.99] disabled:opacity-70 disabled:cursor-not-allowed disabled:hover:scale-100">
                    {isLoading ? <><Loader2 className="h-5 w-5 animate-spin" /><span>Creando...</span></> : "Crear Cuenta"}
                  </button>
                </div>
              </>
            )}
          </form>

          <p className="text-center mt-8 text-white/40 text-sm">
            ¿Ya tienes una cuenta?{" "}
            <Link href="/login" className="text-emerald-400 hover:text-emerald-300 font-medium transition-colors">
              Iniciar Sesión
            </Link>
          </p>
        </div>

        {/* Features */}
        <div className="mt-8 grid grid-cols-3 gap-4">
          {[
            { icon: "🔒", label: "Datos Seguros" },
            { icon: "⚡", label: "Acceso Rápido" },
            { icon: "📱", label: "Multiplataforma" },
          ].map((feature, i) => (
            <div key={i} className="text-center">
              <div className="text-2xl mb-1">{feature.icon}</div>
              <div className="text-xs text-white/40">{feature.label}</div>
            </div>
          ))}
        </div>

        <p className="text-center mt-8 text-white/25 text-xs">
          © {new Date().getFullYear()} MedAgenda. Todos los derechos reservados.
        </p>
      </div>
    </div>
  );
}