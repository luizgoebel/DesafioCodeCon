using DesafioCodeCon.Context;
using DesafioCodeCon.Models;
using DesafioCodeCon.Services.Iservices;

namespace DesafioCodeCon.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UsuarioDbContext _context;

    public UsuarioService(UsuarioDbContext context)
    {
        _context = context;
    }

    public object GetEvaluation()
    {
        throw new NotImplementedException();
    }

    public object GetSuperUsers(int score, bool active)
    {
        throw new NotImplementedException();
    }

    public object GetTeamInsights()
    {
        throw new NotImplementedException();
    }

    public object GetTopCountries()
    {
        throw new NotImplementedException();
    }

    public void PostUsers(List<Usuario> usuarios)
    {
        throw new NotImplementedException();
    }
}
