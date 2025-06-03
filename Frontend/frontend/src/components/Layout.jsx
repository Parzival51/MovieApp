import React from 'react';
import Header from './Header';
import Footer from './Footer';


export default function Layout({ children }) {
  return (
    <div className="flex min-h-screen flex-col bg-background">
      <Header />

      {/* İçerik */}
      <main className="flex-1">
        {children}
      </main>

      <Footer />
    </div>
  );
}
