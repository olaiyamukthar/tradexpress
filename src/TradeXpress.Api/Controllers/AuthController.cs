using MediatR;
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
}