import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    https: false,   // ← veya satırı tamamen silin
    port: 5173
  }
});
