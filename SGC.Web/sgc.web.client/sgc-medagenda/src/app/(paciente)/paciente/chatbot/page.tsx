"use client";

import { useState, useRef, useEffect } from "react";
import { Bot, Send, Loader2, Calendar, Stethoscope, CreditCard } from "lucide-react";
import { ChatbotService } from "@/services/chat.service";
import { useAuth } from "@/components/providers/AuthProvider";
import { usePageTransition } from "@/components/animations/Animatedcomponents";

import { ChatActionCard } from "@/components/chat/ChatActionCard";

const quickActions = [
  { 
    icon: Stethoscope, 
    label: "Directorio Médico", 
    description: "Encuentra y agenda citas con nuestros especialistas.",
    action: "Buscar médico"
  },
  { 
    icon: Calendar, 
    label: "Agendar Cita", 
    description: "Reserva una consulta de forma rápida y segura.",
    action: "Agendar cita"
  },
  { 
    icon: CreditCard, 
    label: "Pagos y Facturas", 
    description: "Consulta tu historial de transacciones.",
    action: "Ver pagos"
  },
];

export default function ChatbotPage() {
  const { user } = useAuth();
  const [messages, setMessages] = useState([
    {
      id: "1",
      role: "bot",
      content: "Hola, soy el asistente virtual de MedAgenda. Estoy aquí para ayudarte con cualquier pregunta relacionada con tus citas, médicos o servicios. ¿En qué puedo ayudarte hoy?",
    },
  ]);
  const [input, setInput] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const bottomRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const handleSend = async (e?: React.FormEvent, customMsg?: string) => {
    e?.preventDefault();
    const messageToSend = customMsg || input;
    if (!messageToSend.trim() || isLoading) return;

    if (!customMsg) setInput("");
    setMessages(p => [...p, { id: Date.now().toString(), role: "user", content: messageToSend }]);
    setIsLoading(true);
    
    try {
      const usuarioId = user?.id && user.id > 0 ? user.id : undefined;
      const res = await ChatbotService.enviarMensaje({ mensaje: messageToSend, usuarioId });
      setMessages(p => [...p, { id: Date.now().toString(), role: "bot", content: res.respuesta }]);
    } catch {
      setMessages(p => [...p, { id: Date.now().toString(), role: "bot", content: "Lo siento, hubo un error al conectar con el asistente. Inténtalo de nuevo." }]);
    } finally {
      setIsLoading(false);
    }
  };

  usePageTransition();

  return (
    <div className="flex flex-col h-[calc(100vh-8rem)] max-w-5xl mx-auto space-y-6 page-content">
      <header className="relative overflow-hidden rounded-3xl border border-emerald-500/10 bg-gradient-to-br from-emerald-500/10 to-teal-500/10 p-6 md:p-7 shadow-sm transition-all duration-500 hover:shadow-lg hover:shadow-emerald-500/5 backdrop-blur-sm">
        <div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-emerald-500/5 dark:bg-emerald-500/10 blur-3xl opacity-50" />
        <div className="absolute -bottom-20 -left-20 h-56 w-56 rounded-full bg-teal-500/5 dark:bg-teal-500/10 blur-3xl opacity-50" />

        <div className="relative z-10 flex items-center gap-4">
          <div className="w-14 h-14 rounded-2xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center shadow-inner group-hover:scale-110 transition-transform duration-500">
            <Bot className="w-8 h-8 text-emerald-600 dark:text-emerald-400" />
          </div>
          <div>
            <p className="text-[10px] font-black uppercase tracking-[0.3em] text-emerald-600/80 dark:text-emerald-400/80">
              Inteligencia Artificial
            </p>
            <h1 className="text-2xl font-black tracking-tight text-foreground">Asistente MedAgenda</h1>
            <p className="text-muted-foreground text-xs font-semibold">Resolviendo tus dudas en tiempo real</p>
          </div>
        </div>
      </header>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {quickActions.map((action, idx) => (
          <ChatActionCard
            key={idx}
            icon={action.icon}
            title={action.label}
            description={action.description}
            accentColor="emerald"
            onClick={() => handleSend(undefined, action.action)}
          />
        ))}
      </div>

      <div className="flex-1 overflow-hidden flex flex-col bg-card border border-border rounded-3xl shadow-sm relative">
        <div className="flex-1 overflow-auto p-6 space-y-4">
          {messages.map(m => (
            <div key={m.id} className={`flex ${m.role === "user" ? "justify-end" : "justify-start"}`}>
              <div className={`max-w-[85%] sm:max-w-[75%] rounded-2xl px-5 py-3.5 shadow-sm transition-all ${
                m.role === "user"
                  ? "bg-emerald-600 text-white font-medium"
                  : "bg-muted/50 border border-border text-foreground"
              }`}>
                {m.role === "bot" && (
                  <div className="flex items-center gap-2 mb-2 pb-2 border-b border-foreground/5">
                    <div className="w-6 h-6 rounded-full bg-emerald-500/10 flex items-center justify-center">
                      <Bot className="w-3.5 h-3.5 text-emerald-600 dark:text-emerald-400" />
                    </div>
                    <span className="text-emerald-600 dark:text-emerald-400 text-[10px] font-black uppercase tracking-widest">Asistente Virtual</span>
                  </div>
                )}
                <p className="text-sm leading-relaxed font-medium">{m.content}</p>
              </div>
            </div>
          ))}

          {isLoading && (
            <div className="flex justify-start">
              <div className="bg-muted/50 border border-border rounded-2xl px-5 py-4">
                <div className="flex items-center gap-3">
                  <div className="w-6 h-6 rounded-full bg-emerald-500/10 flex items-center justify-center">
                    <Bot className="w-3.5 h-3.5 animate-pulse text-emerald-600 dark:text-emerald-400" />
                  </div>
                  <div className="flex gap-1.5">
                    <span className="w-2 h-2 bg-emerald-500/40 rounded-full animate-bounce" style={{ animationDelay: "0ms" }} />
                    <span className="w-2 h-2 bg-emerald-500/40 rounded-full animate-bounce" style={{ animationDelay: "150ms" }} />
                    <span className="w-2 h-2 bg-emerald-500/40 rounded-full animate-bounce" style={{ animationDelay: "300ms" }} />
                  </div>
                </div>
              </div>
            </div>
          )}
          <div ref={bottomRef} />
        </div>

        <form
          onSubmit={handleSend}
          className="p-4 bg-muted/20 border-t border-border"
        >
          <div className="flex items-center gap-3 bg-background border border-border rounded-2xl p-2 pl-4 focus-within:ring-2 focus-within:ring-emerald-500/10 transition-all">
            <input
              type="text"
              value={input}
              onChange={e => setInput(e.target.value)}
              placeholder="Escribe tu mensaje aquí..."
              className="flex-1 bg-transparent text-foreground placeholder-muted-foreground focus:outline-none text-sm font-medium"
            />
            <button
              type="submit"
              disabled={!input.trim() || isLoading}
              className="w-10 h-10 rounded-xl bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center text-white transition-all shadow-lg shadow-emerald-500/20 active:scale-90"
            >
              {isLoading ? <Loader2 className="w-5 h-5 animate-spin" /> : <Send className="w-5 h-5" />}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}