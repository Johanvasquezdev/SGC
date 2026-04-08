# 🌐 Arquitectura del Frontend Web SGCM (MedAgenda)

![Next.js](https://img.shields.io/badge/Next.js-14-black?logo=next.js)
![React](https://img.shields.io/badge/React-18-blue?logo=react)
![TailwindCSS](https://img.shields.io/badge/TailwindCSS-38B2AC?logo=tailwind-css&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?logo=typescript&logoColor=white)

> Bienvenido a la documentación arquitectónica de la aplicación web cliente **MedAgenda**. Este documento detalla los patrones de diseño, flujos de integración y la estricta separación de responsabilidades implementada en la capa de presentación.

---

## 📐 Visión General de la Arquitectura

```text
┌─────────────────────────────────────────────────────────────┐
│                 CAPA DE PRESENTACIÓN (UI)                   │
│  (React Server & Client Components - src/app & components)  │
└──────────────────┬──────────────────────────────────────────┘
                   │ 1. Invoca métodos lógicos
┌──────────────────▼──────────────────────────────────────────┐
│                 CAPA DE SERVICIOS                           │
│  (Lógica de consumo HTTP - src/services)                    │
└──────────────────┬──────────────────────────────────────────┘
                   │ 2. Tipado estricto (DTOs - src/types)
┌──────────────────▼──────────────────────────────────────────┐
│             INTEGRACIÓN HTTP & SEGURIDAD                    │
│  (Instancia de Axios + Interceptor JWT - src/lib/api.ts)    │
└──────────────────┬──────────────────────────────────────────┘
                   │ 3. Petición HTTPS / JSON
┌──────────────────▼──────────────────────────────────────────┐
│             API REST BACKEND (.NET 8)                       │
└─────────────────────────────────────────────────────────────┘

