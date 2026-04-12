"use client";

import { Settings } from "lucide-react";
import { UserSettingsForm } from "@/components/settings/UserSettingsForm";

export default function AdminSettingsPage() {
  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <header>
        <h1 className="text-2xl font-bold text-white flex items-center gap-2">
          <Settings className="w-6 h-6 text-indigo-400" />
          Configuración de Usuario
        </h1>
        <p className="text-slate-400 mt-1">Actualiza tu perfil y tus credenciales de acceso.</p>
      </header>

      <UserSettingsForm />
    </div>
  );
}
