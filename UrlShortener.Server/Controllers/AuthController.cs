using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.DTOs.Auth;
using UrlShortener.BLL.Features.Auth.Commands.Login;
using UrlShortener.BLL.Features.Auth.Commands.Register;
using UrlShortener.Server.Controllers.Common;

namespace UrlShortener.Server.Controllers;


public class AuthController : BaseApiController
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        return HandleResult(await Mediator.Send(new LoginCommand(request)));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        return HandleResult(await Mediator.Send(new RegisterCommand(request)));
    }
}
