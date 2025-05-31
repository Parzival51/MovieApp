import React from 'react';
import clsx from 'clsx';

export default function Card({
  children,
  hover = true,
  padding = true,
  className = '',
  ...rest
}) {
  return (
    <div
      className={clsx(
        'bg-surface shadow rounded overflow-hidden',
        hover && 'hover:shadow-md transition-shadow',
        className
      )}
      {...rest}
    >
      <div className={padding ? 'p-4' : undefined}>{children}</div>
    </div>
  );
}
