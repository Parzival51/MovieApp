import React from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.jsx';

createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    {/* Dark-mode wrapper */}
    <div className="dark">
      <App />
    </div>
  </React.StrictMode>
);

