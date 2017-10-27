using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Experimentarium.AspNetCore.WebApi
{
    public static class JWTExperimentExtensions
    {
        public static void AddJWTExperiment(this AuthenticationBuilder builder, IHostingEnvironment env)
        {
            var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characters long for HmacSha256"));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                ValidateAudience = true,
                // "the audience you want to validate",
                ValidAudience = "Any client of this app",

                ValidateIssuer = true,
                // "the issuer you want to validate",
                ValidIssuer = "Experimentarium.AspNetCore App",

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date                
            };


            builder.AddJwtBearer("JWTSampleSchema", options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = tokenValidationParameters;

                        options.Events = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = c =>
                            {
                                c.NoResult();

                                c.Response.StatusCode = 500;
                                c.Response.ContentType = "text/plain";
                                if (env.IsDevelopment())
                                {
                                    // Debug only, in production do not share exceptions with the remote host.
                                    return c.Response.WriteAsync(c.Exception.ToString());
                                }
                                return c.Response.WriteAsync("An error occurred processing your authentication.");
                            }
                        };
                    });
        }
    }
}