using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeXpress.Business.Auth;

namespace TradeXpress.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register/individual")]
    public async Task<IActionResult> RegisterIndividual(RegisterIndividualCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { userId });
    }

    [HttpPost("register/company")]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var loginResult = await _mediator.Send(command);
        return Ok(loginResult);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst("sub")?.Value;
        var email = User.FindFirst("email")?.Value;
        return Ok(new { userId, email });
    }
}
