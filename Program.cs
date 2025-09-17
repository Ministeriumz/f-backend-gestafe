using f_backend_gestafe.Data;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Data.Repositories;
using f_backend_gestafe.Services.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(opt => { }, AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("DefaultPolicy", policy =>
{
    policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
}));

builder.Services.AddScoped<IIgrejaRepository, IgrejaRepository>();
builder.Services.AddScoped<IIgrejaService, IgrejaService>();

builder.Services.AddScoped<IEventosService,EventosService>();
builder.Services.AddScoped<IEventosRepository, EventosRepository>();

builder.Services.AddScoped<IMinisterioService,MinisterioService>();
builder.Services.AddScoped<IMinisterioRepository, MinisterioRepository>();

builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Build app
var app = builder.Build();

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

// CORS
app.UseCors("DefaultPolicy");

app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();
