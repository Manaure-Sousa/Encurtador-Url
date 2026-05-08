using EncurtadorUrl.src.Data;
using EncurtadorUrl.src.Endpoints;
using EncurtadorUrl.src.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

app.MapShortenUrlEndpoint();

app.UseHttpsRedirection();

app.Run();