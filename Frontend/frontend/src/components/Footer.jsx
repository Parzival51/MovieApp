import React from 'react';
import { HiSun, HiMoon, HiGlobeAlt } from 'react-icons/hi';
import { useTheme } from '../context/ThemeContext';

export default function Footer() {
  const { mode, toggle } = useTheme();

  return (
    <footer className="bg-primary dark:bg-black text-gray-200 dark:text-gray-400 py-6">
      <div className="container mx-auto px-4 flex flex-col sm:flex-row items-center justify-between gap-4">
        {/* Telif */}
        <span>© {new Date().getFullYear()} MovieApp</span>

        {/* Sosyal placeholder (yalnız ikon) */}
        <a href="https://github.com/yourrepo" aria-label="GitHub" className="hover:text-secondary">
          <HiGlobeAlt size={20} />
        </a>

        {/* Tema düğmesi */}
        <button
          onClick={toggle}
          className="flex items-center space-x-1 hover:text-secondary focus:outline-none"
        >
          {mode === 'dark' ? <HiSun /> : <HiMoon />}
          <span className="hidden sm:inline text-sm">
            {mode === 'dark' ? 'Açık Mod' : 'Koyu Mod'}
          </span>
        </button>
      </div>
    </footer>
  );
}
