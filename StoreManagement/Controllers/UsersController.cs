using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<string> _users = new List<string>() { "Ali", "Qdri", "Fouad", "Saad" };
        [HttpGet]
        public List<string> GetUsers()
        {
            return _users;
        }

        [HttpGet("{id}")]
        public string GetUsers(int id)
        {
            if (id >= 0 && id < _users.Count)
            {

                return _users[id];
            }
            return "";
        }

        [HttpPost]

        public void AddUsers(string userName)
        {

            _users.Add(userName);
        }

        [HttpPut("{id}")]

        public void UpdateUser(int id, string userName)
        {
            if(id >= 0 && id < _users.Count) { _users[id] = userName; }
        }

        [HttpDelete("{id}")]

        public void DeleteUser(int id)
        {
            if (id >= 0 && id < _users.Count) { _users.RemoveAt(id); }
        }
    }
}
