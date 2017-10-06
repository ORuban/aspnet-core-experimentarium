using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Experimentarium.AspNetCore.WebApi.ConfigurationExperiment
{
    public class ConfigurationExperimentController : Controller
    {
        public ConfigurationExperimentController(IOptionsMonitor<MyOptions> optionsMonitor)
        {
            var listeningCancelation = optionsMonitor.OnChange( 
                (myOptions, value) => { Console.WriteLine($"MyOptions section has been changed"); }
                );

            // listeningCancelation.Dispose();
        }

        [HttpGet("/api/experiment/configuration")]
        public dynamic GetSettings([FromServices] IOptions<MyOptions> myOptions
                                    ,[FromServices] IOptionsSnapshot<MyOptions> myOptionsSnapshot
                                    )
        {
            return new {
                IOptions = myOptions.Value,
                IOptionsSnapshot = myOptionsSnapshot.Value
            };
        }
    }
}