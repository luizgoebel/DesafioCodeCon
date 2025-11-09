using System.Collections.Concurrent;
using DesafioCodeCon.Context;
using DesafioCodeCon.Models;
using DesafioCodeCon.Services.Iservices;
using Microsoft.EntityFrameworkCore;

namespace DesafioCodeCon.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UsuarioDbContext _context;
    private readonly IServiceScopeFactory _scopeFactory;

    public UsuarioService(UsuarioDbContext context, IServiceScopeFactory scopeFactory)
    {
        _context = context;
        _scopeFactory = scopeFactory;
    }

    public bool PostUsers(List<Usuario> usuarios)
    {
        if (usuarios == null || usuarios.Count == 0) return false;
        _context.Usuarios.AddRange(usuarios);
        int saved = _context.SaveChanges();
        return saved > 0;
    }

    public object GetSuperUsers(int score, bool active)
    {
        var start = DateTime.UtcNow;
        var query = _context.Usuarios
            .AsNoTracking()
            .Where(u => u.Score >= score && u.Active == active)
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Score,
                u.Active,
                u.Country
            })
            .ToList();
        var ttalMilliseconds = (DateTime.UtcNow - start).TotalMilliseconds;
        return new { data = query, processamentoTimeMs = ttalMilliseconds };
    }

    public object GetTopCountries()
    {
        var superUsers = _context.Usuarios
            .AsNoTracking()
            .Where(u => u.Score >= 900 && u.Active)
            .GroupBy(u => u.Country)
            .Select(g => new { country = g.Key, count = g.Count() })
            .OrderByDescending(x => x.count)
            .Take(5)
            .ToList();
        return superUsers;
    }

    public async Task<object> GetTeamInsights(CancellationToken cancellationToken = default)
    {
        // Processamento streaming (sem paralelismo) para estabilidade e velocidade com 1000 registros
        var agg = new Dictionary<string, (int members, int leaders, int projectsCompleted, int active)>();

        await foreach (var u in _context.Usuarios
            .AsNoTracking()
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken))
        {
            var teamName = u.Team?.Name ?? string.Empty;
            var isLeader = u.Team?.Leader ?? false;
            var projectsCompleted = u.Team?.Projects?.Count(p => p.Completed) ?? 0;
            var isActive = u.Active ? 1 : 0;

            if (!agg.TryGetValue(teamName, out var curr))
                curr = (0, 0, 0, 0);

            curr.members++;
            if (isLeader) curr.leaders++;
            curr.projectsCompleted += projectsCompleted;
            curr.active += isActive;

            agg[teamName] = curr;
        }

        var data = agg.Select(kv => new
        {
            team = kv.Key,
            totalMembers = kv.Value.members,
            leaders = kv.Value.leaders,
            projectsCompleted = kv.Value.projectsCompleted,
            activePercentage = kv.Value.members == 0 ? 0 : (double)kv.Value.active / kv.Value.members * 100.0
        })
        .OrderByDescending(x => x.totalMembers)
        .ToList();

        return data;
    }

    public object GetActiveUsersPerDay(int? min)
    {
        var data = _context.Usuarios
            .AsNoTracking()
            .SelectMany(u => u.Logs.Where(l => l.Action != null && l.Action.Equals("login", StringComparison.OrdinalIgnoreCase)).Select(l => l.Date))
            .GroupBy(d => d)
            .Select(g => new { date = g.Key, logins = g.Count() })
            .Where(x => !min.HasValue || x.logins >= min.Value)
            .OrderBy(x => x.date)
            .ToList();
        return data;
    }

    public object GetEvaluation()
    {
        return new { totalUsers = _context.Usuarios.Count() };
    }
}
