using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.BLL.Features.Urls.Commands.Create;
using UrlShortener.BLL.Features.Urls.Commands.Delete;
using UrlShortener.BLL.Features.Urls.Queries.GetAll;
using UrlShortener.BLL.Features.Urls.Queries.GetById;
using UrlShortener.DAL.Enums;
using UrlShortener.Server.Controllers.Common;

namespace UrlShortener.Server.Controllers;

public class UrlsController : BaseApiController
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<UrlResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UrlResponse>>> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllUrlsQuery()));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(UrlDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult<UrlDetailResponse>> GetById(int id)
    {
        return HandleResult(await Mediator.Send(new GetUrlByIdQuery(id)));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(UrlResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UrlResponse>> Create([FromBody] CreateUrlRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.Identity?.Name ?? "Unknown";
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        return HandleResult(await Mediator.Send(new CreateShortUrlCommand(request, userId, userName)));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole(AppRole.Admin.ToString());

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        return HandleResult(await Mediator.Send(new DeleteShortUrlCommand(id, userId, isAdmin)));
    }
}
