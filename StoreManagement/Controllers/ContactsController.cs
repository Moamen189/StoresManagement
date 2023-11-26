using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;
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

        [HttpGet("subject")]
        public IActionResult GetSubject()
        {
            var Subjects = context.Subjects.ToList();
            return Ok(Subjects);
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

        [HttpPost]

        public IActionResult CreateContact(ContactDto contactDto)
        {
            var Subject = context.Subjects.Find(contactDto.SubjectId);
            if (Subject == null)
            {
                ModelState.AddModelError("Subject", "Please Enter A Valid Subject");
                return BadRequest(ModelState);
            }
            Contact contact = new Contact()
            {
                FirstName= contactDto.FirstName,
                LastName= contactDto.LastName,
                Email= contactDto.Email,
                Phone = contactDto.Phone??"",
                Subject = Subject,
                Message= contactDto.Message,
                CreatedAt = DateTime.Now
            };
            context.Contacts.Add(contact);
            context.SaveChanges();
            return Ok(contact);

        }


        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id , ContactDto contactDto)
        {
            var Subject = context.Subjects.Find(contactDto.SubjectId);
            if (Subject == null)
            {
                ModelState.AddModelError("Subject", "Please Enter A Valid Subject");
                return BadRequest(ModelState);
            }
            var contact = context.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }


            contact.LastName = contactDto.LastName;
            contact.FirstName = contactDto.FirstName;
            contact.Email = contactDto.Email;
            contact.Subject = Subject;
            contact.Phone = contactDto.Phone??"";
            contact.Message = contactDto.Message;             
            context.SaveChanges();
            return Ok(contact);
        }

        [HttpDelete]
        public IActionResult DeleteContact(int id)
        {
            //var contact = context.Contacts.Find(id);
            //if (contact == null)
            //{
            //    return NotFound();

            //}
            try
            {
                var contact = new Contact() { Id = id , Subject = new Subject()};
                context.Contacts.Remove(contact);
                context.SaveChanges();
            }catch(Exception)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
