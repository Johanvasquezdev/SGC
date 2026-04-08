"use client";

import { Moon, Sun } from "lucide-react";
import { useEffect, useState } from "react";

export function ThemeToggle() {
  const [isDark, setIsDark] = useState(false);

  useEffect(() => {
    const stored = localStorage.getItem("theme");
    if (stored === "dark") {
      document.documentElement.classList.add("dark");
      setIsDark(true);
    } else if (stored === "light") {
      document.documentElement.classList.remove("dark");
      setIsDark(false);
    } else {
      document.documentElement.classList.add("dark");
      localStorage.setItem("theme", "dark");
      setIsDark(true);
    }

    const onStorage = () => {
      const stored = localStorage.getItem("theme");
      if (stored === "dark") {
        document.documentElement.classList.add("dark");
        setIsDark(true);
      } else if (stored === "light") {
        document.documentElement.classList.remove("dark");
        setIsDark(false);
      }
    };

    const onThemeChanged = () => {
      const active = document.documentElement.classList.contains("dark");
      setIsDark(active);
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
