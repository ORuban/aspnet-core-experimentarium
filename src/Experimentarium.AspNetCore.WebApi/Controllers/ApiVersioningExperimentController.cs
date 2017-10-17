using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

namespace Experimentarium.AspNetCore.WebApi.Controllers.V1
{
    public class VersionController : Controller
    {
        [HttpGet("api/VersionExperiment/version/default")]
        public ApiVersion Get([FromServices] IOptions<ApiVersioningOptions> options)
        {
            return options.Value.DefaultApiVersion;
        }
    } 

    [ApiVersion("1.0")]
    public class VersionExperimentController : Controller
    {
        [HttpGet("api/VersionExperiment/data")]
        public IEnumerable<int> Get()
        {
            return new int[] { 1, 2, 3 };
        }
    }
}
namespace Experimentarium.AspNetCore.WebApi.Controllers.V2
{
    [ApiVersion("2.0")]
    public class VersionExperimentController : Controller
    {
        [HttpGet("api/VersionExperiment/data")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3" };
        }
    }
}