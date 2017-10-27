using System;
using Experimentarium.AspNetCore.WebApi.ConfigurationExperiment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Experimentarium.AspNetCore.WebApi.Controllers;

namespace Experimentarium.AspNetCore.WebApi
{
    public class Startup
    {
        private ILogger _logger;
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILogger<Startup> logger)
        {
            Environment = env;
            Configuration = configuration;
            _logger = logger;
        }

        public IHostingEnvironment Environment { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJWTExperiment(Environment);

            services.Configure<MyOptions>(Configuration.GetSection("MyOptions"));

            services.AddResponseCompression();

            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddMvc();

            // https://www.hanselman.com/blog/ASPNETCoreRESTfulWebAPIVersioningMadeEasy.aspx
            services.AddApiVersioning(options => {
                options.ReportApiVersions = true;
                //By default without specifying api-version the ApiVersionUnspecified error occurres, so:
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2,0);

                options.Conventions.Controller<VersionController>().HasApiVersion(new ApiVersion(1, 0));
                options.Conventions.Controller<VersionController>().HasApiVersion(new ApiVersion(2, 0));
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IApiVersionDescriptionProvider provider)
        {
            applicationLifetime.ApplicationStarted.Register(() => _logger.LogInformation("App started"));
            applicationLifetime.ApplicationStopping.Register(() => _logger.LogInformation("App stopping"));
            applicationLifetime.ApplicationStopped.Register(() => _logger.LogInformation("App stopped"));

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseResponseCompression();

            ExperimentsWithMiddlewareRegistration(app);

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            // By default available by /swagger endpoint
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
        }

        private static void ExperimentsWithMiddlewareRegistration(IApplicationBuilder app)
        {
            app.Map("/middleware/hello", HandleMapHelloWorld);
            
            #region HttpContext Items

            app.Use(async (context, next) =>
            {
                context.Items["item1"] = "value_one";

                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });

            app.Use(async (context, next) =>
            {
                if (context.Items.ContainsKey("item1"))
                {

                }
                await next.Invoke();
            });

            #endregion HttpContext Items

            app.UseEmptyMiddleware();
            // The same middleware could be registered any times.
            // Without adding a separate exptension method, you can register a middleware by .UseMiddleware method:
            // app.UseMiddleware<EmptyMiddleware>();
        }

        private static void HandleMapHelloWorld(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = "Experimentarium API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "Different experiments with ASP.NET Core Web API",
                TermsOfService = "None",
                Contact = new Contact() { Url = "https://github.com/ORuban/aspnet-core-experimentarium" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
