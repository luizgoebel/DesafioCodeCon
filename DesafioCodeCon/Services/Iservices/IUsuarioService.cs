using DesafioCodeCon.Models;

namespace DesafioCodeCon.Services.Iservices;

public interface IUsuarioService
{
    void PostUsers(List<Usuario> usuarios);
    object GetSuperUsers(int score, bool active);
    object GetTopCountries();
    object GetTeamInsights();
    object GetEvaluation();
}
