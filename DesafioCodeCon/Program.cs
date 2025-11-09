using DesafioCodeCon.Context;
using DesafioCodeCon.Seed; // adicionado para usar UsuariosSeed
using DesafioCodeCon.Services;
using DesafioCodeCon.Services.Iservices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<UsuarioDbContext>(options =>
    options.UseInMemoryDatabase("UsuarioDb"));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<UsuarioDbContext>();
    ctx.Database.EnsureCreated();

    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("Seed");
    UsuariosSeed.SeedFromJson(ctx, app.Configuration, env, logger);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Rota raiz simples para mensagem ao abrir no navegador
app.MapGet("/", () => "Api rodando");

app.Run();
