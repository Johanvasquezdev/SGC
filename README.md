
# 🌐 Arquitectura del Frontend Web SGCM (MedAgenda)

Bienvenido a la documentación arquitectónica de la aplicación web cliente **MedAgenda**, desarrollada con **Next.js 14 (App Router)** y **React**. Este documento detalla los patrones de diseño, flujos de integración y la estricta separación de responsabilidades implementada en la capa de presentación.

---

## 📐 Visión General de la Arquitectura

```text
┌─────────────────────────────────────────────────────────────┐
│                 CAPA DE PRESENTACIÓN (UI)                   │
│  (React Server & Client Components - src/app & components)  │
│  - Renderizado visual y captura de eventos                  │
│  - Manejo de estado local (Hooks)                           │
└──────────────────┬──────────────────────────────────────────┘
                   │ 1. Invoca métodos lógicos
┌──────────────────▼──────────────────────────────────────────┐
│                 CAPA DE SERVICIOS                           │
│  (Lógica de consumo HTTP - src/services)                    │
│  - CitaService.ts, AuthService.ts                           │
│  - Desacopla la UI de la red                                │
└──────────────────┬──────────────────────────────────────────┘
                   │ 2. Tipado estricto (DTOs - src/types)
┌──────────────────▼──────────────────────────────────────────┐
│             INTEGRACIÓN HTTP & SEGURIDAD                    │
│  (Instancia de Axios + Interceptor JWT - src/lib/api.ts)    │
│  - Inyección automática de token Bearer                     │
│  - Intercepción global de errores (401, 403)                │
└──────────────────┬──────────────────────────────────────────┘
                   │ 3. Petición HTTPS / JSON
┌──────────────────▼──────────────────────────────────────────┐
│             API REST BACKEND (.NET 8)                       │
└─────────────────────────────────────────────────────────────┘


## 🛠️ Stack Tecnológico
Framework: Next.js 14 (App Router)

Librería UI: React 18 + TypeScript

Estilos: Tailwind CSS (con soporte Glassmorphism y Dark/Light mode)

Cliente HTTP: Axios

Notificaciones UI: Sonner (Toasts)

Iconografía: Lucide React

Pagos: Stripe (Stripe.js)

---

## 🔄 Flujo de Datos y Separación de Responsabilidades
El proyecto prohíbe estrictamente realizar llamadas directas a la API (fetch o axios) desde los componentes visuales. Se utiliza un flujo unidireccional de 3 capas:

1. Componente UI (Vistas)
Se encarga exclusivamente de renderizar, capturar la acción del usuario y mostrar estados de carga (IsLoading).

TypeScript
// src/app/paciente/agendar/page.tsx
"use client";
import { useState } from 'react';
import { toast } from 'sonner';
import { CitaService } from '@/services/CitaService';

export default function AgendarCitaForm() {
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (datosCita) => {
    setIsLoading(true);
    try {
      // Delegamos la acción al Servicio
      await CitaService.crearCita(datosCita);
      toast.success("¡Cita agendada con éxito!");
    } catch (error) {
      toast.error("Error al agendar la cita. Verifique sus datos.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* Elementos UI y lógica de renderizado... */}
      <button disabled={isLoading}>
        {isLoading ? 'Guardando...' : 'Agendar Cita'}
      </button>
    </form>
  );
}
2. Capa de Servicios
Actúa como puente. Recibe los datos de la UI, los formatea a DTOs y llama al cliente HTTP.

TypeScript
// src/services/CitaService.ts
import api from '@/lib/api';
import { CreateCitaDTO, CitaResponseDTO } from '@/types/cita.dto';

export class CitaService {
  static async crearCita(dto: CreateCitaDTO): Promise<CitaResponseDTO> {
    const response = await api.post<CitaResponseDTO>('/api/citas', dto);
    return response.data;
  }
}
3. Cliente HTTP e Interceptor (Seguridad)
Se inyecta automáticamente el JWT en cada petición saliente.

TypeScript
// src/lib/api.ts
import axios from 'axios';

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5189',
});

// Interceptor para inyectar el Token JWT
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('jwt_token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;


## 📂 Estructura de Carpetas
Plaintext
src/
├── app/                  # App Router: Rutas, Layouts y Páginas
│   ├── (auth)/           # Rutas de inicio de sesión y registro
│   ├── admin/            # Dashboard del Administrador
│   ├── paciente/         # Dashboard del Paciente
│   ├── globals.css       # Estilos globales y variables Tailwind
│   └── layout.tsx        # Root layout
├── components/           # Componentes UI reutilizables
│   ├── ui/               # Botones, Inputs, Spinners, Modales
│   └── layout/           # Sidebar, Navbar, Footer
├── lib/                  # Utilidades y configuración core
│   ├── api.ts            # Configuración de Axios e Interceptores
│   └── utils.ts          # Funciones de ayuda (formateo de fechas, etc.)
├── services/             # Lógica de consumo de la API .NET
│   ├── AuthService.ts
│   ├── CitaService.ts
│   └── MedicoService.ts
└── types/                # Interfaces TypeScript (DTOs equivalentes al backend)
    ├── cita.dto.ts
    ├── usuario.dto.ts
    └── respuestas.ts
    
## 🔐 Manejo de Errores y Experiencia de Usuario (UX)
Validación en Doble Capa: Formularios controlados en React previenen envíos nulos, respaldados por la validación del dominio en el backend.

Feedback Inmediato: Uso de Sonner para mostrar Toasts elegantes (éxito, advertencia o error) interpretando los códigos HTTP (400, 401, 500) que retorna el backend.

Estados Asíncronos: Toda petición a la red bloquea botones dinámicamente y despliega Spinners o Skeletons para evitar clics duplicados y mejorar la percepción de velocidad.


##🎯 Flujos Funcionales Implementados
🏥 Perfil Paciente
Autenticación y registro seguro.

Directorio médico avanzado.

Gestión del ciclo de vida de la cita (Agendar, Reprogramar, Cancelar).

Pasarela de pago integrada (Stripe).

Notificaciones y chat asíncrono.

🛡️ Perfil Administrador
Panel de métricas principales.

Módulos CRUD (Crear, Leer, Actualizar, Eliminar) para:

Usuarios

Médicos (Aprobación de Exequatur)

Especialidades Clínicas

Proveedores de Salud (ARS/Hospitales)

Registro de Auditoría del Sistema.

## 🚀 Instalación y Ejecución Local
Clonar el repositorio y entrar a la rama correcta:

Bash
git clone [https://github.com/Johanvasquezdev/SGC.git](https://github.com/Johanvasquezdev/SGC.git)
cd SGC
git checkout feature/web-20251235
Instalar dependencias:

Bash
npm install
Configurar Variables de Entorno:
Crea un archivo .env.local en la raíz del proyecto y define la URL de la API de .NET:

Fragmento de código
NEXT_PUBLIC_API_URL=http://localhost:5189
Ejecutar servidor de desarrollo:

Bash
npm run dev
La aplicación estará disponible en http://localhost:3000.

Participantes: Johan Vásquez (2025-1235) | Versión: 1.0.0 | Estado: ✅ Completo
