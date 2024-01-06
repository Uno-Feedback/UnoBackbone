using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Uno.Api.Controllers;

[Route("v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public abstract class BaseController : Controller
{
    private ISender? _mediatR;
    protected ISender MediatR => _mediatR ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
