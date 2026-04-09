"use client";

import { useEffect } from "react";
import * as signalR from "@microsoft/signalr";

interface UseSignalROptions {
  hubUrl: string;
  onNuevaCita?: (payload: unknown) => void;
}

export function useSignalR({ hubUrl, onNuevaCita }: UseSignalROptions) {
  useEffect(() => {
    const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5189";
    const normalizedBase = baseUrl.replace(/\/$/, "");
    const rawHubUrl = hubUrl || `${normalizedBase}/citahub`;
    const resolvedHubUrl =
      rawHubUrl.replace("http://localhost:5189", "https://localhost:7224")
               .replace("http://127.0.0.1:5189", "https://127.0.0.1:7224");

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(resolvedHubUrl, {
        accessTokenFactory: () =>
          (typeof window !== "undefined" && localStorage.getItem("medagenda_token")) || "",
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    if (onNuevaCita) {
      connection.on("NuevaCita", onNuevaCita);
    }

    connection.start().catch(() => {});

    return () => {
      if (onNuevaCita) {
        connection.off("NuevaCita", onNuevaCita);
      }
      connection.stop();
    };
  }, [hubUrl, onNuevaCita]);
}
