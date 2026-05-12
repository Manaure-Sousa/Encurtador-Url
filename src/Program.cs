using EncurtadorUrl.src.Data;
using EncurtadorUrl.src.Endpoints;
using EncurtadorUrl.src.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<UrlShorteningService>();

// Configurar CORS para aceitar requisições do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Aplicar política de CORS
app.UseCors("AllowFrontend");

app.MapShortenUrlEndpoint();

app.UseHttpsRedirection();

app.Run();