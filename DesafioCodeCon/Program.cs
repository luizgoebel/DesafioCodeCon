using DesafioCodeCon.Context;
using Microsoft.EntityFrameworkCore;
using DesafioCodeCon.Services;
using DesafioCodeCon.Services.Iservices;

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Rota raiz simples para mensagem ao abrir no navegador
app.MapGet("/", () => "Api rodando");

app.Run();
