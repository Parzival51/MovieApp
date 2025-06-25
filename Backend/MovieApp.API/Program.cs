// Program.cs   ( MovieApp.API )

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
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
var cfg = builder.Configuration;


builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowLocalDev", p => p
        .SetIsOriginAllowed(origin =>
        {
            var u = new Uri(origin);
            return u.Host == "localhost" && u.Port == 5173;
        })
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});


builder.Services.AddDbContext<MovieAppDbContext>(o =>
    o.UseSqlServer(cfg.GetConnectionString("DefaultConnection")));


builder.Services.Configure<SmtpSettings>(cfg.GetSection("SmtpSettings"));
builder.Services.Configure<AppUrlsOptions>(cfg.GetSection("AppUrls"));
builder.Services.AddTransient<IEmailSender, EmailSender>();


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


builder.Services.ConfigureApplicationCookie(c =>
{
    c.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    c.Events.OnRedirectToAccessDenied = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(
                    new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")));


builder.Services.Configure<JwtTokenOptions>(cfg.GetSection("TokenOptions"));
var jwt = cfg.GetSection("TokenOptions").Get<JwtTokenOptions>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
        NameClaimType = ClaimTypes.NameIdentifier
    };
});


builder.Services.AddAuthorization(opt =>
{
    opt.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});


builder.Services.AddInfrastructure(cfg);

builder.Services.AddSingleton(UrlEncoder.Default);
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IUserService, UserManagerService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers(c => c.Filters.Add<ValidationFilter>())
                .AddJsonOptions(o =>
                    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext ctx) =>
{
    var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;

    return Results.Problem(
        title: "Sunucu hatasý",
        detail: ex?.Message,
        statusCode: StatusCodes.Status500InternalServerError);
});

using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    foreach (var role in new[] { "Admin", "User", "Moderator" })
        if (!await roleMgr.RoleExistsAsync(role))
            await roleMgr.CreateAsync(new Role { Name = role });
}


app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,  
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseCors("AllowLocalDev");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
