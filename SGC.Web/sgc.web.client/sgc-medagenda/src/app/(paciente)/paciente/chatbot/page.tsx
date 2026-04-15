"use client";
import { useState, useRef, useEffect } from "react";
import { Send, Loader2 } from "lucide-react";
import { ChatbotService } from "@/services/chat.service";
import { useAuth } from "@/components/providers/AuthProvider";

export default function ChatbotPage() {
  const { user } = useAuth();
  const [messages, setMessages] = useState([{ id: "1", role: "bot", content: "Hola, soy el asistente virtual." }]);
  const [input, setInput] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const bottomRef = useRef<HTMLDivElement>(null);
  const inputId = "chatbot-input";

  useEffect(() => { bottomRef.current?.scrollIntoView({ behavior: "smooth" }); }, [messages]);

  const handleSend = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim() || isLoading) return;
    const userMsg = input; setInput("");
    setMessages(p => [...p, { id: Date.now().toString(), role: "user", content: userMsg }]);
    setIsLoading(true);
    try {
      const usuarioId = user?.id && user.id > 0 ? user.id : undefined;
      const res = await ChatbotService.enviarMensaje({ mensaje: userMsg, usuarioId });
      setMessages(p => [...p, { id: Date.now().toString(), role: "bot", content: res.respuesta }]);
    } catch (e) {
      setMessages(p => [...p, { id: Date.now().toString(), role: "bot", content: "Error al conectar." }]);
    } finally { setIsLoading(false); }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto h-[80vh] flex flex-col">
      <div className="flex-1 overflow-y-auto p-4 space-y-4 bg-slate-900/60 border border-slate-800/80 rounded-t-xl text-slate-100">
        {messages.map(m => (
          <div key={m.id} className={`flex ${m.role === 'user' ? 'justify-end' : 'justify-start'}`}>
            <div
              className={`p-3 rounded-xl max-w-[70%] ${
                m.role === 'user'
                  ? 'bg-emerald-600 text-white'
                  : 'bg-slate-950/70 border border-slate-800 text-slate-200'
              }`}
            >
              {m.content}
            </div>
          </div>
        ))}
        {isLoading && <Loader2 aria-hidden="true" className="animate-spin text-emerald-400" />}
        <div ref={bottomRef} />
      </div>
      <form onSubmit={handleSend} className="flex gap-2 p-4 bg-slate-900/60 border border-t-0 border-slate-800/80 rounded-b-xl">
        <label htmlFor={inputId} className="sr-only">
          Mensaje para el asistente
        </label>
        <input
          id={inputId}
          aria-label="Mensaje para el asistente"
          className="flex-1 p-3 border border-slate-800 bg-slate-950/70 text-slate-100 rounded-xl placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-emerald-500"
          value={input}
          onChange={e => setInput(e.target.value)}
          placeholder="Mensaje..."
        />
        <button type="submit" aria-label="Enviar mensaje" className="bg-emerald-600 text-white p-3 rounded-xl"><Send aria-hidden="true" /></button>
      </form>
    </div>
  );
}

