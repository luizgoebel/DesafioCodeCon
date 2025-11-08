using DesafioCodeCon.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioCodeCon.Controllers;

[ApiController]
//[Route("api/usuario")]
public class UsuariosController : ControllerBase
{

    [HttpPost("users")]
    public IActionResult Users(List<Usuario> usuarios)
    {
        return Ok();
    }

    [HttpGet("superusers")]
    public IActionResult SuperUsers(int score = 900, bool active = true)
    {
        return Ok();
    }

    [HttpGet("top-countries")]
    public IActionResult TopCountries()
    {
        return Ok();
    }

    [HttpGet("team-insights")]
    public IActionResult TeamInsights()
    {
        return Ok();
    }

    [HttpGet("active-users-per-day")]
    public IActionResult ActiveUsersPerDay()
    {
        return Ok();
    }

    [HttpGet("evaluation")]
    public IActionResult Evaluation()
    {
        return Ok();
    }
}
