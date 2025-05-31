import React, { useState } from 'react';
import HeroBanner from './HeroBanner';
import TopRatedMovies from './TopRatedMovies';
import GenreFilter from './GenreFilter';
import NewReleases from './NewReleases';
import FeaturedActors from './FeaturedActors';
import LatestReviews from './LatestReviews';
import FeaturedDirectors from './FeaturedDirectors';

export default function FeaturesHome() {
  const [selectedGenre, setSelectedGenre] = useState(null);

  return (
    <main className="flex flex-col">
      {/* Hero Section */}
      <HeroBanner />

      {/* Discover Section */}
      <section className="bg-primary dark:bg-black py-12">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-heading text-white mb-6">Discover</h2>
          <TopRatedMovies />
        </div>
      </section>

      

      {/* New Releases Section */}
      <section className="bg-white dark:bg-primary py-12">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-heading text-primary dark:text-white mb-6">
            New Releases
          </h2>
          <NewReleases genre={selectedGenre} />
        </div>
      </section>

      {/* Inspiration Section */}
      <section className="bg-background dark:bg-primary py-12">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-heading text-primary dark:text-white mb-6">
            Inspiration
          </h2>
          <FeaturedActors />
        </div>
      </section>

       {/* Directors Picks Section */}
      <section className="bg-background dark:bg-primary py-12">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-heading text-primary dark:text-white mb-6">
            Directors Picks
          </h2>
          <FeaturedDirectors />
        </div>
      </section>

      {/* Community Picks Section */}
      <section className="bg-white dark:bg-primary py-12">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-heading text-primary dark:text-white mb-6">
            Community Picks
          </h2>
          <LatestReviews />
        </div>
      </section>
    </main>
  );
}