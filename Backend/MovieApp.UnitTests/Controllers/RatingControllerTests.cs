// MovieApp.UnitTests/Controllers/RatingControllerTests.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.API.Controllers;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.RatingDtos;
using MovieApp.Entity.Entities;
using Xunit;

namespace MovieApp.UnitTests.Controllers
{
    public class RatingControllerTests
    {
        /// <summary>
        /// Basit bir AutoMapper konfigürasyonu oluşturuyoruz. 
        /// Rating <--> CreateRatingDto, Rating <--> RatingDetailDto eşlemelerini ekliyoruz.
        /// </summary>
        private IMapper BuildMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateRatingDto, Rating>();
                cfg.CreateMap<Rating, RatingListDto>();
                cfg.CreateMap<Rating, RatingDetailDto>().IncludeBase<Rating, RatingListDto>();
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// Sahte HttpContextAccessor oluştururuz; "User" bilgisi gerektiği zaman kullanılacak.
        /// Bu testlerde “mine” parametresiyle ilgili bir test yok, dolayısıyla boş bırakıyoruz.
        /// </summary>
        private IHttpContextAccessor BuildHttpContextAccessor()
        {
            // Burada sadece bir ClaimsPrincipal atıyoruz. Teste özgü “mine” testi yapılacaksa,
            // uygun ClaimTypes.NameIdentifier içeren bir ClaimsPrincipal tanımlanmalı.
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            }, "mock"));

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.User).Returns(user);

            var mockAccessor = new Mock<IHttpContextAccessor>();
            mockAccessor.SetupGet(x => x.HttpContext).Returns(mockHttpContext.Object);

            return mockAccessor.Object;
        }

        [Fact]
        public async Task Get_WithMovieId_ReturnsOkWithFilteredRatings()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            var ratings = new List<Rating>
            {
                new Rating { Id = Guid.NewGuid(), MovieId = movieId, UserId = Guid.NewGuid(), Score10 = 4, RatedAt = DateTime.UtcNow },
                new Rating { Id = Guid.NewGuid(), MovieId = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 7, RatedAt = DateTime.UtcNow }
            };

            // IRatingService.GetAllAsync() tüm ratingleri dönecek
            var mockRatingSvc = new Mock<IRatingService>();
            mockRatingSvc.Setup(s => s.GetAllAsync())
                         .ReturnsAsync(ratings);

            var mockMovieSvc = new Mock<IMovieService>();
            // movie servis burada kullanılmıyor, bu testte "movieId" filtresi için sadece GetAllAsync() yeterli.

            var mapper = BuildMapper();
            var accessor = BuildHttpContextAccessor();
            var controller = new RatingsController(mockRatingSvc.Object, mockMovieSvc.Object, mapper, accessor);

            // Act: movieId parametresini verdik
            var actionResult = await controller.Get(movieId: movieId, userId: null, mine: false);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var ok = actionResult.Result as OkObjectResult;
            ok.Value.Should().BeAssignableTo<IEnumerable<RatingListDto>>();

            var list = ok.Value as IEnumerable<RatingListDto>;
            list.Should().HaveCount(1);
            list.First().MovieId.Should().Be(movieId);
        }

        [Fact]
        public async Task Get_WithNoFilters_ReturnsAllRatings()
        {
            // Arrange
            var ratings = new List<Rating>
            {
                new Rating { Id = Guid.NewGuid(), MovieId = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 5, RatedAt = DateTime.UtcNow },
                new Rating { Id = Guid.NewGuid(), MovieId = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 9, RatedAt = DateTime.UtcNow },
                new Rating { Id = Guid.NewGuid(), MovieId = Guid.NewGuid(), UserId = Guid.NewGuid(), Score10 = 3, RatedAt = DateTime.UtcNow }
            };

            var mockRatingSvc = new Mock<IRatingService>();
            mockRatingSvc.Setup(s => s.GetAllAsync())
                         .ReturnsAsync(ratings);

            var mockMovieSvc = new Mock<IMovieService>();

            var mapper = BuildMapper();
            var accessor = BuildHttpContextAccessor();
            var controller = new RatingsController(mockRatingSvc.Object, mockMovieSvc.Object, mapper, accessor);

            // Act: hiçbir filtre vermiyoruz
            var actionResult = await controller.Get(movieId: null, userId: null, mine: false);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var ok = actionResult.Result as OkObjectResult;
            ok.Value.Should().BeAssignableTo<IEnumerable<RatingListDto>>();

            var list = ok.Value as IEnumerable<RatingListDto>;
            list.Should().HaveCount(3);
        }

        [Fact]
        public async Task Upsert_WhenMovieExists_ReturnsOkWithRatingDetail()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            var dto = new CreateRatingDto
            {
                MovieId = movieId,
                Score10 = 8
            };

            // 1) IMovieService.GetByIdAsync(dto.MovieId) film bulundu sinimali veriyor
            var mockMovieSvc = new Mock<IMovieService>();
            mockMovieSvc.Setup(m => m.GetByIdAsync(movieId))
                        .ReturnsAsync(new Movie { Id = movieId, Title = "Var Olan Film" });

            // 2) IRatingService.AddOrUpdateAsync(...) doğrudan aynı Rating nesnesini geri “kaydedilmiş” şekilde dönecek
            var savedRating = new Rating
            {
                Id = Guid.NewGuid(),
                MovieId = movieId,
                UserId = Guid.NewGuid(),
                Score10 = dto.Score10,
                RatedAt = DateTime.UtcNow
            };
            var mockRatingSvc = new Mock<IRatingService>();
            mockRatingSvc.Setup(r => r.AddOrUpdateAsync(It.IsAny<Rating>()))
                         .ReturnsAsync(savedRating);

            var mapper = BuildMapper();
            var accessor = BuildHttpContextAccessor();
            var controller = new RatingsController(mockRatingSvc.Object, mockMovieSvc.Object, mapper, accessor);

            // Act
            var actionResult = await controller.Upsert(dto);

            // Assert
            // ActionResult<RatingDetailDto> dönüyor; bu yüzden .Result üzerinden OkObjectResult’a ulaşalım
            actionResult.Result.Should().BeOfType<OkObjectResult>();

            var ok = actionResult.Result as OkObjectResult;
            ok.Value.Should().BeOfType<RatingDetailDto>();

            var detailDto = ok.Value as RatingDetailDto;
            detailDto.Id.Should().Be(savedRating.Id);
            detailDto.Score10.Should().Be(8);
            detailDto.MovieId.Should().Be(movieId);
        }

        [Fact]
        public async Task Upsert_WhenMovieNotFound_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            var dto = new CreateRatingDto
            {
                MovieId = movieId,
                Score10 = 4
            };

            // IMovieService.GetByIdAsync(dto.MovieId) null dönsün → film yok
            var mockMovieSvc = new Mock<IMovieService>();
            mockMovieSvc.Setup(m => m.GetByIdAsync(movieId))
                        .ReturnsAsync((Movie?)null);

            // IRatingService çağrılmamalı, ama mock tanımlayalım yine de
            var mockRatingSvc = new Mock<IRatingService>();

            var mapper = BuildMapper();
            var accessor = BuildHttpContextAccessor();
            var controller = new RatingsController(mockRatingSvc.Object, mockMovieSvc.Object, mapper, accessor);

            // Act
            var actionResult = await controller.Upsert(dto);

            // Assert
            // “Film bulunamadı” → NotFoundObjectResult dönmeli
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();

            var notFound = actionResult.Result as NotFoundObjectResult;
            // Mesaj olarak “Movie {id} not found” bekleyebiliriz
            notFound.Value.Should().Be($"Movie {movieId} not found");

            // _ratings.AddOrUpdateAsync hiç çağrılmamalı
            mockRatingSvc.Verify(r => r.AddOrUpdateAsync(It.IsAny<Rating>()), Times.Never);
        }
    }
}
