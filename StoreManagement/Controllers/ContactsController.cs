using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ContactsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetContacts() { 
        
            var contacts = context.Contacts.ToList();

            return Ok(contacts);
        }

        [HttpGet("{id}")]

        public IActionResult GetContact(int id)
        {
            var contact = context.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        //[HttpPost]
    }
}
