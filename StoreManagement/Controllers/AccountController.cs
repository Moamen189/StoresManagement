using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Models;
using StoreManagement.Services;
using System.ComponentModel.DataAnnotations;
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

        [Authorize]
        [HttpGet("AuthorizeAuthenticatedusers")]
        public IActionResult GetAuthorizeAuthenticatedusers()
        {
            return Ok("You are Authorize");
        }


        [Authorize (Roles ="admin")]
        [HttpGet("AuthorizeAuthenticatedAdmins")]
        public IActionResult AuthorizeAuthenticatedAdmins()
        {
            return Ok("You are Authorize");
        }

        [Authorize (Roles ="admin , seller")]
        [HttpGet("AuthorizeAuthenticatedAdminAndSellers")]
        public IActionResult AuthorizeAuthenticatedAdminAndSellers()
        {
            return Ok("You are Authorize");
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

        [HttpPost("ForgetPassword")]

        public IActionResult ForgetPassword(string email)
        {
            var Useremail = context.Users.FirstOrDefault(u => u.Email == email);

            if (Useremail == null)
            {
                return NotFound();
            }

            var oldPassword = context.PasswordResets.FirstOrDefault(u => u.Email == email);
            if (oldPassword != null) {
                context.Remove(oldPassword); 
            
            }

            string token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();

            var PasswordReset = new PasswordReset()
            {
                Email = email,
                Token = token,
                CreatedAt = DateTime.Now
            };
            context.PasswordResets.Add(PasswordReset);
            context.SaveChanges();

            var EmailSubject = "Password Reset";

            var userName = Useremail.firstName + " " + Useremail.lastName;

            var emailMessage = "Dear : " + userName + "\n" +
                "We Recevied your Passwored Reset Request \n" +
                "Please copy this Token and Paste this in the Password Reset Form: " + token;
            return Ok(emailMessage);
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(string token, string Password)
        {
            var PasswordReset = context.PasswordResets.FirstOrDefault(r => r.Token == token);
            if (PasswordReset == null)
            {
                ModelState.AddModelError("Token", "Worng or Expired Token");
                return BadRequest(ModelState);
            }

            var user = context.Users.FirstOrDefault(e => e.Email == PasswordReset.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Worng or Expired Email");
                return BadRequest(ModelState);
            }
            var PassworedHasher = new PasswordHasher<User>();
            var encryptedPassword = PassworedHasher.HashPassword(new User(), Password);
            user.Password = encryptedPassword;

            context.PasswordResets.Remove(PasswordReset);
            context.SaveChanges();
            return Ok();

           
        }

        [Authorize]
        [HttpGet("Profile")]
        public IActionResult GetProfile()
        {
            int id = JwtReader.getUserId(User);

            var user = context.Users.Find(id);
            if (user == null)
            {
                return Unauthorized();
            }

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

            return Ok(userProfileDto);
        }
        [Authorize]
        [HttpPut("UpdateProfile")]

        public IActionResult UpdateProfile(UserProfileUpdate userProfileUpdate) {
            int id = JwtReader.getUserId(User);

            var user = context.Users.Find(id);
            if (user == null)
            {
                return Unauthorized();
            }
            user.firstName = userProfileUpdate.firstName;
            user.lastName = userProfileUpdate.lastName;
            user.Email = userProfileUpdate.Email;
            user.Phone = userProfileUpdate.Phone ?? "";
            user.Address = userProfileUpdate.Address;

            context.SaveChanges();
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
            return Ok(userProfileDto);
        }
        [Authorize]
        [HttpPut("UpdatePassword")]

        public IActionResult UpdatePassword([Required , MinLength(8) , MaxLength(100)]string password)
        {
            int id = JwtReader.getUserId(User);

            var user = context.Users.Find(id);
            if (user == null)
            {
                return Unauthorized();
            }

            var PassworedHasher = new PasswordHasher<User>();
            var encryptedPassword = PassworedHasher.HashPassword(new Models.User(), password);
            user.Password = encryptedPassword;
            context.SaveChanges();

            return Ok();
        }
        [Authorize]
        [HttpGet("GetTokenClaim")]
        public IActionResult GetTokenClaim()
        {

            var Identity = User.Identity as ClaimsIdentity;
            if (Identity != null)
            {
                Dictionary<string, string> Claims = new Dictionary<string, string>();

                foreach (Claim claim in Identity.Claims)
                {
                    Claims.Add(claim.Type, claim.Value);
                }
                return Ok(Claims);

            }
            return Ok();
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
