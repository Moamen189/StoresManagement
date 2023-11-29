using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Models;
using StoreManagement.Services;
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
        private readonly ApplicationDbContext context;

        public AccountController(IConfiguration configuration , ApplicationDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        [HttpPost("Register")]

        public IActionResult Register(UserDto userDto)
        {
            var emailCount = context.Users.Count(u => u.Email == userDto.Email);
            if (emailCount > 1) {
                ModelState.AddModelError("Email", "this email is used before");
                return BadRequest(ModelState);
            
            }

            var PassworedHashed = new PasswordHasher<User>();
            var encryptedPassword = PassworedHashed.HashPassword(new User(), userDto.Password);

            User user = new User()
            {
                firstName = userDto.firstName,
                lastName = userDto.lastName,
                Email = userDto.Email,
                Phone = userDto.Phone ?? "",
                Password = encryptedPassword,
                Address = userDto.Address,
                Role = "client",
                CreatedAt = DateTime.UtcNow,

            };

            context.Users.Add(user);
            context.SaveChanges();
            var jwt = CreateJwtToken(user);
            UserProfileDto userProfileDto = new UserProfileDto()
            {
                Id = user.Id,
                firstName = user.firstName,
                lastName = user.lastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                CreatedAt = DateTime.UtcNow,
            };

            var response = new
            {
                Token = jwt,
                user = userProfileDto
            };
            return Ok(response);
        }
        [HttpPost("login")]
        public IActionResult login (string email , string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if(user == null)
            {
                ModelState.AddModelError("Error", "Email or Password are not valid");
                return BadRequest(ModelState);
            }
            var PasswordHasher = new PasswordHasher<User>();
            var result = PasswordHasher.VerifyHashedPassword(new Models.User() , user.Password, password);
            if (result == PasswordVerificationResult.Failed) {
                ModelState.AddModelError("Password", "Wrong Password");
                return BadRequest(ModelState);
            
            }

            var Jwt = CreateJwtToken(user);
            UserProfileDto userProfileDto = new UserProfileDto()
            {
                Id = user.Id,
                firstName = user.firstName,
                lastName = user.lastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                CreatedAt = DateTime.UtcNow,
            };

            var response = new
            {
                Token = Jwt,
                user = userProfileDto
            };
            return Ok(response);
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
