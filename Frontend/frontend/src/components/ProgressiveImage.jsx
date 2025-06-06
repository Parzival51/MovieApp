import React, { useState } from 'react';
import clsx from 'clsx';


export default function ProgressiveImage({ src, alt = '', className = '' }) {
  const [loaded, setLoaded] = useState(false);

  return (
    <img
      src={src}
      alt={alt}
      loading="lazy"
      onLoad={() => setLoaded(true)}
      className={clsx(
        'bg-muted/10 transition-all duration-500',
        !loaded && 'blur-md scale-105',
        loaded && 'blur-0 scale-100',
        className
      )}
    />
  );
}
