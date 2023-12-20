using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetUsers() {
         var users = context.Users.OrderByDescending(u => u.Id).ToList();
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
        return Ok(userProfileDto);
        }
    }
}
 