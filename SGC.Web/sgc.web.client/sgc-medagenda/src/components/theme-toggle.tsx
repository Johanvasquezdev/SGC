"use client";

import { Moon, Sun } from "lucide-react";
import { useEffect, useState, useSyncExternalStore } from "react";

export function ThemeToggle() {
  const [isDark, setIsDark] = useState(() => {
    if (typeof window === "undefined") return true;
    const stored = localStorage.getItem("theme");
    if (stored === "light") return false;
    return true;
  });

  const mounted = useSyncExternalStore(
    () => () => undefined,
    () => true,
    () => false
  );

  useEffect(() => {
    const stored = localStorage.getItem("theme");
    const nextIsDark = stored ? stored === "dark" : true;

    document.documentElement.classList.toggle("dark", nextIsDark);
    document.body.classList.toggle("dark", nextIsDark);
    document.documentElement.style.colorScheme = nextIsDark ? "dark" : "light";

    if (!stored) {
      localStorage.setItem("theme", nextIsDark ? "dark" : "light");
    }

    const onStorage = () => {
      const stored = localStorage.getItem("theme");
      const next = stored === "dark";
      document.documentElement.classList.toggle("dark", next);
      document.body.classList.toggle("dark", next);
      document.documentElement.style.colorScheme = next ? "dark" : "light";
      queueMicrotask(() => setIsDark(next));
    };

    const onThemeChanged = () => {
      const active = document.documentElement.classList.contains("dark");
      queueMicrotask(() => setIsDark(active));
    };

    window.addEventListener("storage", onStorage);
    window.addEventListener("theme-changed", onThemeChanged);
    return () => {
      window.removeEventListener("storage", onStorage);
      window.removeEventListener("theme-changed", onThemeChanged);
    };
  }, []);

  const toggleTheme = () => {
    const currentlyDark = document.documentElement.classList.contains("dark");
    const next = !currentlyDark;
    document.documentElement.classList.toggle("dark", next);
    document.body.classList.toggle("dark", next);
    document.documentElement.style.colorScheme = next ? "dark" : "light";
    localStorage.setItem("theme", next ? "dark" : "light");
    setIsDark(next);
    window.dispatchEvent(new Event("theme-changed"));
  };

  if (!mounted) {
    return (
      <button
        className="relative p-2 rounded-xl bg-card border border-border hover:bg-accent text-foreground"
        aria-label="Cambiar tema"
        disabled
      >
        <Sun className="w-5 h-5" />
      </button>
    );
  }

  return (
    <button
      onClick={toggleTheme}
      className="relative p-2 rounded-xl bg-card border border-border hover:bg-accent text-foreground"
      aria-label="Cambiar tema"
    >
      {isDark ? <Sun className="w-5 h-5" /> : <Moon className="w-5 h-5" />}
    </button>
  );
}
