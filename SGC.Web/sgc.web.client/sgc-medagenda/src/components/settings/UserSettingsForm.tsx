"use client";

import { useEffect, useMemo, useState } from "react";
import { CheckCircle2, CircleUserRound, Loader2, Shield } from "lucide-react";
import { PerfilService } from "@/services/perfil.service";
import { useAuth } from "@/components/providers/AuthProvider";

export function UserSettingsForm() {
  const { user } = useAuth();
  const [loading, setLoading] = useState(true);
  const [savingProfile, setSavingProfile] = useState(false);
  const [savingPassword, setSavingPassword] = useState(false);
  const [profileMessage, setProfileMessage] = useState<string | null>(null);
  const [passwordMessage, setPasswordMessage] = useState<string | null>(null);

  const [nombre, setNombre] = useState("");
  const [email, setEmail] = useState("");
  const [telefono, setTelefono] = useState("");
  const [rol, setRol] = useState("");

  const [passwordActual, setPasswordActual] = useState("");
  const [passwordNueva, setPasswordNueva] = useState("");
  const [passwordConfirmacion, setPasswordConfirmacion] = useState("");

  const initials = useMemo(() => {
    const base = nombre || user?.nombre || "U";
    return base.trim().charAt(0).toUpperCase();
  }, [nombre, user?.nombre]);

  const telefonoHabilitado = rol.toLowerCase() === "paciente" || rol.toLowerCase() === "medico";

  useEffect(() => {
    const cargarPerfil = async () => {
      try {
        const perfil = await PerfilService.obtenerMiPerfil();
        setNombre(perfil.nombre || "");
        setEmail(perfil.email || "");
        setTelefono(perfil.telefono || "");
        setRol(String(perfil.rol || ""));
      } finally {
        setLoading(false);
      }
    };

    void cargarPerfil();
  }, []);

  const handleGuardarPerfil = async (e: React.FormEvent) => {
    e.preventDefault();
    setSavingProfile(true);
    setProfileMessage(null);

    try {
      const perfil = await PerfilService.actualizarMiPerfil({
        nombre,
        email,
        telefono: telefonoHabilitado ? telefono : null,
      });

      localStorage.setItem(
        "medagenda_user",
        JSON.stringify({
          id: perfil.id,
          nombre: perfil.nombre,
          rol: perfil.rol,
          email: perfil.email,
        })
      );
      window.dispatchEvent(new Event("auth-changed"));
      setProfileMessage("Perfil actualizado correctamente.");
    } catch (error: any) {
      setProfileMessage(error?.response?.data?.message || "No se pudo actualizar el perfil.");
    } finally {
      setSavingProfile(false);
    }
  };

  const handleCambiarPassword = async (e: React.FormEvent) => {
    e.preventDefault();
    setPasswordMessage(null);

    if (passwordNueva !== passwordConfirmacion) {
      setPasswordMessage("La confirmación de contraseña no coincide.");
      return;
    }

    setSavingPassword(true);
    try {
      await PerfilService.cambiarMiPassword({
        passwordActual,
        passwordNueva,
      });
      setPasswordActual("");
      setPasswordNueva("");
      setPasswordConfirmacion("");
      setPasswordMessage("Contraseña actualizada correctamente.");
    } catch (error: any) {
      setPasswordMessage(error?.response?.data?.message || "No se pudo cambiar la contraseña.");
    } finally {
      setSavingPassword(false);
    }
  };

  if (loading) {
    return (
      <div className="rounded-2xl border border-slate-800/80 bg-slate-900/60 p-8 text-center text-slate-300">
        <Loader2 className="mx-auto mb-3 h-6 w-6 animate-spin text-emerald-400" />
        Cargando configuración de usuario...
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <section className="rounded-2xl border border-slate-800/80 bg-slate-900/60 p-6">
        <div className="flex items-center gap-4">
          <div className="h-14 w-14 rounded-full border border-emerald-500/40 bg-emerald-500/20 text-emerald-300 font-semibold flex items-center justify-center">
            {initials}
          </div>
          <div>
            <h2 className="text-lg font-semibold text-white flex items-center gap-2">
              <CircleUserRound className="h-5 w-5 text-emerald-400" />
              Foto de perfil
            </h2>
            <p className="text-sm text-slate-400">Próximamente podrás cargar una foto real de perfil.</p>
          </div>
        </div>
      </section>

      <section className="rounded-2xl border border-slate-800/80 bg-slate-900/60 p-6">
        <h2 className="text-lg font-semibold text-white mb-4">Datos del perfil</h2>
        <form className="grid gap-4" onSubmit={handleGuardarPerfil}>
          <label className="grid gap-2">
            <span className="text-sm text-slate-300">Nombre</span>
            <input
              value={nombre}
              onChange={(e) => setNombre(e.target.value)}
              className="rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100"
              required
            />
          </label>

          <label className="grid gap-2">
            <span className="text-sm text-slate-300">Email</span>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100"
              required
            />
          </label>

          <label className="grid gap-2">
            <span className="text-sm text-slate-300">Teléfono</span>
            <input
              value={telefono}
              onChange={(e) => setTelefono(e.target.value)}
              disabled={!telefonoHabilitado}
              className="rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100 disabled:opacity-60"
              placeholder={telefonoHabilitado ? "809-555-0000" : "No aplica para este rol"}
            />
          </label>

          <button
            type="submit"
            disabled={savingProfile}
            className="mt-2 inline-flex items-center justify-center rounded-xl bg-emerald-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-emerald-400 disabled:opacity-60"
          >
            {savingProfile ? <Loader2 className="h-4 w-4 animate-spin" /> : "Guardar cambios"}
          </button>

          {profileMessage ? (
            <p className="text-sm text-emerald-300 flex items-center gap-2">
              <CheckCircle2 className="h-4 w-4" />
              {profileMessage}
            </p>
          ) : null}
        </form>
      </section>

      <section className="rounded-2xl border border-slate-800/80 bg-slate-900/60 p-6">
        <h2 className="text-lg font-semibold text-white mb-4 flex items-center gap-2">
          <Shield className="h-5 w-5 text-indigo-400" />
          Seguridad
        </h2>

        <form className="grid gap-4" onSubmit={handleCambiarPassword}>
          <label className="grid gap-2">
            <span className="text-sm text-slate-300">Contraseña actual</span>
            <input
              type="password"
              value={passwordActual}
              onChange={(e) => setPasswordActual(e.target.value)}
              className="rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100"
              required
            />
          </label>

          <label className="grid gap-2">
            <span className="text-sm text-slate-300">Nueva contraseña</span>
            <input
              type="password"
              value={passwordNueva}
              onChange={(e) => setPasswordNueva(e.target.value)}
              className="rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100"
              required
            />
          </label>

          <label className="grid gap-2">
            <span className="text-sm text-slate-300">Confirmar nueva contraseña</span>
            <input
              type="password"
              value={passwordConfirmacion}
              onChange={(e) => setPasswordConfirmacion(e.target.value)}
              className="rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100"
              required
            />
          </label>

          <button
            type="submit"
            disabled={savingPassword}
            className="mt-2 inline-flex items-center justify-center rounded-xl bg-indigo-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-indigo-400 disabled:opacity-60"
          >
            {savingPassword ? <Loader2 className="h-4 w-4 animate-spin" /> : "Actualizar contraseña"}
          </button>

          {passwordMessage ? <p className="text-sm text-slate-300">{passwordMessage}</p> : null}
        </form>
      </section>
    </div>
  );
}
