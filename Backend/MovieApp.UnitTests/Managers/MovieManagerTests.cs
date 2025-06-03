// MovieApp.UnitTests/Managers/MovieManagerTests.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using MovieApp.Business.Concrete;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using Xunit;

namespace MovieApp.UnitTests.Managers
{
    public class MovieManagerTests : IDisposable
    {
        private readonly Mock<IMovieRepository> _mockRepo;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly IMapper _mapper;
        private readonly MovieManager _manager;

        public MovieManagerTests()
        {
            // ---------------- 1) AutoMapper configuration ----------------
            var config = new MapperConfiguration(cfg =>
            {
                // Movie -> MovieListDto
                cfg.CreateMap<Movie, MovieListDto>()
                   .ForMember(
                       dest => dest.AverageRating,
                       opt => opt.MapFrom(src => src.Ratings != null && src.Ratings.Count > 0
                           ? src.Ratings.Average(r => r.Score10)
                           : 0.0)
                    );
                // MovieListDto -> SearchResultDto<MovieListDto> map'ı otomatik
                // sağlanır (generic yapıda ek bir ayar gerekmez).
            });
            _mapper = config.CreateMapper();

            // ---------------- 2) IMovieRepository mock ----------------
            _mockRepo = new Mock<IMovieRepository>();

            // ---------------- 3) IMemoryCache mock ----------------
            _mockCache = new Mock<IMemoryCache>();

            // MovieManager örneğini oluşturuyoruz:
            _manager = new MovieManager(
                _mockRepo.Object,
                /* httpContextAccessor: */ null,
                _mapper,
                _mockCache.Object
            );
        }

        [Fact]
        public async Task SearchAsync_WithTermAndCache_ReturnsExpectedSearchResult()
        {
            // Arrange
            var searchTerm = "Star";
            int page = 1, pageSize = 10;
            var cacheKey = $"search:{searchTerm}:{page}:{pageSize}";

            // 1) Repository’den dönecek örnek SearchResultDto<MovieListDto>
            var repoResult = new SearchResultDto<MovieListDto>
            {
                Total = 2,
                Items = new List<MovieListDto>
                {
                    new MovieListDto
                    {
                        Id = Guid.NewGuid(),
                        Title = "Star Wars",
                        ReleaseDate = new DateTime(1977, 5, 25),
                        PosterUrl = "url1",
                        AverageRating = 8.5
                    },
                    new MovieListDto
                    {
                        Id = Guid.NewGuid(),
                        Title = "Star Trek",
                        ReleaseDate = new DateTime(1979, 12, 7),
                        PosterUrl = "url2",
                        AverageRating = 7.2
                    }
                }
            };

            // 2) İlk önce cache.TryGetValue(...) false dönecek
            object cacheEntry;
            _mockCache
                .Setup(c => c.TryGetValue(cacheKey, out cacheEntry))
                .Returns(false);

            // 3) Repository.SearchAsync çağrısı repoResult’u dönecek
            _mockRepo
                .Setup(r => r.SearchAsync(searchTerm, page, pageSize))
                .ReturnsAsync(repoResult);

            // 4) Manager içindeki cache.CreateEntry(...) metodu gerçekmiş gibi dönsün
            var mockCacheEntry = new Mock<ICacheEntry>();
            _mockCache
                .Setup(c => c.CreateEntry(cacheKey))
                .Returns(mockCacheEntry.Object);

            // Act
            var result = await _manager.SearchAsync(searchTerm, page, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(2);
            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(i => i.Title == "Star Wars");
            result.Items.Should().Contain(i => i.Title == "Star Trek");

            // Repository.SearchAsync en az bir kez çağrılmış mı?
            _mockRepo.Verify(r => r.SearchAsync(searchTerm, page, pageSize), Times.Once);

            // Cache.CreateEntry(cacheKey) en az bir kez çağrılmış mı?
            _mockCache.Verify(c => c.CreateEntry(cacheKey), Times.Once);
        }

        [Fact]
        public async Task SuggestAsync_WhenCalled_ReturnsListOfSuggestions()
        {
            // Arrange
            var prefix = "God";
            var mockMovies = new List<Movie>
            {
                new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Godfather",
                    ReleaseDate = new DateTime(1972, 3, 24),
                    Duration = 175,
                    Language = "en",
                    Country = "US",
                    Ratings = new List<Rating>()
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Godzilla",
                    ReleaseDate = new DateTime(2014, 5, 16),
                    Duration = 123,
                    Language = "en",
                    Country = "JP",
                    Ratings = new List<Rating>()
                }
            };

            // Repository’den “SuggestAsync” listesi gelsin
            _mockRepo
                .Setup(r => r.SuggestAsync(prefix, 5))
                .ReturnsAsync(mockMovies);

            // Act
            var suggestions = await _manager.SuggestAsync(prefix, 5);

            // Assert
            suggestions.Should().HaveCount(2);
            suggestions.Should().Contain(s => s.Title == "Godfather");
            suggestions.Should().Contain(s => s.Title == "Godzilla");
        }

        public void Dispose()
        {
            // Eğer başka IDisposable nesneler olsaydı burada temizleyebilirdik.
        }
    }
}
