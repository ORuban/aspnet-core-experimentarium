using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Experimentarium.AspNetCore.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            // The ConfigureAppConfiguration was intended to configure the IConfiguration in the application services 
            // whereas UseConfiguration is intended to configure the settings of the WebHostBuilder.
            // Since the url address is a setting on the WebHostBuilder only UseConfiguration will work here.
            // More here https://github.com/aspnet/Hosting/issues/1148
            var argsConfig = new ConfigurationBuilder().AddCommandLine(args).Build();

            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(argsConfig)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .UseStartup<Startup>()
                .Build();

            return webHost;
        }
    }
}