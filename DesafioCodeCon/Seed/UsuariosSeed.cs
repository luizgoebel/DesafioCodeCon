using DesafioCodeCon.Context;
using DesafioCodeCon.Models;
using System.Text.Json;

namespace DesafioCodeCon.Seed;

public static class UsuariosSeed
{
    public static void SeedFromJson(UsuarioDbContext context, IConfiguration configuration, IWebHostEnvironment env, ILogger logger)
    {
        // Evita reseed se já houver dados
        if (context.Usuarios.Any())
        {
            logger.LogInformation("UsuariosSeed: dados já existem, ignorando seed.");
            return;
        }

        // Caminho configurável em appsettings: "Seed:UsersFilePath". Ex.: "Seed/users.json"
        var configuredPath = configuration["Seed:UsersFilePath"];
        string path = configuredPath ?? Path.Combine("Seed", "usuarios.json");

        // Se for relativo, tornar absoluto a partir do ContentRoot
        if (!Path.IsPathRooted(path))
        {
            path = Path.Combine(env.ContentRootPath, path);
        }

        if (!File.Exists(path))
        {
            logger.LogWarning("UsuariosSeed: arquivo JSON não encontrado em {Path}. Nenhum dado foi carregado.", path);
            return;
        }

        try
        {
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, options) ?? new List<Usuario>();

            if (usuarios.Count == 0)
            {
                logger.LogWarning("UsuariosSeed: arquivo vazio ou inválido em {Path}.", path);
                return;
            }

            // Garante IDs para registros sem Id
            foreach (var u in usuarios)
            {
                if (u.Id == Guid.Empty)
                    u.Id = Guid.NewGuid();

                // Normaliza coleções nulas
                u.Team ??= new Team();
                u.Team.Projects ??= new List<Project>();
                u.Logs ??= new List<Log>();
            }

            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();
            logger.LogInformation("UsuariosSeed: {Count} usuários importados de {Path}.", usuarios.Count, path);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UsuariosSeed: erro ao carregar arquivo JSON de usuários.");
        }
    }
}
