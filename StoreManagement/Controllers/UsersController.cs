using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using StoreManagement.Models;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UsersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetUsers(int? page) {
            if(page == null || page <1) {
                page = 1;
            }

            int pageSize  = 5 ;
            
            int totalPages = 0;

            decimal PageCount = context.Users.Count();

            totalPages = (int)Math.Ceiling(PageCount / pageSize);
         var users = context.Users.Skip((int)(page - 1) * pageSize ).Take(pageSize).OrderByDescending(u => u.Id).ToList();
         List<UserProfileDto> userProfileDto = new List<UserProfileDto>();
        foreach (var user in users)
            {
                var userProfiles = new UserProfileDto()
                {
                    Id = user.Id,
                    firstName = user.firstName, 
                    lastName = user.lastName,
                    Email = user.Email,
                    Address = user.Address,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
                userProfileDto.Add(userProfiles);
            }
            var response = new
            {
                users = userProfileDto,
                totalPages = totalPages,
                Page = page,
                pageSize = pageSize,

            };
        return Ok(response);
        }

        [HttpGet("{id}")]

        public IActionResult GetUser(int id)
        {
            var user = context.Users.Find(id);

            if(user == null)
            {
                return BadRequest();
            }
            var userProfiles = new UserProfileDto()
            {
                Id = user.Id,
                firstName = user.firstName,
                lastName = user.lastName,
                Email = user.Email,
                Address = user.Address,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
            return Ok(userProfiles);
        }
    }
}
 