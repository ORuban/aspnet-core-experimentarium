using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Experimentarium.AspNetCore.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger();

            try
            {
                Log.Information("Starting web host");

                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
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
                .UseSerilog()
                .Build();

            return webHost;
        }

        // Look into https://github.com/serilog/serilog-aspnetcore
        private static Serilog.ILogger CreateSerilogLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("serilog/webApi.plain.log")
                .WriteTo.File(new JsonFormatter(), "serilog/webApi.json.log")
                .CreateLogger();
        }
    }
}