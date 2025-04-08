using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hackathon.AuthService.Data;
using Hackathon.AuthService.Entities;
using Hackathon.AuthService.Interfaces;
using Hackathon.AuthService.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация из appsettings.json + env (если нужно)
var configuration = builder.Configuration;

// Connection string из appsettings.json
var connectionString = configuration.GetConnectionString("DefaultConnection");

// JWT настройки
var jwtKey = configuration["JWT:SecretKey"];
var jwtIssuer = configuration["JWT:Issuer"];
var jwtAudience = configuration["JWT:Audience"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// PostgreSQL + AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ===== Swagger + JWT Bearer =====
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ===== CORS =====
var corsPolicy = "_allowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ===== Identity =====
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// ===== JWT =====
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? "fallback-secret")),
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

// ===== Custom сервисы =====
builder.Services.AddScoped<IUserInfoService, userInfoService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ===== Build & Run =====
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
