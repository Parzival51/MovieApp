import React, { createContext, useState, useCallback, useContext } from 'react';
import { motion, useReducedMotion } from 'framer-motion';

const NotificationContext = createContext();

export function NotificationProvider({ children }) {
  const [notifications, setNotifications] = useState([]);
  const shouldReduce = useReducedMotion();

  const notify = useCallback((type, message, duration = 3000) => {
    const id = Date.now().toString();
    setNotifications(prev => [...prev, { id, type, message }]);
    setTimeout(
      () => setNotifications(prev => prev.filter(n => n.id !== id)),
      duration
    );
  }, []);

  return (
    <NotificationContext.Provider value={{ notify }}>
      {children}

      {/* Toast container */}
      <div
        className="fixed bottom-4 right-4 space-y-2 z-50"
        aria-live="assertive"
        role="alert"
      >
        {notifications.map(n => (
          <motion.div
            key={n.id}
            initial={!shouldReduce ? { opacity: 0, translateY: 20 } : false}
            animate={!shouldReduce ? { opacity: 1, translateY: 0 } : false}
            exit={!shouldReduce ? { opacity: 0, translateY: 20 } : false}
            transition={{ duration: 0.25 }}
            className={`
              px-4 py-2 rounded shadow text-white
              ${n.type === 'error'   ? 'bg-red-600'    : ''}
              ${n.type === 'success' ? 'bg-green-600'  : ''}
              ${n.type === 'info'    ? 'bg-blue-600'   : ''}
            `}
          >
            {n.message}
          </motion.div>
        ))}
      </div>
    </NotificationContext.Provider>
  );
}

export function useNotification() {
  return useContext(NotificationContext);
}
