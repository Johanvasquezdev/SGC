"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { Loader2, User, Mail, Lock, CheckCircle2, ShieldPlus } from "lucide-react";
import { toast } from "sonner";
import { PacienteService } from "@/services/paciente.service";

export default function RegistroPage() {
  const router = useRouter();
  const [formData, setFormData] = useState({
    nombre: "",
    email: "",
    password: "",
    confirmPassword: "",
    cedula: "",
    telefono: "",
    fechaNacimiento: "",
    tipoSeguro: "",
    numeroSeguro: ""
  });
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    if (formData.password !== formData.confirmPassword) {
      setError("Las contrasenas no coinciden.");
      setIsLoading(false);
      return;
    }

    try {
      // Llama a la API real para crear el paciente.
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
      
      setTimeout(() => {
        router.push("/login");
      }, 2000);
    } catch (err: any) {
      setError("Ocurrio un error al crear la cuenta. Por favor, intenta de nuevo.");
      toast.error("No se pudo crear la cuenta");
    } finally {
      setIsLoading(false);
    }
  };

  if (isSuccess) {
    return (
      <div className="min-h-screen w-full relative z-10 flex items-center justify-center px-6 py-10">
        <div className="w-full max-w-md rounded-2xl border border-white/10 bg-white/5 p-8 text-center text-white backdrop-blur-xl animate-in zoom-in duration-300">
          <CheckCircle2 className="mx-auto mb-4 h-16 w-16 text-emerald-400" />
          <h2 className="text-2xl font-bold">Cuenta creada</h2>
          <p className="mt-2 text-white/70">Seras redirigido al inicio de sesion.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen w-full relative z-10 flex items-start justify-center px-6 py-10">
      <div className="w-full max-w-md">
        <div className="mb-8 flex flex-col items-center">
          <div className="relative mb-4">
            <div
              className="absolute inset-0 scale-150 rounded-2xl blur-xl opacity-60"
              style={{ background: "linear-gradient(135deg, #10b981, #059669)" }}
            />
            <div
              className="relative h-16 w-16 rounded-2xl flex items-center justify-center"
              style={{ background: "linear-gradient(135deg, #10b981, #059669)" }}
            >
              <ShieldPlus className="h-8 w-8 text-white" strokeWidth={1.5} />
            </div>
          </div>

          <h1 className="text-3xl font-bold tracking-tight text-white">MedAgenda</h1>
          <p className="mt-1 text-sm text-white/60">Portal de Gestion Clinica</p>
        </div>

        <div className="rounded-2xl border border-white/10 bg-white/5 p-8 backdrop-blur-xl animate-in slide-in-from-bottom-4 duration-500">
          <h2 className="mb-6 text-2xl font-bold text-white">Crear Cuenta</h2>

          <form onSubmit={handleRegister} className="space-y-4">
            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Nombre Completo</label>
              <div className="relative">
                <User className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-white/40" />
                <input
                  type="text"
                  name="nombre"
                  required
                  className="w-full rounded-xl border border-white/10 bg-white/5 py-3 pl-10 pr-4 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                  value={formData.nombre}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Correo Electronico</label>
              <div className="relative">
                <Mail className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-white/40" />
                <input
                  type="email"
                  name="email"
                  required
                  className="w-full rounded-xl border border-white/10 bg-white/5 py-3 pl-10 pr-4 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                  value={formData.email}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Contrasena</label>
              <div className="relative">
                <Lock className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-white/40" />
                <input
                  type="password"
                  name="password"
                  required
                  minLength={6}
                  className="w-full rounded-xl border border-white/10 bg-white/5 py-3 pl-10 pr-4 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                  value={formData.password}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Confirmar Contrasena</label>
              <div className="relative">
                <Lock className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-white/40" />
                <input
                  type="password"
                  name="confirmPassword"
                  required
                  minLength={6}
                  className="w-full rounded-xl border border-white/10 bg-white/5 py-3 pl-10 pr-4 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                  value={formData.confirmPassword}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Cedula</label>
              <input
                type="text"
                name="cedula"
                className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                value={formData.cedula}
                onChange={handleChange}
              />
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Telefono</label>
              <input
                type="text"
                name="telefono"
                className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                value={formData.telefono}
                onChange={handleChange}
              />
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Fecha de Nacimiento</label>
              <input
                type="date"
                name="fechaNacimiento"
                className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white outline-none transition-all [color-scheme:dark] focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                value={formData.fechaNacimiento}
                onChange={handleChange}
              />
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Tipo de Seguro</label>
              <input
                type="text"
                name="tipoSeguro"
                className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                value={formData.tipoSeguro}
                onChange={handleChange}
              />
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-white/80">Numero de Seguro</label>
              <input
                type="text"
                name="numeroSeguro"
                className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white placeholder:text-white/30 outline-none transition-all focus:border-transparent focus:ring-2 focus:ring-emerald-500"
                value={formData.numeroSeguro}
                onChange={handleChange}
              />
            </div>

            {error && (
              <div className="rounded-xl border border-red-400/30 bg-red-500/10 p-3 text-sm text-red-200">{error}</div>
            )}

            <button
              disabled={isLoading}
              className="mt-6 flex w-full items-center justify-center rounded-xl bg-emerald-500 py-3 font-semibold text-white transition-all duration-200 hover:bg-emerald-600 disabled:cursor-not-allowed disabled:opacity-70"
            >
              {isLoading ? <Loader2 className="h-5 w-5 animate-spin" /> : "Crear Cuenta"}
            </button>
          </form>

          <p className="mt-6 text-center text-sm text-white/50">
            Ya tienes una cuenta?{" "}
            <Link href="/login" className="font-medium text-emerald-400 transition-colors hover:text-emerald-300">
              Inicia Sesion
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}
