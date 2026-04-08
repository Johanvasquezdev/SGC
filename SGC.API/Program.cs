using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SGC.API.Middleware;
using SGC.IOC;
using SGC.Infraestructure.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. Dependencias del sistema
// ============================================================
builder.Services.AddSGCDependencies(builder.Configuration);

// ============================================================
// 2. JWT Authentication
// ============================================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// ============================================================
// 3. CORS
// ============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("SGCPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5189",
                    "https://localhost:7224")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
        else
        {
            policy.WithOrigins(
                    "https://tu-dominio-web.com",
                    "https://tu-dominio-desktop.com")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    });
});

// ============================================================
// 4. Controllers y Swagger con JWT
// ============================================================
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MedAgenda API",
        Version = "v1",
        Description = "API del Sistema de Gestion de Citas Medicas"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa: Bearer {token}"
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
            Array.Empty<string>()
        }
    });
});

// ============================================================
// 5. Health Checks
// ============================================================
builder.Services.AddHealthChecks();

var app = builder.Build();

// ============================================================
// Pipeline de middleware
// ============================================================
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MedAgenda v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("SGCPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<CitaHub>("/citahub");
app.MapHub<DisponibilidadHub>("/disponibilidadhub");

// ============================================================
// Health Check endpoint
// ============================================================
app.MapHealthChecks("/health");

app.Run();
