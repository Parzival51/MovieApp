// MovieApp.UnitTests/Repositories/MovieRepositoryTests.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieApp.DataAccess.Context;
using MovieApp.DataAccess.Concrete;
using MovieApp.Entity.Entities;
using Xunit;

namespace MovieApp.UnitTests.Repositories
{
    public class MovieRepositoryTests : IDisposable
    {
        private readonly MovieAppDbContext _context;
        private readonly MovieRepository _repository;

        public MovieRepositoryTests()
        {
            // 1) InMemory DbContext ayarı
            var options = new DbContextOptionsBuilder<MovieAppDbContext>()
                .UseInMemoryDatabase(databaseName: $"MovieDb_{Guid.NewGuid()}")
                .Options;

            _context = new MovieAppDbContext(options);

            // 2) Örnek veriyi ekle
            SeedDatabase();

            // 3) Repository örneği
            _repository = new MovieRepository(_context);
        }

        private void SeedDatabase()
        {
            // İki film ekleyelim; her birine farklı Ratings listesi verelim
            var movie1 = new Movie
            {
                Id = Guid.NewGuid(),
                ExternalId = 100,
                Title = "Alpha",
                ReleaseDate = new DateTime(2020, 1, 1),
                Duration = 120,
                Language = "en",
                Country = "US",
                Ratings = new List<Rating>
                {
                    new Rating { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 8 },
                    new Rating { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 6 }
                }
            };

            var movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                ExternalId = 200,
                Title = "Beta",
                ReleaseDate = new DateTime(2021, 1, 1),
                Duration = 100,
                Language = "en",
                Country = "US",
                Ratings = new List<Rating>
                {
                    new Rating { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 9 },
                    new Rating { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 9 }
                }
            };

            _context.Movies.AddRange(movie1, movie2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTopRatedMoviesAsync_WhenCalledWithCount_ReturnsMoviesOrderedByAverageScore()
        {
            // Arrange
            int count = 1; // Sadece en yüksek ortalamaya sahip film gelecek

            // Act
            var result = (await _repository.GetTopRatedMoviesAsync(count)).ToList();

            // Böylece movie2: ortalama (9 + 9) / 2 = 9.0
            // movie1: ortalama (8 + 6) / 2 = 7.0 
            // count=1 → sadece "Beta" dönecek
            result.Should().HaveCount(1);
            result.First().Title.Should().Be("Beta");
        }

        [Fact]
        public async Task GetSimilarMoviesAsync_WhenGivenExistingMovie_ReturnsOtherMoviesWithSameGenre()
        {
            // Bu testte “Genre” tablosu ilişkili olarak yok, 
            // bu nedenle MovieGenre üzerinden manuel ilişki kuracağız.

            // 1) Yeni bir film ekleyelim; “Alpha” ile aynı tür (GenreId) kullanalım
            var sharedGenreId = Guid.NewGuid();

            // “Alpha” filmine de aynı genreId’yi iliştir
            var alphaMovie = _context.Movies.First(m => m.Title == "Alpha");
            alphaMovie.MovieGenres = new List<MovieGenre>
            {
                new MovieGenre { GenreId = sharedGenreId }
            };

            var extraMovie = new Movie
            {
                Id = Guid.NewGuid(),
                ExternalId = 300,
                Title = "Gamma",
                ReleaseDate = new DateTime(2019, 1, 1),
                Duration = 110,
                Language = "en",
                Country = "US",
                MovieGenres = new List<MovieGenre>
                {
                    new MovieGenre { GenreId = sharedGenreId }
                },
                Ratings = new List<Rating>()
            };

            _context.Movies.Add(extraMovie);
            _context.SaveChanges();

            // Act
            var similar = (await _repository.GetSimilarMoviesAsync(alphaMovie.Id, max: 5)).ToList();

            // Assert
            // “Alpha” hariç, aynı genreId’yi paylaşan “Gamma” dönmüş olacak.
            similar.Should().Contain(m => m.Title == "Gamma");
            similar.Should().NotContain(m => m.Title == "Alpha");
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
