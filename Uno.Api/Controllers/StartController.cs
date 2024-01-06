using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Uno.Api.Controllers
{
    public class StartController : BaseController
    {
        private readonly IWebHostEnvironment _webHost;
        

        public StartController(IWebHostEnvironment webHost)
            => _webHost = webHost;

        [HttpGet]
        [ApiVersion("1")]

        public IActionResult Index()
            => Ok($"Welcome to {_webHost.ApplicationName} in {_webHost.EnvironmentName} Mode ... DateTime: {DateTime.Now}");
    }
}
