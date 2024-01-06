using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Uno.Application.Services;

namespace Uno.Api.Controllers;

public class IssueController : BaseController
{
    [HttpPost]
    [ApiVersion("1")]

    public async Task<JsonResult> Add([FromForm] AddIssueCommand issueCommand, CancellationToken ct = default)
        => Json(await MediatR.Send(issueCommand, ct));


    [HttpPost]
    [ApiVersion("1")]

    public async Task<JsonResult> Send([FromBody] SendIssueCommand issueCommand, CancellationToken ct = default)
        => Json(await MediatR.Send(issueCommand, ct));
}
