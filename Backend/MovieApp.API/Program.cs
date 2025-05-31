using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieApp.API.Extensions;
using MovieApp.API.Filters;
using MovieApp.Business.Abstract;
using MovieApp.Business.Concrete;
using MovieApp.Business.Configurations;
using MovieApp.DataAccess.Abstract;
using MovieApp.DataAccess.Concrete;
using MovieApp.DataAccess.Context;
using MovieApp.Entity.Entities;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddDbContext<MovieAppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddInfrastructure(configuration);

builder.Services.AddHttpClient<IExternalMovieService, ExternalMovieService>(c =>
{
    c.BaseAddress = new Uri("https://api.themoviedb.org/3/");
});

builder.Services.AddScoped<IGenreService, GenreManager>();
builder.Services.AddScoped<IActorService, ActorManager>();
builder.Services.AddScoped<IDirectorService, DirectorManager>();


builder.Services.AddIdentity<User, Role>(opts =>
{
    opts.Password.RequiredLength = 6;
    opts.Password.RequireDigit = false;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = false;
    opts.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<MovieAppDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<JwtTokenOptions>(configuration.GetSection("TokenOptions"));

var jwtOptions = configuration.GetSection("TokenOptions").Get<JwtTokenOptions>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IUserService, UserManagerService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<ValidationFilter>();
    })
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.Map("/error", (HttpContext http) =>
{
    var feature = http.Features.Get<IExceptionHandlerFeature>();
    var ex = feature?.Error;

    var pd = new ProblemDetails
    {
        Status = (int)HttpStatusCode.InternalServerError,
        Title = "Sunucu hatasý",
        Detail = ex?.Message
    };

    return Results.Problem(pd);
});

app.UseCors("AllowLocalDev");

app.UseHttpsRedirection();



using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var roles = new[] { "Admin", "User", "Moderator" };

    foreach (var roleName in roles)
    {
        if (!await roleMgr.RoleExistsAsync(roleName))
        {
            var result = await roleMgr.CreateAsync(new Role { Name = roleName });
            if (!result.Succeeded)
                throw new Exception($"Rol oluþturulamadý: {roleName} — {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();




app.Run();