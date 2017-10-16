﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Experimentarium.AspNetCore.WebApi.ConfigurationExperiment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;

namespace Experimentarium.AspNetCore.WebApi
{
    public class Startup
    {
        private ILogger _logger;
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<MyOptions>(Configuration.GetSection("MyOptions"));

            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { 
                    Title = "Experimentarium API V1", 
                    Version = "v1", 
                    Description = "Different experiments with ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact(){ Url = "https://github.com/ORuban/aspnet-core-experimentarium" }
                    });

                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStarted.Register(() => _logger.LogInformation("App started"));
            applicationLifetime.ApplicationStopping.Register(() => _logger.LogInformation("App stopping"));
            applicationLifetime.ApplicationStopped.Register(() => _logger.LogInformation("App stopped"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            // By default available by /swagger endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Experimentarium API V1");
            });

            app.UseMvc();
        }
    }
}
