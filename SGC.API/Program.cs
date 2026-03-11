using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SGC.API.Middleware;
using SGC.API.Services;
using SGC.Application.Contracts;
using SGC.IOC;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. Registro de dependencias del sistema (IoC)
// ============================================================
builder.Services.AddSGCDependencies(builder.Configuration);

// Registro del servicio de tokens JWT (implementacion en la capa API)
builder.Services.AddScoped<ITokenService, TokenService>();

// ============================================================
// 2. Configuracion de autenticacion JWT
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

// ============================================================
// 3. CORS - Permitir acceso desde el frontend web y desktop
// ============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("SGCPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ============================================================
// 4. Controllers y Swagger
// ============================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ============================================================
// Pipeline de middleware
// ============================================================

// Middleware global de manejo de excepciones (primer middleware para capturar todo)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS antes de autenticacion
app.UseCors("SGCPolicy");

// Autenticacion antes de autorizacion
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
