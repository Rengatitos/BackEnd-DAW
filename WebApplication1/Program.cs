using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Helpers;
using Onboarding.CORE.Services;
using Onboarding.CORE.Settings;
using Onboarding.INFRA.Repositories;
using Onboarding.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// =======================================
// 🔥 Configuración CORRECTA de HttpClient
// =======================================
builder.Services.AddHttpClient<OllamaClient>(client =>
{
    client.BaseAddress = new Uri("http://134.199.192.88:11434/"); // ⚠ tu server Ollama
    client.Timeout = TimeSpan.FromSeconds(120);
});

// =======================================================
// 🔹 CONFIGURACIÓN DE MONGO DB
// =======================================================
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connection = builder.Configuration["MongoDB:ConnectionString"]
        ?? "mongodb://localhost:27017";
    return new MongoClient(connection);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("OnboardingDB");
});

// =======================================================
// 🔹 REPOSITORIOS Y SERVICIOS
// =======================================================
builder.Services.AddScoped<IActividadRepository, ActividadRepository>();
builder.Services.AddScoped<IActividadService, ActividadService>();

builder.Services.AddScoped<IInteraccionChatRepository, InteraccionChatRepository>();
builder.Services.AddScoped<IInteraccionChatService, InteraccionChatService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();

builder.Services.AddScoped<IRecursoRepository, RecursoRepository>();
builder.Services.AddScoped<IRecursoService, RecursoService>();

builder.Services.AddScoped<IJwtService, JwtService>();


// =======================================================
// 🔐 CONFIGURACIÓN DE JWT
// =======================================================
var jwtKey = builder.Configuration["Jwt:Secret"] ?? "clave-secreta-super-segura-12345";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "OnboardingAPI";

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
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// =======================================================
// 🌐 CORS — NECESARIO PARA SWAGGER Y FRONTEND
// =======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =======================================================
// 🌐 CONTROLADORES + SWAGGER
// =======================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =======================================================
// ⚙️ CREAR APP
// =======================================================
var app = builder.Build();

// =======================================================
// 🧩 MIDDLEWARE
// =======================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("============================================");
Console.WriteLine("  🚀 Onboarding API Iniciada Correctamente");
Console.WriteLine("  🤖 Ollama disponible en: http://134.199.192.88:11434/api/");
Console.WriteLine("============================================\n");

app.Run();
