using DesafioCodeCon.Models;
using System.Threading;

namespace DesafioCodeCon.Services.Iservices;

public interface IUsuarioService
{
    bool PostUsers(List<Usuario> usuarios);
    object GetSuperUsers(int score, bool active);
    object GetTopCountries();
    Task<object> GetTeamInsights(CancellationToken cancellationToken = default);
    object GetEvaluation();
    object GetActiveUsersPerDay(int? min);
}
