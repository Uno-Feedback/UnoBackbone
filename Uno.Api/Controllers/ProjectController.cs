using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Uno.Application.Services;

namespace Uno.Api.Controllers;

public class ProjectController : BaseController
{
    [HttpPost]
    [ApiVersion("1")]

    public async Task<JsonResult> Add([FromBody] AddProjectCommand addProjectCommand, CancellationToken ct = default)
        => Json(await MediatR.Send(addProjectCommand, ct));

    [HttpGet]
    [ApiVersion("1")]

    public async Task<JsonResult> List([FromQuery] GetProjecttListQuery getProjecttListQuery, CancellationToken ct = default)
        => Json(await MediatR.Send(getProjecttListQuery, ct));
}
