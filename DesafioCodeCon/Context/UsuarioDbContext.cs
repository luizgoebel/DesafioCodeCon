using DesafioCodeCon.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioCodeCon.Context;

public class UsuarioDbContext : DbContext
{
    public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var usuario = modelBuilder.Entity<Usuario>();
        usuario.ToTable("users");
        usuario.HasKey(u => u.Id);

        usuario.Property(u => u.Name).IsRequired();
        usuario.Property(u => u.Country).IsRequired();
        usuario.Property(u => u.Age).IsRequired();
        usuario.Property(u => u.Score).IsRequired();
        usuario.Property(u => u.Active).IsRequired();

        usuario.OwnsOne(u => u.Team, team =>
        {
            team.Property(t => t.Name).IsRequired();
            team.Property(t => t.Leader).IsRequired();

            team.OwnsMany(t => t.Projects, project =>
            {
                project.ToTable("projects");
                project.WithOwner().HasForeignKey("UsuarioId");
                project.Property<int>("Id");
                project.HasKey("Id");
                project.Property(p => p.Name).IsRequired();
                project.Property(p => p.Completed).IsRequired();
            });
        });

        usuario.OwnsMany(u => u.Logs, log =>
        {
            log.ToTable("logs");
            log.WithOwner().HasForeignKey("UsuarioId");
            log.Property<int>("Id");
            log.HasKey("Id");
            log.Property(l => l.Date).HasColumnType("date");
            log.Property(l => l.Action).IsRequired();
        });
    }
}
