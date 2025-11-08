using DesafioCodeCon.Context;
using Microsoft.EntityFrameworkCore;
using DesafioCodeCon.Services;
using DesafioCodeCon.Services.Iservices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<UsuarioDbContext>(options =>
    options.UseInMemoryDatabase("UsuarioDb"));

// Register application services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<UsuarioDbContext>();
    ctx.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
