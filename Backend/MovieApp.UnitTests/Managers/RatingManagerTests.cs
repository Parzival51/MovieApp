// MovieApp.UnitTests/Managers/RatingManagerTests.cs

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieApp.Business.Concrete;
using MovieApp.DataAccess.Abstract;
using MovieApp.Entity.Entities;
using Xunit;

namespace MovieApp.UnitTests.Managers
{
    public class RatingManagerTests
    {
        private readonly Mock<IRatingRepository> _mockRepo;
        private readonly Mock<IHttpContextAccessor> _mockCtx;
        private readonly RatingManager _manager;
        private readonly Guid _fakeUserId;

        public RatingManagerTests()
        {
            // 1) Repository mock
            _mockRepo = new Mock<IRatingRepository>();

            // 2) IHttpContextAccessor mock → içinde bir ClaimTypes.NameIdentifier atıyoruz
            _fakeUserId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _fakeUserId.ToString())
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };
            _mockCtx = new Mock<IHttpContextAccessor>();
            _mockCtx.Setup(x => x.HttpContext).Returns(httpContext);

            // 3) Manager örneği
            _manager = new RatingManager(_mockRepo.Object, _mockCtx.Object);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenExistingRating_UpdatesAndReturnsExisting()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            var userId = _fakeUserId;

            // 1) Repository zaten bir rating döndürecek (existing)
            var existingRating = new Rating
            {
                Id = Guid.NewGuid(),
                MovieId = movieId,
                UserId = userId,
                Score10 = 3,
                RatedAt = DateTime.UtcNow.AddDays(-1)
            };
            _mockRepo
                .Setup(r => r.GetByMovieAndUserAsync(movieId, userId))
                .ReturnsAsync(existingRating);

            // 2) SaveChangesAsync mock
            _mockRepo
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act: toUpdate.UserId = Guid.Empty bırakıyoruz, manager içerde HttpContext’ten doldursun
            var toUpdate = new Rating
            {
                MovieId = movieId,
                UserId = Guid.Empty,
                Score10 = 9
            };
            var result = await _manager.AddOrUpdateAsync(toUpdate);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingRating.Id);
            result.Score10.Should().Be(9);
            result.RatedAt.Date.Should().Be(DateTime.UtcNow.Date);

            // Repository.Update(existingRating) en az bir kez çağrıldı mı?
            _mockRepo.Verify(r => r.Update(existingRating), Times.Once);

            // AddAsync çağrılmadı
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Rating>()), Times.Never);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenNoExistingRating_AddsAndReturnsNew()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            var userId = _fakeUserId;

            // 1) Repository “null” döndürecek → hiç rating yok
            _mockRepo
                .Setup(r => r.GetByMovieAndUserAsync(movieId, userId))
                .ReturnsAsync((Rating?)null);

            // 2) AddAsync ve SaveChangesAsync için setup
            _mockRepo
                .Setup(r => r.AddAsync(It.IsAny<Rating>()))
                .Returns(Task.CompletedTask);

            _mockRepo
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var newRating = new Rating
            {
                MovieId = movieId,
                UserId = Guid.Empty, // boş bırakalım; manager dolduracak
                Score10 = 7
            };
            var result = await _manager.AddOrUpdateAsync(newRating);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(It.Is<Rating>(x =>
                x.MovieId == movieId &&
                x.UserId == userId &&
                x.Score10 == 7
            )), Times.Once);

            result.Should().NotBeNull();
            result.MovieId.Should().Be(movieId);
            result.UserId.Should().Be(userId);
            result.Score10.Should().Be(7);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenUserNotInContext_ThrowsUnauthorizedAccessException()
        {
            // Arrange: IHttpContextAccessor.HttpContext null dönecek şekilde mock’luyoruz
            var mockEmptyCtx = new Mock<IHttpContextAccessor>();
            mockEmptyCtx.Setup(x => x.HttpContext).Returns((HttpContext)null!);

            var managerWithNoUser = new RatingManager(_mockRepo.Object, mockEmptyCtx.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                managerWithNoUser.AddOrUpdateAsync(new Rating { MovieId = Guid.NewGuid() }));
        }
    }
}
