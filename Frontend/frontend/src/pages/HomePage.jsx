// src/pages/HomePage.jsx
import React from 'react';
import Header from '../components/Header';
import Footer from '../components/Footer';
import FeaturesHome from '../features/home/FeaturesHome';

export default function HomePage() {
  return (
    <div className="min-h-screen flex flex-col bg-background dark:bg-primary">
      <Header />

      <main className="flex-1">
        <FeaturesHome />
      </main>

      <Footer />
    </div>
  );
}
