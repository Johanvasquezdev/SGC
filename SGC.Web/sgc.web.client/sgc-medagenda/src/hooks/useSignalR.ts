"use client";

import { useEffect } from "react";
import * as signalR from "@microsoft/signalr";

interface UseSignalROptions {
  hubUrl: string;
  onNuevaCita?: (payload: unknown) => void;
}

export function useSignalR({ hubUrl, onNuevaCita }: UseSignalROptions) {
  useEffect(() => {
    // Inicializa la conexion con reconexion automatica.
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
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
