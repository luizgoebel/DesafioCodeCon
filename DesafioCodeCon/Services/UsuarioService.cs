using DesafioCodeCon.Context;
using DesafioCodeCon.Models;
using DesafioCodeCon.Services.Iservices;
using Microsoft.EntityFrameworkCore;

namespace DesafioCodeCon.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UsuarioDbContext _context;

    public UsuarioService(UsuarioDbContext context)
    {
        _context = context;
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

    public object GetTeamInsights()
    {
        var data = _context.Usuarios
            .AsNoTracking()
            .GroupBy(u => u.Team.Name)
            .Select(g => new
            {
                team = g.Key,
                totalMembers = g.Count(),
                leaders = g.Count(x => x.Team.Leader),
                projectsCompleted = g.SelectMany(x => x.Team.Projects).Count(p => p.Completed),
                activePercentage = g.Count(x => x.Active) == 0 ? 0 : (double)g.Count(x => x.Active) / g.Count() * 100.0
            })
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
        // Placeholder: Could implement additional evaluation metrics if specified later.
        return new { totalUsers = _context.Usuarios.Count() };
    }
}
