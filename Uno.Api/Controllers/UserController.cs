using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Uno.Application.Services;

namespace Uno.Api.Controllers;

public class UserController : BaseController
{
    [HttpPost]
    [ApiVersion("1")]

    public async Task<JsonResult> Add([FromBody] AddUserCommand addUserCommand, CancellationToken ct = default)
        => Json(await MediatR.Send(addUserCommand, ct));
}
