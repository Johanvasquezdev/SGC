"use client";

import { usePathname } from "next/navigation";
import { ReactNode, useEffect, useState } from "react";

type PageTransitionProps = {
  children: ReactNode;
};

export function PageTransition({ children }: PageTransitionProps) {
  const pathname = usePathname();
  const [isTransitioning, setIsTransitioning] = useState(false);
  const [displayChildren, setDisplayChildren] = useState(children);

  useEffect(() => {
    setIsTransitioning(true);

    const timeoutId = window.setTimeout(() => {
      setDisplayChildren(children);
      setIsTransitioning(false);
    }, 150);

    return () => window.clearTimeout(timeoutId);
  }, [pathname, children]);

  return (
    <div
      className={`transition-all duration-300 ease-out motion-reduce:transition-none ${
        isTransitioning ? "translate-y-2 opacity-0" : "translate-y-0 opacity-100"
      }`}
    >
      {displayChildren}
    </div>
  );
}
