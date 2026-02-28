using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.DTOs.About;
using UrlShortener.BLL.Features.About.Commands.Update;
using UrlShortener.BLL.Features.About.Queries.Get;
using UrlShortener.DAL.Enums;
using UrlShortener.Server.Controllers.Common;

namespace UrlShortener.Server.Controllers;

public class AboutController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(AboutContentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AboutContentResponse>> Get()
    {
        return HandleResult(await Mediator.Send(new GetAboutContentQuery()));
    }

    [HttpPatch]
    [Authorize(Roles = nameof(AppRole.Admin))]
    [ProducesResponseType(typeof(AboutContentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AboutContentResponse>> Update([FromBody] AboutContentUpdateRequest request)
    {
        return HandleResult(await Mediator.Send(new UpdateAboutContentCommand(request)));
    }
}
