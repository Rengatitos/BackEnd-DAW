using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Helpers;
using Onboarding.CORE.Services;
using Onboarding.INFRA.Repositories;
using Onboarding.Infrastructure.Repositories;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// =======================================================
// 🌍 RENDER: PUERTO DINÁMICO (NO BORRAR)
// =======================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// =======================================================
// 🔧 VARIABLES DE ENTORNO
// =======================================================
builder.Configuration.AddEnvironmentVariables();

// =======================================================
// 🤖 CLIENTE OLLAMA (CORREGIDO CON TU IP)
// =======================================================
// 1. Intenta leer la variable 'Ollama:BaseUrl' de Render (DuckDNS).
// 2. Si no existe, usa tu IP directa como respaldo.
var ollamaUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://134.199.192.88:11434/api/";

builder.Services.AddHttpClient<OllamaClient>(client =>
{
    // Aseguramos que la URL termine en barra '/' para evitar errores de ruta
    if (!ollamaUrl.EndsWith("/")) ollamaUrl += "/";

    client.BaseAddress = new Uri(ollamaUrl);
    client.Timeout = TimeSpan.FromMinutes(5); // Timeout largo para modelos de IA
});

// =======================================================
// 🔹 MONGO DB (TU CONFIGURACIÓN ORIGINAL)
// =======================================================
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoConnectionString =
        builder.Configuration.GetValue<string>("MongoDB:ConnectionString")
        ?? builder.Configuration["MONGODB_CONNECTIONSTRING"]
        ?? "mongodb://localhost:27017";

    return new MongoClient(mongoConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var dbName =
        builder.Configuration.GetValue<string>("MongoDB:DatabaseName")
        ?? builder.Configuration["MONGODB_DATABASENAME"]
        ?? "OnboardingDB";

    return client.GetDatabase(dbName);
});

// =======================================================
// 🔹 REPOSITORIOS Y SERVICIOS (TODOS TUS MÓDULOS)
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

// Descomenta si usas estos módulos también:
builder.Services.AddScoped<ICatalogoOnboardingRepository, CatalogoOnboardingRepository>();
builder.Services.AddScoped<ICatalogoOnboardingService, CatalogoOnboardingService>();
builder.Services.AddScoped<ISalasChatRepository, SalasChatRepository>();
builder.Services.AddScoped<ISalasChatService, SalasChatService>();

// =======================================================
// 🔐 JWT (TU CONFIGURACIÓN ORIGINAL)
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
// 🌐 CORS
// =======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// =======================================================
// 🌐 CONTROLADORES Y SWAGGER
// =======================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Onboarding API", Version = "v1" });
    // Configuración del candadito para probar Login en Swagger
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
// 🧩 MIDDLEWARE
// =======================================================

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // Render maneja SSL, mejor dejarlo comentado

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("==============================================");
Console.WriteLine($"🚀 Onboarding API iniciada en puerto: {port}");
Console.WriteLine($"🧠 Cliente Ollama configurado a: {ollamaUrl}");
Console.WriteLine("==============================================");

app.Run();