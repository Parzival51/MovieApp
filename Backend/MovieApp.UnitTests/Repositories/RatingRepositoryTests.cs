// MovieApp.UnitTests/Repositories/RatingRepositoryTests.cs

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
    public class RatingRepositoryTests : IDisposable
    {
        private readonly MovieAppDbContext _context;
        private readonly RatingRepository _repository;

        public RatingRepositoryTests()
        {
            // 1) InMemory DbContext ayarı
            var options = new DbContextOptionsBuilder<MovieAppDbContext>()
                .UseInMemoryDatabase(databaseName: $"RatingDb_{Guid.NewGuid()}")
                .Options;

            _context = new MovieAppDbContext(options);

            // 2) Seed aşamasında hem User hem de Rating verilerini ekliyoruz
            SeedDatabase();

            // 3) Repository örneği
            _repository = new RatingRepository(_context);
        }

        private void SeedDatabase()
        {
            // --- 2a) Önce User tablosuna kayıt ekleyelim ---
            var movieId1 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            var user1 = new User
            {
                Id = userId1,
                UserName = "user1",
                DisplayName = "User One",
                Email = "user1@example.com",
                // Aşağıdaki iki alan InMemory verisinde pek kullanılmıyor, 
                // ama IdentityDbContext yapısında “Required” oldukları için dolduruyoruz:
                PasswordHash = null,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var user2 = new User
            {
                Id = userId2,
                UserName = "user2",
                DisplayName = "User Two",
                Email = "user2@example.com",
                PasswordHash = null,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            _context.Users.AddRange(user1, user2);

            // --- 2b) Şimdi Rating verilerini ekleyelim ---
            // (İki tanesi aynı movieId1'e, biri farklı bir filme ait)
            var rating1 = new Rating
            {
                Id = Guid.NewGuid(),
                MovieId = movieId1,
                UserId = userId1,
                Score10 = 5
                // Erken tarihe sabitlenmeye gerek yok, EF Core InMemory zaten DateTime.UtcNow ayarlamıyor default olarak
            };
            var rating2 = new Rating
            {
                Id = Guid.NewGuid(),
                MovieId = movieId1,
                UserId = userId2,
                Score10 = 7
            };
            var rating3 = new Rating
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),  // bu satır başka bir filme ait
                UserId = userId1,
                Score10 = 9
            };

            _context.Ratings.AddRange(rating1, rating2, rating3);

            // --- 2c) Veritabanına kaydet ---
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetByMovieAndUserAsync_WhenCalledWithExisting_ReturnsRating()
        {
            // Arrange
            // SeedDatabase() içinde movieId1,userId1 ile Score10=5 şeklinde koymuştuk
            var existing = _context.Ratings
                                   .Include(r => r.User)
                                   .First(r => r.Score10 == 5);

            // Act
            var result = await _repository.GetByMovieAndUserAsync(existing.MovieId, existing.UserId);

            // Assert
            result.Should().NotBeNull();                      // null olmamalı
            result.Id.Should().Be(existing.Id);               // doğru kaydı bulmalı
            result.Score10.Should().Be(existing.Score10);     // Score10 = 5
            result.User.Should().NotBeNull();                  // Include işlemi, User nav-prop'unu doldurmalı
            result.User.UserName.Should().Be("user1");        // user1 bilgisi gelmiş olmalı
        }

        [Fact]
        public async Task GetByMovieAndUserAsync_WhenNoMatch_ReturnsNull()
        {
            // Arrange
            var nonExistingMovieId = Guid.NewGuid();
            var someUserId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByMovieAndUserAsync(nonExistingMovieId, someUserId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByMovieAsync_WhenCalledWithMovieId_ReturnsAllRatingsForThatMovie()
        {
            // Arrange
            // SeedDatabase'da movieId1 için 2 kayıt eklemiştik
            var movieId = _context.Ratings.First().MovieId;

            // Act
            var list = (await _repository.GetByMovieAsync(movieId)).ToList();

            // Assert
            list.Should().HaveCount(2);                       // tam 2 tane rating dönmeli
            list.All(r => r.MovieId == movieId).Should().BeTrue();
            // Bu iki rating’in User nav-prop’unun da gelmiş olduğunu teyit edebiliriz:
            list.ForEach(r => r.User.Should().NotBeNull());
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
