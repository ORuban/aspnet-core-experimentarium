using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Experimentarium.AspNetCore.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class TokenExperimentController : Controller
    {
        [Authorize("JWTSampleSchema")]
        [HttpGet("/experiment/token/details")]
        public IActionResult TokenDetails()
        {
            return Ok();
        }

        [HttpGet("/experiment/token")]
        public string CreateToken([FromQuery] string name = "John Doe", [FromQuery] string mail = "e-mail@test.com")
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characters long for HmacSha256"));

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, mail),
                new Claim("custom_claim", "custom data")
            };

            var token = new JwtSecurityToken(
                issuer: "Experimentarium.AspNetCore App",
                audience: "Any client of this app",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(14),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        [ApiVersion("2.0")]
        public class TokenExperimentControllerV2 : Controller
        {
            [Authorize("JWTSampleSchema")]
            [HttpGet("/experiment/token/details")]
            public IActionResult TokenDetails()
            {
                return Ok();
            }

            [HttpGet("/experiment/token")]
            public string CreateToken([FromQuery] string name = "John Doe", [FromQuery] string mail = "e-mail@test.com")
            {
                var claims = new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.NameId, name),
                new Claim(JwtRegisteredClaimNames.Email, mail),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, "Any client of this app"),
                new Claim(JwtRegisteredClaimNames.Iss, "Experimentarium.AspNetCore App"),
                new Claim("custom_claim", "custom data")
                };

                var token = new JwtSecurityToken(
                    new JwtHeader(new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characters long for HmacSha256")),
                                                 SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(claims));

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}