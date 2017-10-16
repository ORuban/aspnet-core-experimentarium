using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Experimentarium.AspNetCore.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class VersionExperimentController : Controller
    {
        [HttpGet("api/VersionExperiment/value")]
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
        [HttpGet("api/VersionExperiment/value")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3" };
        }
    }
}