using Microsoft.AspNetCore.Mvc;

namespace DesafioCodeCon.Controllers;

[ApiController]
//[Route("api/usuario")]
public class UsuariosController : ControllerBase
{

    [HttpPost("users")]
    public IActionResult Users()
    {
        return Ok();
    }

    [HttpGet("superusers")]
    public IActionResult Superusers(int score = 900, bool active = true)
    {
        return Ok();
    }

    [HttpGet("top-countries")]
    public IActionResult TopCcountries()
    {
        return Ok();
    }

    [HttpGet("team-insights")]
    public IActionResult TeamInsights()
    {
        return Ok();
    }

    [HttpGet("active-users-per-day")]
    public IActionResult ActiveUsersPerDay(int? min = 3000)
    {
        return Ok();
    }

    [HttpGet("evaluation")]
    public IActionResult evaluation()
    {
        return Ok();
    }
}
