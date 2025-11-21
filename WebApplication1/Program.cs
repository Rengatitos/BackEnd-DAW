using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
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

// Añadir soporte para variables de entorno
builder.Configuration.AddEnvironmentVariables();

// =======================
// 🔹 CONFIGURAR MONGO DB
// =======================
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoConnectionString = builder.Configuration["MONGODB_CONNECTIONSTRING"]
        ?? "mongodb://localhost:27017"; // Valor por defecto para desarrollo
    return new MongoClient(mongoConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["MONGODB_DATABASENAME"] ?? "OnboardingDB");
});

// =======================
// 🔹 REPOSITORIOS Y SERVICIOS
// =======================
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

// =======================
// 🤖 CLIENTE OLLAMA
// =======================
builder.Services.AddHttpClient<OllamaClient>(client =>
{
    client.BaseAddress = new Uri("http://134.199.192.88:11434/api/");
    client.Timeout = TimeSpan.FromSeconds(300);
});

// =======================
// 🔐 CONFIGURAR JWT
// =======================
var jwtKey = builder.Configuration["JWT_SECRET"] ?? "clave-secreta-prueba-12345";
var jwtIssuer = builder.Configuration["JWT_ISSUER"] ?? "OnboardingAPI";

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
        ValidAudience = builder.Configuration["JWT_AUDIENCE"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// =======================
// 🌐 CORS (SOLUCIÓN AL ERROR "Failed to fetch")
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =======================
// 🌐 CONTROLADORES Y SWAGGER
// =======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =======================
// ⚙ MIDDLEWARE
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // ✅ NECESARIO PARA QUE SWAGGER FUNCIONE

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// =======================
// 🚀 ARRANQUE FINAL
// =======================
Console.WriteLine("✅ Servidor Onboarding API iniciado correctamente...");
Console.WriteLine("🧠 Cliente Ollama disponible en http://134.199.192.88:11434/api/");

app.Run();