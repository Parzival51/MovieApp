using Microsoft.EntityFrameworkCore;
using MovieApp.Business.Abstract;
using MovieApp.Business.Concrete;
using MovieApp.DataAccess.Abstract;
using MovieApp.DataAccess.Concrete;
using MovieApp.DataAccess.Context;
using MovieApp.DataAccess.Repositories;

namespace MovieApp.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MovieAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IMovieImageRepository, MovieImageRepository>();

            services.AddScoped<IActorRatingRepository, ActorRatingRepository>();
            services.AddScoped<IActorRatingService, ActorRatingManager>();

            services.AddScoped<IMovieService, MovieManager>();
            services.AddScoped<IReviewService, ReviewManager>();
            services.AddScoped<ICommentService, CommentManager>();
            services.AddScoped<IRatingService, RatingManager>();
            services.AddScoped<IRefreshTokenService, RefreshTokenManager>();
            services.AddScoped<IGenreService, GenreManager>();
            services.AddScoped<IActorService, ActorManager>();
            services.AddScoped<IDirectorService, DirectorManager>();
            services.AddScoped<IActivityLogService, ActivityLogManager>();
            services.AddScoped<IFavoriteService, FavoriteManager>();
            services.AddScoped<IWatchlistService, WatchlistManager>();
            services.AddScoped<ILanguageService, LanguageManager>();
            services.AddScoped<IMovieImageService, MovieImageManager>();

            services.AddScoped<IExternalMovieService, ExternalMovieService>();
            services.AddScoped<IExternalPersonService, ExternalPersonService>();

            services.AddHttpClient<IExternalMovieService, ExternalMovieService>();
            services.AddHttpClient<IExternalPersonService, ExternalPersonService>();


            services.AddScoped<IMovieImageRepository, MovieImageRepository>();
            services.AddScoped<IMovieImageService, MovieImageManager>();

            services.AddScoped<IDirectorRatingRepository, DirectorRatingRepository>();
            services.AddScoped<IDirectorRatingService, DirectorRatingManager>();

            return services;
        }
    }
}
