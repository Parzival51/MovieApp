import React, { useEffect, useRef } from 'react';
import ReactDOM from 'react-dom';
import clsx from 'clsx';

const FOCUSABLE = [
  'a[href]', 'area[href]', 'input:not([disabled])', 'select:not([disabled])',
  'textarea:not([disabled])', 'button:not([disabled])', '[tabindex]:not([tabindex="-1"])'
].join(',');


function getFocusables(node) {
  if (!node) return [];
  return Array.from(node.querySelectorAll(FOCUSABLE)).filter(el => el.offsetParent !== null);
}


export default function Modal({
  open,
  onClose,
  className = '',
  children,
  labelledById,
  ariaLabel,      
}) {
  const panelRef = useRef(null);
  const previousFocus = useRef(null);

  useEffect(() => {
    if (!open) return;
    const handleKey = e => {
      if (e.key === 'Escape') {
        e.stopPropagation();
        onClose();
      }
      if (e.key === 'Tab') {
        const focusables = getFocusables(panelRef.current);
        if (focusables.length === 0) return;
        const first = focusables[0];
        const last  = focusables[focusables.length - 1];

        if (e.shiftKey && document.activeElement === first) {
          e.preventDefault();
          last.focus();
        } else if (!e.shiftKey && document.activeElement === last) {
          e.preventDefault();
          first.focus();
        }
      }
    };
    document.addEventListener('keydown', handleKey);
    return () => document.removeEventListener('keydown', handleKey);
  }, [open, onClose]);


  useEffect(() => {
    if (!open) return;
    previousFocus.current = document.activeElement;
    const t = setTimeout(() => {
      const focusables = getFocusables(panelRef.current);
      (focusables[0] || panelRef.current).focus();
    });
    return () => {
      clearTimeout(t);
      previousFocus.current?.focus?.();
    };
  }, [open]);


  if (!open) return null;
  return ReactDOM.createPortal(
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm"
      role="dialog"
      aria-modal="true"
      aria-labelledby={labelledById}
      aria-label={labelledById ? undefined : ariaLabel}
      onMouseDown={e => {
        if (e.target === e.currentTarget) onClose();
      }}
    >
      <div
        ref={panelRef}
        tabIndex={-1}
        className={clsx('bg-surface rounded shadow-md p-6 w-[22rem] max-w-full', className)}
      >
        {children}
      </div>
    </div>,
    document.body
  );
}
