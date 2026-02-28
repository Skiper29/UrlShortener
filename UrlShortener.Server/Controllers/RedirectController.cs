using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.Features.Urls.Queries.GetByShortCode;
using UrlShortener.Server.Controllers.Common;

namespace UrlShortener.Server.Controllers;

public class RedirectController : BaseApiController
{
    [HttpGet("{shortCode}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status302Found)]
    public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
    {
        var result = await Mediator.Send(new GetUrlByShortCodeQuery(shortCode));
        if (result.IsSuccess)
        {
            var originalUrl = result.Value.OriginalUrl;
            return Redirect(originalUrl);
        }
        return NotFound();
    }
}
