using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        private string CreateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id" , "" + user.Id),
                new Claim("role" , user.Role)

            };

            string secretKey = configuration["JwtSettings:Key"]!;
            var SmentricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signInCredetial = new SigningCredentials(SmentricKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience :configuration["JwtSettings:Audience"],
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials: signInCredetial
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
