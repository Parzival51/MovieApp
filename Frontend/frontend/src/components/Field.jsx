import React from 'react';
import clsx from 'clsx';

/**
 * <Field label="E-posta" error="Zorunlu alan">
 *   <input ... />
 * </Field>
 *
 * Props: label (zorunlu), error?, className?
 */
export default function Field({ label, error, children, className = '' }) {
  return (
    <div className={clsx('space-y-1', className)}>
      <label className="block text-sm font-medium text-muted">{label}</label>

      {/* Girdi */}
      {children}

      {/* Hata */}
      {error && (
        <p className="text-xs text-danger">{error}</p>
      )}
    </div>
  );
}
