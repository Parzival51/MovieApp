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
using MovieApp.Business.Utilities.Results;
using MovieApp.DataAccess.Context;
using MovieApp.DataAccess.Models;
using MovieApp.Entity.Entities;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

/* ---------- CORS ---------- */
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowLocalDev", p =>
        p.SetIsOriginAllowed(origin =>
        {
            var uri = new Uri(origin);
            return uri.Host == "localhost" && uri.Port == 5173;
        })
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});
/* ---------- DbContext ---------- */
builder.Services.AddDbContext<MovieAppDbContext>(o =>
    o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

/* ---------- SMTP & URL Options ---------- */
builder.Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
builder.Services.Configure<AppUrlsOptions>(configuration.GetSection("AppUrls"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

/* ---------- Infrastructure & Identity ---------- */
builder.Services.AddInfrastructure(configuration);

builder.Services.AddIdentity<User, Role>(o =>
{
    o.Password.RequiredLength = 6;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = false;
    o.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<MovieAppDbContext>()
.AddDefaultTokenProviders();

/* ---------- JWT ---------- */
builder.Services.Configure<JwtTokenOptions>(configuration.GetSection("TokenOptions"));
var jwt = configuration.GetSection("TokenOptions").Get<JwtTokenOptions>();

builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = jwt.Issuer,
        ValidAudience = jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
                                      Encoding.UTF8.GetBytes(jwt.Key)),
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

/* ---------- DI ---------- */
builder.Services.AddSingleton(UrlEncoder.Default);
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IUserService, UserManagerService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IExternalMovieService, ExternalMovieService>();
builder.Services.AddHttpClient<IExternalPersonService, ExternalPersonService>();

/* ---------- MVC ---------- */
builder.Services.AddControllers(c => c.Filters.Add<ValidationFilter>())
                .AddJsonOptions(j =>
                    j.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* ---------- Build & Middleware ---------- */
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext ctx) =>
{
    var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    return Results.Problem(new ProblemDetails
    {
        Status = (int)HttpStatusCode.InternalServerError,
        Title = "Sunucu hatasý",
        Detail = ex?.Message
    });
});



/* ---------- Rol seed ---------- */
using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    foreach (var role in new[] { "Admin", "User", "Moderator" })
        if (!await roleMgr.RoleExistsAsync(role))
            await roleMgr.CreateAsync(new Role { Name = role });
}

/* ---------- Cookie, Auth, Routing ---------- */
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});


app.UseCors("AllowLocalDev");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
