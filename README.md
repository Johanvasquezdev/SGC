# MedAgenda — Sistema de Gestión de Citas Médicas (SGCM)

Proyecto académico de aplicación web y desktop de sistema de gestión de citas médicas  
**Profesor:** Francis Ramirez · **Asignatura:** Programación II · **ITLA 2026**

---

> ### ⚠️ INFORMACIÓN IMPORTANTE SOBRE RAMAS Y COMMITS
>
> **Ramas sin matrícula** (`feature/api`, `feature/ioc`, `feature/application`, `feature/dominio`, `feature/persistencia`, `feature/unittest`) fueron desarrolladas **en conjunto** por ambos integrantes: Johan Vasquez y Gregori Evangelista.
>
> **Sobre los commits:** Debido al flujo de trabajo del equipo, la mayoría de los commits fueron realizados desde la cuenta de Johan Vasquez. El código correspondiente a la App Desktop (.NET MAUI) y los módulos bajo responsabilidad de Gregori Evangelista fue desarrollado por él y enviado al repositorio a través de su compañero. Esto no refleja menor participación de ninguno de los dos integrantes.

---

## 📋 Descripción del Proyecto

MedAgenda es un sistema de software diseñado para gestionar de forma integral el ciclo de vida de las citas médicas en clínicas y centros de salud. El sistema centraliza la información de pacientes, médicos y citas, eliminando los procesos manuales que generan errores, duplicidad de información y mala comunicación entre los actores del sistema.

### Problema que resuelve

Actualmente muchos centros médicos gestionan sus citas de forma manual o con herramientas no especializadas, lo que genera:

- Conflictos de horarios y doble asignación de citas
- Falta de notificaciones oportunas a pacientes y médicos
- Ausencia de trazabilidad y control centralizado
- Carga administrativa excesiva que afecta la calidad del servicio

### Actores del sistema

| Actor | Descripción |
|-------|-------------|
| Paciente | Busca médicos, programa, consulta y cancela sus citas |
| Médico | Gestiona su agenda, disponibilidad e historial de pacientes |
| Administrador | Gestiona usuarios, roles, especialidades y catálogos del sistema |

### Alcance

✅ **El sistema cubre:**
- Gestión completa del ciclo de vida de citas médicas
- Gestión de disponibilidad de médicos
- Gestión de pacientes, médicos y especialidades
- Gestión de proveedores de salud
- Envío de notificaciones y recordatorios (Email, SMS, Push)
- Registro histórico y auditoría de operaciones
- Facturación médica y pagos en línea (Stripe)
- Asistente virtual con IA (Claude API)

❌ **Fuera de alcance:**
- Expedientes clínicos completos
- Diagnósticos automatizados o soporte médico con IA clínica
- Despliegue en producción real en nube (solo entorno local + Supabase hosting)
- Aplicaciones móviles nativas

---

## 🏗️ Arquitectura

MedAgenda está construido bajo una **Arquitectura en Capas (Layering) con Clean Architecture** en el backend, garantizando separación de responsabilidades, mantenibilidad y escalabilidad.

### Stack tecnológico

| Capa | Tecnología |
|------|------------|
| Portal Web (Pacientes/Admin) | Next.js (React) |
| App de Escritorio (Médicos) | .NET MAUI |
| Backend API | .NET 8 Web API |
| Base de Datos | PostgreSQL (Supabase Cloud — solo hosting) |
| ORM | Entity Framework Core |
| Autenticación | JWT custom (sin Supabase Auth) |
| Tiempo Real | SignalR (WebSockets) |
| Notificaciones Email | MailKit / SMTP |
| Notificaciones SMS | Twilio API |
| Caché | Redis |
| Pagos | Stripe API |
| Chatbot IA | Claude API (Anthropic) |
| Logging | Serilog via ISGCLogger |
| CI/CD | GitHub Actions |

### Estructura de la solución

```
SGC Solution
├── Core/
│   ├── SGC.Domain          ← Entidades, interfaces, enums, excepciones, validadores
│   └── SGC.Application     ← Servicios de casos de uso, DTOs, Mappers, Contracts
├── Infrastructure/
│   ├── SGC.Infrastructure  ← Email, SMS, SignalR, Stripe, Claude, Redis, Logging
│   └── SGC.Persistence     ← EF Core, repositorios y migraciones
├── API/
│   └── SGC.API             ← 12 Controllers, JWT Auth, Swagger, Middleware
├── IOC/
│   └── SGC.IOC             ← Inyección de dependencias (DependencyContainer)
├── Web/
│   ├── sgc.web.client      ← Next.js (portal paciente y panel admin)
│   └── SGC.Web.Server      ← BFF / Proxy, SSR, Middleware Web
├── Desktop/
│   └── SGC.Desktop         ← .NET MAUI (gestión médica — App Médico)
└── ApplicationTest/
    └── SGC.ApplicationTest ← Pruebas unitarias e integración (xUnit + Moq)
```

### Capas del Backend

```
┌─────────────────────────────────────────────────────┐
│          Capa de Presentación (Frontend)             │  Next.js + .NET MAUI
├─────────────────────────────────────────────────────┤
│          Capa de API (Controllers)                   │  .NET 8 Web API — 12 Controllers
├─────────────────────────────────────────────────────┤
│          Capa de Aplicación (Casos de Uso)           │  Servicios, DTOs, Mappers
├─────────────────────────────────────────────────────┤
│          Capa de Dominio (Core)                      │  Entidades, Interfaces, Reglas
├─────────────────────────────────────────────────────┤
│     Persistencia          │     Infraestructura      │  EF Core / Email, SMS, Stripe...
└─────────────────────────────────────────────────────┘
```

### Principios aplicados

- **Clean Architecture** — El núcleo del sistema es independiente de frameworks y tecnologías externas
- **DIP (Inversión de Dependencias)** — Interfaces definidas en el dominio, implementaciones en Infrastructure
- **Patrón Repository** — Abstracción del acceso a datos
- **MVVM** — Patrón aplicado en la aplicación de escritorio (.NET MAUI)
- **MVC** — Patrón aplicado en el portal web (Next.js)

---

## 👥 Equipo y División de Trabajo

> Las capas compartidas del backend (SGC.Domain, SGC.Application, SGC.Persistence, SGC.Infrastructure, SGC.IOC, SGC.API) fueron desarrolladas **en conjunto** por ambos integrantes.

### Johan Vasquez — Matrícula: 2025-1235

**Portal Web (sgc.web.client + SGC.Web.Server):**
- Módulo de Registro y Login
- Módulo de Búsqueda de Médicos
- Módulo de Agendamiento de Citas
- Módulo de Mis Citas
- Módulo de Pagos (Stripe)
- Módulo de Notificaciones
- Módulo de Chatbot (Claude API)

**Panel Administrador:**
- Módulo de Gestión de Usuarios
- Módulo de Gestión de Médicos
- Módulo de Gestión de Especialidades
- Módulo de Gestión de Proveedores
- Módulo de Auditoría

**Ramas:** `feature/web-20251235`, y ramas compartidas sin matrícula.

---

### Gregori Evangelista — Matrícula: 2025-1232

**App Médico Desktop (SGC.Desktop — .NET MAUI):**
- Módulo de Agenda Médica
- Módulo de Gestión de Citas (confirmar, iniciar, completar, asistencia)
- Módulo de Disponibilidad
- Módulo de Historial de Pacientes
- Módulo de Seguridad JWT (Desktop)

**Ramas:** `feature/Desktop-20251232`, `feature/desktop-agenda-20251232`, `feature/desktop-citas-20251232`, `feature/desktop-disponibilidad-20251232`, `feature/desktop-pacientes-20251232`, `feature/desktop-seguridad-jwt-20251232`, y ramas compartidas sin matrícula.

---

## 🔌 API REST

**Swagger UI:** `http://localhost:5189/swagger`

### Ramas por integrante

| Rama | Integrante | Descripción |
|------|------------|-------------|
| `feature/api` | Conjunto (ambos) | Controllers, Middleware, Program.cs, Swagger |
| `feature/ioc` | Conjunto (ambos) | DependencyContainer — registro de dependencias |
| `feature/application` | Conjunto (ambos) | Servicios, DTOs, Mappers, Contracts |
| `feature/dominio` | Conjunto (ambos) | Entidades, interfaces, enums, excepciones, validators |
| `feature/persistencia` | Conjunto (ambos) | DbContext, repositorios EF Core, migraciones |
| `feature/unittest` | Conjunto (ambos) | Pruebas unitarias xUnit + Moq |
| `feature/infraestructure` | Conjunto (ambos) | Email, SMS, SignalR, Stripe, Claude, Redis, Serilog |
| `feature/web-20251235` | Johan Vasquez | Portal Web Next.js — Portal Paciente + Panel Admin |
| `feature/Desktop-20251232` | Gregori Evangelista | App Desktop .NET MAUI — estructura base |
| `feature/desktop-agenda-20251232` | Gregori Evangelista | Módulo Agenda Médica |
| `feature/desktop-citas-20251232` | Gregori Evangelista | Módulo Gestión de Citas |
| `feature/desktop-disponibilidad-20251232` | Gregori Evangelista | Módulo Disponibilidad |
| `feature/desktop-pacientes-20251232` | Gregori Evangelista | Módulo Historial de Pacientes |
| `feature/desktop-seguridad-jwt-20251232` | Gregori Evangelista | Módulo Seguridad JWT (Desktop) |

---

## 🔌 API REST

**Swagger UI:** `http://localhost:5189/swagger`

| Controller | Endpoints principales |
|------------|-----------------------|
| AuthController | POST /api/auth/login, POST /api/auth/refresh |
| CitasController | POST /api/citas, GET /api/citas/paciente/{id}, GET /api/citas/medico/agenda, PUT /{id}/cancelar, PUT /{id}/reprogramar, PUT /{id}/confirmar, PUT /{id}/iniciar-consulta, PUT /{id}/asistencia |
| MedicosController | GET /api/medicos, GET /api/medicos/{id}, POST, PUT |
| PacientesController | POST /api/pacientes, GET /api/pacientes/{id}, PUT |
| DisponibilidadController | POST/PUT/DELETE /api/disponibilidad |
| EspecialidadesController | GET/POST/PUT /api/especialidades |
| ProveedoresController | GET/POST/PUT /api/proveedores |
| UsuariosController | GET/PUT /api/usuarios |
| NotificacionesController | GET /api/Notificaciones/usuario/{id} |
| PagosController | POST /api/Pagos/crear-intento |
| ChatbotController | POST /api/chatbot/mensaje |
| AuditoriaController | GET /api/auditoria |

### Comunicación en tiempo real (SignalR)

- **CitaHub** — Notificaciones push de nuevas citas al médico en la app desktop
- **DisponibilidadHub** — Actualización automática de disponibilidad médica en el calendario web

---

## 📄 Documentación

| Documento | Descripción |
|-----------|-------------|
| SRS MedAgenda | Especificación de Requisitos de Software |
| SAD MedAgenda v1.1 | Documento de Arquitectura de Software (actualizado 23/03/2026) |

---

ITLA — Instituto Tecnológico de las Américas · Programación II · 2026
