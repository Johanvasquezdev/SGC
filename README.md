# MedAgenda - Sistema de Gestion de Citas Medicas (SGCM)

Proyecto academico para Programacion II (ITLA)
Profesor: Francis Ramirez

## Nota importante sobre ramas
Las ramas sin matricula fueron trabajadas en conjunto por Johan y Gregori.

## Descripcion del proyecto
MedAgenda es un sistema para gestionar el ciclo de vida de las citas medicas. Centraliza pacientes, medicos y citas, reduciendo errores, duplicidad y mala comunicacion.

### Problema que resuelve
- Conflictos de horarios y doble asignacion de citas
- Falta de notificaciones oportunas
- Ausencia de trazabilidad y control centralizado
- Carga administrativa excesiva

### Actores
- Paciente: busca medicos, programa, consulta y cancela citas
- Medico: gestiona agenda, disponibilidad e historial de pacientes
- Administrador: gestiona usuarios, roles, especialidades y catalogos

### Alcance
Incluye:
- Gestion completa del ciclo de vida de citas
- Disponibilidad de medicos
- Gestion de pacientes, medicos y especialidades
- Gestion de proveedores de salud
- Notificaciones y recordatorios (Email, SMS, Push)
- Auditoria

Fuera de alcance:
- Expedientes clinicos completos

## Arquitectura
Arquitectura en capas con Clean Architecture en el backend.

### Stack tecnologico
- Backend: .NET 8 Web API
- Web: Next.js (React)
- Desktop: .NET MAUI
- DB: PostgreSQL (Supabase solo hosting)
- ORM: Entity Framework Core
- Auth: JWT
- Real-time: SignalR
- Pagos: Stripe
- Chatbot: Claude (Anthropic)
- Cache: Redis
- Notificaciones: SMTP (MailKit) y Twilio
- Logging: Serilog via ISGCLogger

### Estructura de la solucion
SGC Solution/
- SGC.Domain
- SGC.Application
- SGC.Persistence
- SGC.Infraestructure
- SGC.IOC
- SGC.API
- SGC.Web (Next.js)
- SGC.Desktop (MAUI)
- SGC.ApplicationTest

### Capas del backend
- API: Controllers y middleware
- Application: Servicios, DTOs, Mappers
- Domain: Entidades, reglas, interfaces
- Persistence: EF Core y repositorios
- Infrastructure: Email, SMS, Stripe, Redis, SignalR, Claude
- IOC: DependencyContainer

## Division de trabajo (para evaluacion)
**Johan Vasquez (2025-1235)**
Portal Paciente:
- Registro y Login
- Busqueda de Medicos
- Agendamiento de Citas
- Mis Citas
- Pagos
- Notificaciones
- Chatbot

Panel Administrador:
- Gestion de Usuarios
- Gestion de Medicos
- Gestion de Especialidades
- Gestion de Proveedores
- Auditoria

Capas y entregables:
- SGC.Web (Next.js)
- 6 Controllers API
- DTOs y Mappers

**Gregori Evangelista (2025-1232)**
App Medico:
- Agenda Medica
- Gestion de Citas
- Disponibilidad
- Historial de Pacientes
- Seguridad JWT

Capas y entregables:
- SGC.Desktop (.NET MAUI)
- 6 Controllers API
- Middleware
- Infrastructure
- ApplicationTest
- Program.cs
- Contracts

## Ramas por modulo
- feature/web-20251235 (Web - Johan)
- feature/desktop-20251232 (Desktop - Gregori)
- feature/api, feature/ioc, feature/infraestructure, feature/dominio, feature/persistencia, feature/application (trabajo conjunto)

## Documentacion
- SRS MedAgenda
- SAD MedAgenda

ITLA - 2026