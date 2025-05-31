/** @type {import('tailwindcss').Config} */
export default {
  darkMode: 'class',

  content: [
    './index.html',
    './src/**/*.{js,jsx,ts,tsx}'
  ],

  theme: {
    extend: {
      /* ----------  DESIGN TOKENS (CSS vars) ---------- */
      colors: {
        /* renkler CSS değişkenlerinden okunur;
           <alpha-value> Tailwind’in JIT sentaksı içindir */
        primary:  'rgb(var(--color-primary) / <alpha-value>)',
        background:'rgb(var(--color-background) / <alpha-value>)',
        surface:  'rgb(var(--color-surface) / <alpha-value>)',
        secondary:'rgb(var(--color-secondary) / <alpha-value>)',
        accent:   'rgb(var(--color-accent) / <alpha-value>)',
        success:  'rgb(var(--color-success) / <alpha-value>)',
        danger:   'rgb(var(--color-danger) / <alpha-value>)',

        /* Nötr ton ‒ metin & sınır için */
        muted: {
          DEFAULT: 'rgb(var(--color-muted) / <alpha-value>)'
        }
      },

      borderRadius: {
        sm: 'var(--radius-sm)',
        DEFAULT: 'var(--radius-md)',
        full: 'var(--radius-full)'
      },

      boxShadow: {
        sm: 'var(--shadow-sm)',
        DEFAULT: 'var(--shadow-md)'
      },

      fontFamily: {
        sans:    ['Inter', 'sans-serif'],
        heading: ['Poppins', 'sans-serif']
      }
    }
  },

  plugins: []
};
