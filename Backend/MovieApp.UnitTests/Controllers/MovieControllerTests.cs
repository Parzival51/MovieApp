// MovieApp.UnitTests/Controllers/MovieControllerTests.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.API.Controllers;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using Xunit;

namespace MovieApp.UnitTests.Controllers
{
    public class MovieControllerTests
    {
        
        private IMapper BuildMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Movie, MovieListDto>()
                   .ForMember(d => d.AverageRating, opt => opt.MapFrom(src =>
                       src.Ratings != null && src.Ratings.Count > 0
                           ? src.Ratings.Average(r => (double)r.Score10)
                           : 0.0));
                cfg.CreateMap<Movie, MovieDetailDto>().IncludeBase<Movie, MovieListDto>();
                cfg.CreateMap<CreateMovieDto, Movie>();
                cfg.CreateMap<UpdateMovieDto, Movie>();
            });

            return config.CreateMapper();
        }

        [Fact]
        public async Task GetAll_ReturnsOkObjectResult_WithListOfMovies()
        {
            // Arrange
            var mockMovieSvc = new Mock<IMovieService>();
            mockMovieSvc
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Movie>
                {
                    new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = "Film A",
                        ReleaseDate = DateTime.Today,
                        PosterUrl = "http://example.com/a.jpg",
                        Ratings = new List<Rating>
                        {
                            new Rating { Score10 = 8 },
                            new Rating { Score10 = 9 }
                        }
                    },
                    new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = "Film B",
                        ReleaseDate = DateTime.Today,
                        PosterUrl = "http://example.com/b.jpg",
                        Ratings = new List<Rating>() 
                    }
                });

            var mapper = BuildMapper();
            var controller = new MoviesController(mockMovieSvc.Object, mapper);

            // Act
            var actionResult = await controller.GetAll(); 

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            var ok = actionResult as OkObjectResult;
            ok.Value.Should().BeAssignableTo<IEnumerable<MovieListDto>>();

            var list = ok.Value as IEnumerable<MovieListDto>;
            list.Should().HaveCount(2);

            var first = Assert.IsType<MovieListDto>(ok.Value.As<IEnumerable<MovieListDto>>()?.FirstOrDefault());
            list.Should().Contain(m => m.AverageRating == 8.5);
        }

        [Fact]
        public async Task GetTopRated_WithDefaultCount_ReturnsOkObjectResult_WithExpectedMovies()
        {
            var mockMovieSvc = new Mock<IMovieService>();
            mockMovieSvc
                .Setup(s => s.GetTopRatedMoviesAsync(5))
                .ReturnsAsync(new List<Movie>
                {
                    new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = "TopFilm",
                        ReleaseDate = DateTime.Today,
                        PosterUrl = "http://example.com/top.jpg",
                        Ratings = new List<Rating>
                        {
                            new Rating { Score10 = 10 },
                            new Rating { Score10 = 9 }
                        }
                    }
                });

            var mapper = BuildMapper();
            var controller = new MoviesController(mockMovieSvc.Object, mapper);

            var actionResult = await controller.GetTopRated(); 

            actionResult.Should().BeOfType<OkObjectResult>();
            var ok = actionResult as OkObjectResult;
            ok.Value.Should().BeAssignableTo<IEnumerable<MovieListDto>>();

            var list = ok.Value as IEnumerable<MovieListDto>;
            list.Should().ContainSingle(m => m.Title == "TopFilm");
        }

        [Fact]
        public async Task GetById_WhenNotFound_ReturnsNotFoundResult()
        {
            
            var id = Guid.NewGuid();
            var mockMovieSvc = new Mock<IMovieService>();
            mockMovieSvc
                .Setup(s => s.GetMovieWithDetailsAsync(id))
                .ReturnsAsync((Movie?)null); 

            var mapper = BuildMapper();
            var controller = new MoviesController(mockMovieSvc.Object, mapper);

            var actionResult = await controller.GetById(id);

            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_WhenFound_ReturnsOkObjectResult_WithMovieDetailDto()
        {
            var id = Guid.NewGuid();
            var sampleMovie = new Movie
            {
                Id = id,
                Title = "Bulunan Film",
                ReleaseDate = DateTime.Today,
                PosterUrl = "http://example.com/found.jpg",
                Description = "Açıklama",
                Duration = 120,
                Language = "en",
                Country = "US",
                Tagline = "Bir Tagline",
                Status = "Released",
                Budget = 1000000,
                Revenue = 500000,
                ImdbId = "tt1234567",
                HomepageUrl = "http://example.com",
                IsAdult = false,
                BackdropPath = "http://example.com/bg.jpg",
                Ratings = new List<Rating>
                {
                    new Rating { Score10 = 7 },
                    new Rating { Score10 = 8 }
                },
                MovieGenres = new List<MovieGenre>(),
                MovieActors = new List<MovieActor>(),
                MovieDirectors = new List<MovieDirector>(),
                MovieLanguages = new List<MovieLanguage>(),
                Images = new List<MovieImage>()
            };

            var mockMovieSvc = new Mock<IMovieService>();
            mockMovieSvc
                .Setup(s => s.GetMovieWithDetailsAsync(id))
                .ReturnsAsync(sampleMovie);

            var mapper = BuildMapper();
            var controller = new MoviesController(mockMovieSvc.Object, mapper);

            // Act
            var actionResult = await controller.GetById(id);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            var ok = actionResult as OkObjectResult;
            ok.Value.Should().BeOfType<MovieDetailDto>();

            var detail = ok.Value as MovieDetailDto;
            detail.Id.Should().Be(id);
            detail.Title.Should().Be("Bulunan Film");
            detail.AverageRating.Should().BeApproximately((7.0 + 8.0) / 2.0, 0.001);
        }

    
    }
}
