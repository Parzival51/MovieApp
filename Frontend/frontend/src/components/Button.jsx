import React from 'react';
import clsx from 'clsx';

const base =
  'inline-flex items-center justify-center font-medium rounded transition-colors focus:outline-none focus:ring-2 focus:ring-accent disabled:opacity-50';

const sizes = {
  sm: 'px-3 py-1 text-sm',
  md: 'px-4 py-2',
  lg: 'px-6 py-3 text-lg'
};

const variants = {
  primary:   'bg-secondary text-black hover:bg-secondary/90',
  secondary: 'bg-surface border border-muted hover:bg-muted/10 text-muted',
  danger:    'bg-danger text-white hover:bg-danger/90',
  ghost:     'bg-transparent text-secondary hover:bg-secondary/10'
};

/**
 * Custom Button component that can render as a native <button> or any other component (e.g. react-router-dom's Link).
 *
 * Usage:
 *   <Button variant="primary" size="md">Click me</Button>
 *   <Button as={Link} to="/path" variant="ghost" size="sm">Go</Button>
 */
export default function Button({
  as: Component = 'button',
  to,
  children,
  variant = 'primary',
  size = 'md',
  className = '',
  ...rest
}) {
  const cls = clsx(base, sizes[size], variants[variant], className);

  // Determine props for the rendered component
  const componentProps =
    Component === 'button'
      ? { type: rest.type ?? 'button' }
      : { to };

  return (
    <Component className={cls} {...componentProps} {...rest}>
      {children}
    </Component>
  );
}
