
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Uno.Application.Services;

namespace Uno.Api.Controllers;


public class ConnectorController : BaseController
{
    [HttpPost]
    [ApiVersion("1")]

    public async Task<JsonResult> Add([FromBody] AddConnectorCommand addConnectorCommand, CancellationToken ct = default)
        => Json(await MediatR.Send(addConnectorCommand, ct));

    [HttpGet]
    [ApiVersion("1")]

    public async Task<JsonResult> ListMetaData([FromQuery] GetConnectorMetaDataQuery getConnectorMetaDataQuery, CancellationToken ct = default)
        => Json(await MediatR.Send(getConnectorMetaDataQuery, ct));

    [HttpGet]
    [ApiVersion("1")]
    public async Task<JsonResult> List([FromQuery] ListConnectorQuery listConnectorQuery, CancellationToken ct = default)
        => Json(await MediatR.Send(listConnectorQuery, ct));
}
