using DesafioCodeCon.Models;
using DesafioCodeCon.Services.Iservices;
using Microsoft.AspNetCore.Mvc;

namespace DesafioCodeCon.Controllers;

[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("users")]
    public IActionResult PostUsers([FromBody] List<Usuario> usuarios)
    {
        bool saved = _usuarioService.PostUsers(usuarios);
        if (saved)
            return Ok();
        return BadRequest();
    }

    [HttpGet("superusers")]
    public IActionResult SuperUsers(int score = 900, bool active = true)
    {
        try
        {
            var response = _usuarioService.GetSuperUsers(score, active);
            if (response == null)
                return BadRequest();
            return Ok(response);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("top-countries")]
    public IActionResult TopCountries()
    {
        try
        {
            var response = _usuarioService.GetTopCountries();
            if (response == null)
                return BadRequest();
            return Ok(response);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("team-insights")]
    public async Task<IActionResult> TeamInsights(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _usuarioService.GetTeamInsights(cancellationToken);
            if (response == null)
                return BadRequest();
            return Ok(response);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("active-users-per-day")]
    public IActionResult ActiveUsersPerDay([FromQuery] int? min)
    {
        try
        {
            var response = _usuarioService.GetActiveUsersPerDay(min);
            if (response == null)
                return BadRequest();
            return Ok(response);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("evaluation")]
    public IActionResult Evaluation()
    {
        var result = _usuarioService.GetEvaluation();

        return Ok(result);
    }
}
