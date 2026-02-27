# 🏥 MedAgenda — Sistema de Gestión de Citas Médicas (SGCM)

> Proyecto académico de aplicacion web y desktop de sistema de gestion de citas medicas
> Profesor: Francis Ramirez

---

## 📋 Descripción del Proyecto

**MedAgenda** es un sistema de software diseñado para gestionar de forma integral el ciclo de vida de las citas médicas en clínicas y centros de salud. El sistema centraliza la información de pacientes, médicos y citas, eliminando los procesos manuales que generan errores, duplicidad de información y mala comunicación entre los actores del sistema.

### Problema que resuelve

Actualmente muchos centros médicos gestionan sus citas de forma manual o con herramientas no especializadas, lo que genera:

- Conflictos de horarios y doble asignación de citas
- Falta de notificaciones oportunas a pacientes y médicos
- Ausencia de trazabilidad y control centralizado
- Carga administrativa excesiva que afecta la calidad del servicio

### Actores del sistema

| Actor | Descripción |
|---|---|
| **Paciente** | Busca médicos, programa, consulta y cancela sus citas |
| **Médico** | Gestiona su agenda, disponibilidad e historial de pacientes |
| **Administrador** | Gestiona usuarios, roles, especialidades y catálogos del sistema |

### Alcance

✅ El sistema cubre:
- Gestión completa del ciclo de vida de citas médicas
- Gestión de disponibilidad de médicos
- Gestión de pacientes, médicos y especialidades
- Gestión de proveedores de salud
- Envío de notificaciones y recordatorios (Email, SMS, Push)
- Registro histórico y auditoría de operaciones

❌ Fuera de alcance:
- Facturación médica y pagos en línea
- Expedientes clínicos completos
- Diagnósticos automatizados o soporte médico con IA

---

## 🏗️ Arquitectura

MedAgenda está construido bajo una **Arquitectura en Capas (Layering)** con **Clean Architecture** en el backend, garantizando separación de responsabilidades, mantenibilidad y escalabilidad.

### Stack tecnológico

| Capa | Tecnología |
|---|---|
| Portal Web (Pacientes/Admin) | Next.js (React) |
| App de Escritorio (Médicos) | .NET MAUI |
| Backend API | .NET 8 Web API |
| Base de Datos | PostgreSQL (Supabase Cloud) |
| ORM | Entity Framework Core |
| Autenticación | JWT (JSON Web Token) |
| Tiempo Real | SignalR (WebSockets) |
| Notificaciones Email | SMTP |
| Notificaciones SMS | Twilio API |
| Caché | Redis |
| CI/CD | GitHub Actions |

### Estructura de la solución

```
SGC Solution
├── Core/
│   ├── SGC.Domain          ← Entidades, interfaces y reglas de negocio
│   └── SGC.Application     ← Servicios de casos de uso y DTOs
├── Infrastructure/
│   ├── SGC.Infrastructure  ← Email, SMS, SignalR Hubs
│   └── SGC.Persistence     ← EF Core, repositorios y migraciones
├── API/
│   └── SGC.API             ← Controllers, JWT Auth, Swagger
├── IOC/
│   └── SGC.IOC             ← Inyección de dependencias
├── Web/
│   ├── sgc.web.client      ← Next.js (portal paciente y panel admin)
│   └── SGC.Web.Server      ← BFF / Proxy, SSR, Middleware
├── Desktop/
│   └── SGC.Desktop         ← .NET MAUI (gestión médica)
└── ApplicationTest/
    └── SGC.ApplicationTest ← Pruebas unitarias e integración
```

### Capas del Backend

```
┌─────────────────────────────────┐
│     Capa de Presentación        │  Next.js + .NET MAUI
├─────────────────────────────────┤
│     Capa de API (Controllers)   │  .NET 8 Web API
├─────────────────────────────────┤
│     Capa de Aplicación          │  Servicios y casos de uso
├─────────────────────────────────┤
│     Capa de Dominio (Core)      │  Entidades y reglas de negocio
├─────────────────────────────────┤
│     Capa de Infraestructura     │  EF Core, Email, SMS, SignalR
└─────────────────────────────────┘
```

### Principios aplicados

- **Clean Architecture** — El núcleo del sistema es independiente de frameworks y tecnologías externas
- **Principio de Inversión de Dependencia (DIP)** — Las capas superiores dependen de abstracciones, no de implementaciones
- **Patrón Repository** — Abstracción del acceso a datos mediante interfaces definidas en el dominio
- **MVVM** — Patrón aplicado en la aplicación de escritorio (.NET MAUI)
- **MVC** — Patrón aplicado en el portal web (Next.js)

### Decisión sobre Supabase

Se utiliza **Supabase exclusivamente como proveedor de hosting para PostgreSQL**, sin uso de sus servicios de Auth ni Realtime. Esta decisión no afecta la integridad de la arquitectura limpia, ya que el acceso a datos se realiza mediante el patrón Repository. El core del sistema (`SGC.Domain`) permanece agnóstico al proveedor, cumpliendo con el Principio de Inversión de Dependencia.

### Comunicación en tiempo real

Se utiliza **SignalR** para:
- Actualización automática de disponibilidad médica en tiempo real
- Notificaciones push de nuevas citas al médico en la app de escritorio
- Sincronización de estados de citas entre plataformas

---

## 👥 Equipo

| Nombre | Matrícula | Responsabilidad |
|---|---|---|
| **Johan Vasquez** | 2025-1235 | SGC.Web (Next.js), SGC.API, Capas compartidas |
| **Gregori Evangelista** | 2025-1232 | SGC.Desktop (.NET MAUI), Capas compartidas |

---

## 📄 Documentación

| Documento | Descripción |
|---|---|
| SRS MedAgenda | Especificación de Requisitos de Software |
| SAD MedAgenda | Documento de Arquitectura de Software |

---

*ITLA — Instituto Tecnológico de las Américas · 2026*
