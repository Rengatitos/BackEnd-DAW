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

// =======================================================
// 🌍 Render: Puerto dinámico
// =======================================================
builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT")}");

// =======================================================
// 🔧 Cargar variables de entorno
// =======================================================
builder.Configuration.AddEnvironmentVariables();

// =======================================================
// 🤖 CONFIGURAR CLIENTE OLLAMA (fusión de ambos codes)
// =======================================================
builder.Services.AddHttpClient<OllamaClient>(client =>
{
    client.BaseAddress = new Uri("http://134.199.192.88:11434/api/");
    client.Timeout = TimeSpan.FromSeconds(300);
});

// =======================================================
// 🔹 CONFIGURACIÓN DE MONGO DB (fusionado)
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
// 🔹 REPOSITORIOS Y SERVICIOS (combinado de ambos codes)
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

//// Extra que el primer Program.cs tenía:
//builder.Services.AddScoped<ICatalogoOnboardingRepository, CatalogoOnboardingRepository>();
//builder.Services.AddScoped<ICatalogoOnboardingService, CatalogoOnboardingService>();

//builder.Services.AddScoped<ISalasChatRepository, SalasChatRepository>();
//builder.Services.AddScoped<ISalasChatService, SalasChatService>();

// =======================================================
// 🔐 JWT — Fusión correcta
// =======================================================
var jwtKey = builder.Configuration["Jwt:Secret"]
    ?? builder.Configuration["JWT_SECRET"]
    ?? "clave-secreta-prueba-12345";

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
// 🌐 CORS (para frontend y swagger)
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

var app = builder.Build();

// =======================================================
// 🧩 MIDDLEWARE
// =======================================================

// Swagger siempre habilitado (Render lo permite)
app.UseSwagger();
app.UseSwaggerUI();

// Render maneja HTTPS, así que no usar redirección
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("==============================================");
Console.WriteLine("🚀 Onboarding API iniciada correctamente");
Console.WriteLine("🧠 Cliente Ollama: http://134.199.192.88:11434/api/");
Console.WriteLine("==============================================");

app.Run();
