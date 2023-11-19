using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;
using System.Reflection.Metadata.Ecma335;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<UserDto> _users = new List<UserDto>() { 
        
            new UserDto()
            {
                FirstName = "Ali",
                LastName = "Sayed",
                Email = "Ali@yahoo.com",
                Phone ="01056456565",
                Address="Nasr City"
            },

            new UserDto()
            {
                FirstName = "Fouad",
                LastName = "Magdy",
                Email = "Fouash@yahoo.com",
                Phone ="01056556985",
                Address="Maadi City"
            }

        };
        [HttpGet]
        public IActionResult GetUsers()
        {
            if(_users.Count > 0)
            {

            return Ok(_users);
            }
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUsers(int id)
        {
            if (id >= 0 && id < _users.Count)
            {

                return Ok(_users[id]);
            }
            return NotFound();
        }

        [HttpPost]

        public IActionResult AddUsers(UserDto user)
        {
            if(user.Email.Equals("user@example.com "))
            {
                ModelState.AddModelError("Email", "this Email Address is Not Authorized");
                return BadRequest(ModelState);
            }

            _users.Add(user);
            return Ok();
        }

        [HttpGet("{name:string}")]

        public IActionResult GetUser(string name)
        {
            var user =  _users.FirstOrDefault(u => u.FirstName.Contains(name) || u.LastName.Contains(name));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateUser(int id, UserDto user)
        {
            if (user.Email.Equals("user@example.com "))
            {
                ModelState.AddModelError("Email", "this Email Address is Not Authorized");
                return BadRequest(ModelState);
            }
            if (id >= 0 && id < _users.Count) { 
                _users[id] = user; 
            
            }
            return Ok();

        }

        [HttpDelete("{id}")]

        public IActionResult DeleteUser(int id)
        {
            if (id >= 0 && id < _users.Count) { _users.RemoveAt(id); }
            return NoContent();
        }
    }
}
