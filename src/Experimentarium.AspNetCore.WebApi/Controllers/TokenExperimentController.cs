using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Experimentarium.AspNetCore.WebApi.Controllers
{
    public class TokenExperimentController : Controller
    {
        [HttpGet("/experiment/token")]
        public string CreateToken([FromQuery] string name = "John Doe", [FromQuery] string mail = "e-mail@test.com")
        {
            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("a secret that needs to be at least 16 characters long"));

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, mail),
            };

            var token = new JwtSecurityToken(
                issuer: "Experimentarium.AspNetCore App",
                audience: "Any client of this app",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(14),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwtToken;
        }
    }
}