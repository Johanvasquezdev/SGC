"use client";
import { useState, useEffect } from "react";
import { Search, MapPin, Star, Clock, Filter, Loader2 } from "lucide-react";
import { MedicoDTO } from "@/types/api.types";
import { MedicoService } from "@/services/medico.service";
import { AgendarCitaModal } from "@/components/citas/AgendarCita";

export default function BuscarMedicosPage() {
  const [medicos, setMedicos] = useState<MedicoDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedMedico, setSelectedMedico] = useState<MedicoDTO | null>(null);

  useEffect(() => {
    const fetchMedicos = async () => {
      try {
        const data = await MedicoService.obtenerTodos();
        setMedicos(data.filter(m => m.activo));
      } catch (err) {} finally { setLoading(false); }
    };
    fetchMedicos();
  }, []);

  const filtered = medicos.filter(m => m.nombre.toLowerCase().includes(searchTerm.toLowerCase()));

  if (loading) return <div className="flex justify-center py-12"><Loader2 className="animate-spin" /></div>;

  return (
    <div className="space-y-8">
      <input
        type="text"
        placeholder="Buscar médico..."
        className="w-full p-3 border border-border bg-card text-foreground rounded-xl placeholder:text-muted-foreground"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />
      <div className="grid md:grid-cols-3 gap-6">
        {filtered.map(medico => (
          <div
            key={medico.id}
            className="bg-card p-6 rounded-2xl border border-border flex flex-col"
          >
            <h3 className="font-semibold text-lg text-foreground">{medico.nombre}</h3>
            <p className="text-emerald-600 dark:text-emerald-400 text-sm">
              {medico.especialidadNombre || "Especialista"}
            </p>
            <button
              onClick={() => setSelectedMedico(medico)}
              className="mt-4 bg-emerald-600 text-white py-2 rounded-xl hover:bg-emerald-500 transition-colors"
            >
              Agendar Cita
            </button>
          </div>
        ))}
      </div>
      <AgendarCitaModal medico={selectedMedico} isOpen={!!selectedMedico} onClose={() => setSelectedMedico(null)} />
    </div>
  );
}

