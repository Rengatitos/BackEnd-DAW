using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
// =======================================================
// 📦 USINGS (Importación de tus carpetas)
// =======================================================
// Si alguna línea marca rojo, bórrala y deja la que funcione según tu estructura de carpetas.

using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Helpers;
using Onboarding.CORE.Infrastructure.Repositories;
using Onboarding.CORE.Services;
// A veces los repositorios están en INFRA o Infrastructure, dejo ambos por si acaso:
using Onboarding.INFRA.Repositories;
using Onboarding.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// 🌍 1. CONFIGURACIÓN DE PUERTO (Vital para Render)
// =======================================================
// Render asigna un puerto dinámico en la variable PORT. Si no existe, usa el 8080.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Cargar variables de entorno del sistema
builder.Configuration.AddEnvironmentVariables();

// =======================================================
// 🤖 2. CLIENTE OLLAMA (Configuración Dual)
// =======================================================
// Intenta leer la variable de Render. Si no existe, usa tu IP de respaldo.
var ollamaUrl = builder.Configuration["OllamaBaseUrl"] ?? "http://134.199.192.88:11434/api/";

builder.Services.AddHttpClient<OllamaClient>(client =>
{
    // Aseguramos que termine en '/' para evitar errores de ruta
    if (!ollamaUrl.EndsWith("/")) ollamaUrl += "/";

    client.BaseAddress = new Uri(ollamaUrl);
    client.Timeout = TimeSpan.FromMinutes(5); // Tiempo de espera largo para IA
});

// =======================================================
// 🍃 3. MONGO DB
// =======================================================
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connection = builder.Configuration["MongoDB:ConnectionString"]
        ?? builder.Configuration["MONGODB_CONNECTIONSTRING"]
        ?? "mongodb://localhost:27017";
    return new MongoClient(connection);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var dbName = builder.Configuration.GetValue<string>("MongoDB:DatabaseName")
        ?? builder.Configuration["MONGODB_DATABASENAME"]
        ?? "OnboardingDB";
    return client.GetDatabase(dbName);
});

// =======================================================
// 💉 4. INYECCIÓN DE DEPENDENCIAS (Todos los módulos)
// =======================================================

// --- Módulos Base ---
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

// --- Módulos Extra (Activos) ---
builder.Services.AddScoped<ICatalogoOnboardingRepository, CatalogoOnboardingRepository>();
builder.Services.AddScoped<ICatalogoOnboardingService, CatalogoOnboardingService>();

builder.Services.AddScoped<ISalasChatRepository, SalasChatRepository>();
builder.Services.AddScoped<ISalasChatService, SalasChatService>();

// =======================================================
// 🔐 5. SEGURIDAD (JWT)
// =======================================================
var jwtKey = builder.Configuration["Jwt:Secret"]
    ?? builder.Configuration["JWT_SECRET"]
    ?? "clave-secreta-prueba-12345-super-larga-para-seguridad";

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? builder.Configuration["JWT_ISSUER"]
    ?? "OnboardingAPI";

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? builder.Configuration["JWT_AUDIENCE"]
    ?? "OnboardingFrontend";

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// =======================================================
// 🌐 6. CORS & SWAGGER
// =======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Onboarding API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization. Escribe 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// =======================================================
// 🚀 7. PIPELINE (Middleware)
// =======================================================

// Swagger siempre visible (incluso en producción/Render)
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Mensajes de log para confirmar arranque
Console.WriteLine("==============================================");
Console.WriteLine($"🚀 Onboarding API iniciada en puerto: {port}");
Console.WriteLine($"🧠 Cliente Ollama configurado a: {ollamaUrl}");
Console.WriteLine("==============================================");

app.Run();