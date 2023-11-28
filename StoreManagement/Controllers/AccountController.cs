using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;
using System.Security.Claims;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private string CreateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id" , "" + user.Id),
                new Claim("role" , user.Role)

            };

        }
    }
}
