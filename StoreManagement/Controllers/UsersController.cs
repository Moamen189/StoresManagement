using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<string> _users = new List<string>() { "Ali", "Qdri", "Fouad", "Saad" };
        [HttpGet]
        public IActionResult GetUsers()
        {
            if(_users.Count > 0)
            {

            return Ok(_users);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetUsers(int id)
        {
            if (id >= 0 && id < _users.Count)
            {

                return Ok(_users[id]);
            }
            return NotFound();
        }

        [HttpPost]

        public IActionResult AddUsers(string userName)
        {

            _users.Add(userName);
            return Ok();
        }

        [HttpPut("{id}")]

        public IActionResult UpdateUser(int id, string userName)
        {
            if(id >= 0 && id < _users.Count) { 
                _users[id] = userName; 
            
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
