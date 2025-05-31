using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entity.Entities;

namespace MovieApp.DataAccess.Context
{
    public class MovieAppDbContext
        : IdentityDbContext<
            User,
            Role,
            Guid,
            IdentityUserClaim<Guid>,
            UserRole,
            IdentityUserLogin<Guid>,
            IdentityRoleClaim<Guid>,
            IdentityUserToken<Guid>>
    {
        public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie>           Movies            { get; set; }
        public DbSet<Genre>           Genres            { get; set; }
        public DbSet<MovieGenre>      MovieGenres       { get; set; }
        public DbSet<Actor>           Actors            { get; set; }
        public DbSet<MovieActor>      MovieActors       { get; set; }
        public DbSet<Director>        Directors         { get; set; }
        public DbSet<MovieDirector>   MovieDirectors    { get; set; }
        public DbSet<Review>          Reviews           { get; set; }
        public DbSet<Rating>          Ratings           { get; set; }
        public DbSet<Comment>         Comments          { get; set; }
        public DbSet<Favorite>        Favorites         { get; set; }
        public DbSet<Watchlist>       Watchlists        { get; set; }
        public DbSet<RefreshToken>    RefreshTokens     { get; set; }
        public DbSet<ActivityLog>     ActivityLogs      { get; set; }
        public DbSet<Language>        Languages         { get; set; }
        public DbSet<MovieLanguage>   MovieLanguages    { get; set; }
        public DbSet<MovieImage>      MovieImages       { get; set; }
        public DbSet<ActorRating> ActorRatings { get; set; }

        public DbSet<DirectorRating> DirectorRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<User>(e => e.ToTable("Users"));
            b.Entity<Role>(e => e.ToTable("Roles"));
            b.Entity<IdentityUserClaim<Guid>>(e => e.ToTable("UserClaims"));
            b.Entity<UserRole>(e => e.ToTable("UserRoles"));
            b.Entity<IdentityUserLogin<Guid>>(e => e.ToTable("UserLogins"));
            b.Entity<IdentityRoleClaim<Guid>>(e => e.ToTable("RoleClaims"));
            b.Entity<IdentityUserToken<Guid>>(e => e.ToTable("UserTokens"));

            b.Entity<ActorRating>(e =>
            {
                e.HasKey(ar => ar.Id);
                e.HasOne(ar => ar.Actor)
                 .WithMany()              
                 .HasForeignKey(ar => ar.ActorId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ar => ar.User)
                 .WithMany()             
                 .HasForeignKey(ar => ar.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.ToTable("ActorRatings");
            });


            b.Entity<UserRole>(e =>
            {
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
                e.HasOne(ur => ur.User)
                 .WithMany(u => u.UserRoles)
                 .HasForeignKey(ur => ur.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ur => ur.Role)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(ur => ur.RoleId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            b.Entity<Genre>(e =>
            {
                e.HasKey(g => g.Id);
                e.Property(g => g.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(g => g.Name).IsUnique();
                e.ToTable("Genres");
            });

            b.Entity<Movie>(e =>
            {
                e.HasKey(m => m.Id);
                e.Property(m => m.Title).IsRequired().HasMaxLength(200);
                e.HasIndex(m => m.Title);
                e.Property(m => m.Tagline).HasMaxLength(255);
                e.Property(m => m.Status).HasMaxLength(30);
                e.Property(m => m.Budget);
                e.Property(m => m.Revenue);
                e.Property(m => m.ImdbId).HasMaxLength(15);
                e.Property(m => m.HomepageUrl).HasMaxLength(300);
                e.Property(m => m.IsAdult).HasDefaultValue(false);
                e.Property(m => m.BackdropPath).HasMaxLength(200);
                e.ToTable("Movies");
                e.HasIndex(m => m.ExternalId).IsUnique();
            });

            b.Entity<MovieGenre>(e =>
            {
                e.HasKey(mg => new { mg.MovieId, mg.GenreId });
                e.HasOne(mg => mg.Movie)
                 .WithMany(m => m.MovieGenres)
                 .HasForeignKey(mg => mg.MovieId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(mg => mg.Genre)
                 .WithMany(g => g.MovieGenres)
                 .HasForeignKey(mg => mg.GenreId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.ToTable("MovieGenres");
            });

            
            b.Entity<Actor>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Name).IsRequired().HasMaxLength(200);
                e.Property(a => a.Gender);
                e.Property(a => a.ProfilePath).HasMaxLength(200);
                e.Property(a => a.Popularity);
                e.HasIndex(a => a.ExternalId).IsUnique();
                e.ToTable("Actors");
            });

            
            b.Entity<MovieActor>(e =>
            {
                e.HasKey(ma => new { ma.MovieId, ma.ActorId });
                e.HasOne(ma => ma.Movie)
                 .WithMany(m => m.MovieActors)
                 .HasForeignKey(ma => ma.MovieId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ma => ma.Actor)
                 .WithMany(a => a.MovieActors)
                 .HasForeignKey(ma => ma.ActorId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.Property(ma => ma.Order).HasDefaultValue(0);
                e.ToTable("MovieActors");
            });

            
            b.Entity<Director>(e =>
            {
                e.HasKey(d => d.Id);
                e.Property(d => d.Name).IsRequired().HasMaxLength(200);
                e.Property(d => d.Gender);
                e.Property(d => d.ProfilePath).HasMaxLength(200);
                e.Property(d => d.Popularity);
                e.HasIndex(d => d.ExternalId).IsUnique();
                e.ToTable("Directors");
            });

            
            b.Entity<MovieDirector>(e =>
            {
                e.HasKey(md => new { md.MovieId, md.DirectorId });
                e.HasOne(md => md.Movie)
                 .WithMany(m => m.MovieDirectors)
                 .HasForeignKey(md => md.MovieId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(md => md.Director)
                 .WithMany(d => d.MovieDirectors)
                 .HasForeignKey(md => md.DirectorId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.ToTable("MovieDirectors");
            });

         
            b.Entity<Review>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.IsApproved).HasDefaultValue(false);
                e.HasOne(r => r.User)
                 .WithMany(u => u.Reviews)
                 .HasForeignKey(r => r.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(r => r.Movie)
                 .WithMany(m => m.Reviews)
                 .HasForeignKey(r => r.MovieId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.ToTable("Reviews");
            });

            
            b.Entity<Rating>(e =>
            {
                e.HasKey(r => r.Id);
                e.HasIndex(r => new { r.MovieId, r.UserId }).IsUnique();
                e.ToTable("Ratings");
            });

            
            b.Entity<Comment>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Content).IsRequired().HasMaxLength(1000);
                e.HasOne(c => c.Review)
                 .WithMany(r => r.Comments)
                 .HasForeignKey(c => c.ReviewId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(c => c.User)
                 .WithMany()
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.ToTable("Comments");
            });

            b.Entity<Favorite>(e =>
            {
                e.HasKey(f => f.Id);
                e.HasIndex(f => new { f.UserId, f.MovieId }).IsUnique();
                e.ToTable("Favorites");
            });

            b.Entity<Watchlist>(e =>
            {
                e.HasKey(w => w.Id);
                e.HasIndex(w => new { w.UserId, w.MovieId }).IsUnique();
                e.ToTable("Watchlists");
            });

            b.Entity<RefreshToken>(e =>
            {
                e.HasKey(rt => rt.Id);
                e.Property(rt => rt.Token).IsRequired();
                e.ToTable("RefreshTokens");
            });

            b.Entity<ActivityLog>(e =>
            {
                e.HasKey(al => al.Id);
                e.Property(al => al.Action).IsRequired().HasMaxLength(200);
                e.HasOne(al => al.User)
                 .WithMany(u => u.ActivityLogs)
                 .HasForeignKey(al => al.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.ToTable("ActivityLogs");
            });

            b.Entity<Language>(e =>
            {
                e.HasKey(l => l.Iso639);
                e.Property(l => l.EnglishName).IsRequired().HasMaxLength(80);
                e.ToTable("Languages");
            });

            b.Entity<MovieLanguage>(e =>
            {
                e.HasKey(ml => new { ml.MovieId, ml.Iso639 });
                e.HasOne(ml => ml.Movie)
                 .WithMany(m => m.MovieLanguages)
                 .HasForeignKey(ml => ml.MovieId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ml => ml.Language)
                 .WithMany()
                 .HasForeignKey(ml => ml.Iso639)
                 .OnDelete(DeleteBehavior.Cascade);
                e.ToTable("MovieLanguages");
            });

            b.Entity<MovieImage>(e =>
        {
                e.HasKey(mi => mi.Id);
                e.HasOne(mi => mi.Movie)
                             .WithMany(m => m.Images)
                             .HasForeignKey(mi => mi.MovieId)
                             .OnDelete(DeleteBehavior.Cascade);
                e.Property(mi => mi.Type).HasMaxLength(20).IsRequired();
                e.ToTable("MovieImages");
                        });
        }
    }
}
