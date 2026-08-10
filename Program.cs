using f_backend_gestafe.Configurations;
using f_backend_gestafe.Data;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Data.Repositories;
using f_backend_gestafe.Hubs; // <-- IMPORTANTE: Namespace onde você vai criar a classe LogHub
using f_backend_gestafe.Middleware;
using f_backend_gestafe.Services.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. ADICIONADO: Registrar os serviços do SignalR no container
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(opt => { }, AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };

        // --- ADICIONE ESTE BLOCO DE EVENTOS ABAIXO ---
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // Se o token vier na URL e for direcionado ao nosso hub do SignalR
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/loghub"))
                {
                    // Força o ASP.NET a ler o token que o SignalR enviou
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
        // --------------------------------------------
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Mantivemos sua política com suporte a credenciais e as portas de Dev
builder.Services.AddCors(o => o.AddPolicy("DefaultPolicy", policy =>
{
    policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://127.0.0.1:5500")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
}));

// Injeções de Dependência de Repositories e Services
builder.Services.AddScoped<IIgrejaRepository, IgrejaRepository>();
builder.Services.AddScoped<IIgrejaService, IgrejaService>();

builder.Services.AddScoped<IEventosService, EventosService>();
builder.Services.AddScoped<IEventosRepository, EventosRepository>();

builder.Services.AddScoped<IMinisterioService, MinisterioService>();
builder.Services.AddScoped<IMinisterioRepository, MinisterioRepository>();

builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IConfiguracoesRepository, ConfiguracoesRepository>();
builder.Services.AddScoped<IConfiguracoesService, ConfiguracoesService>();

builder.Services.AddScoped<IFinanceiroRepository, FinanceiroRepository>();
builder.Services.AddScoped<IFinanceiroService, FinanceiroService>();

builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<ICargosRepository, CargosRepository>();
builder.Services.AddScoped<ICargosService, CargosService>();

builder.Services.AddScoped<ICargosUsuarioRepository, CargosUsuarioRepository>();
builder.Services.AddScoped<ICargosUsuarioService, CargosUsuarioService>();

builder.Services.AddScoped<IEscalaRepository, EscalaRepository>();
builder.Services.AddScoped<IEscalaService, EscalaService>();

// Carrega as configurações do WhatsAppSettings
builder.Services.Configure<WhatsAppSettings>(
    builder.Configuration.GetSection("WhatsAppService"));

// Registra o Typed HttpClient com ciclo de vida gerenciado
builder.Services.AddHttpClient<IWhatsAppIntegrationService, WhatsAppIntegrationService>();

// Build app
var app = builder.Build();

// Aplica as migrations e popula os dados iniciais somente quando o banco está vazio.
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await DatabaseSeeder.SeedIfEmptyAsync(
        dbContext,
        app.Configuration,
        app.Logger);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
        c.RoutePrefix = string.Empty; // Acessar direto em http://localhost:5000/
    });
}

// HTTPS
app.UseHttpsRedirection();

// CORS (Sempre antes do Authentication, Authorization e MapHub)
app.UseCors("DefaultPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Controllers e Middlewares
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LogMiddleware>();

app.MapControllers();

// 2. ADICIONADO: Mapear o endpoint de conexão do SignalR
// Qualquer requisição para "/loghub" vai cair no Hub de Logs
app.MapHub<LogHub>("/loghub");

app.Run();
