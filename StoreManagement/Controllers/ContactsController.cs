using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManagement.Models;
using StoreManagement.Services;
using System.Linq;

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
        [Authorize (Roles ="admin")]
        [HttpGet]
        public IActionResult GetContacts(int? page) { 
            if(page == null || page < 1)
            {
                page =1;
            }
            var pageSize = 5;
            var totalPage = 0;
            decimal count = context.Contacts.Count();
            totalPage = (int)Math.Ceiling(count / pageSize);
        
            var contacts = context.Contacts
                .Include(x=> x.Subject)
                .OrderByDescending(c => c.Id)
                .Skip((int)(page -1 ) * pageSize)
                .Take(pageSize)
                .ToList();
            var response = new
            {
                Contacts = contacts,
                TotalPage = totalPage,
                PageSize = pageSize,
                Page = page
            };

            return Ok(response);
        }
        [Authorize(Roles ="admin")]
        [HttpGet("{id}")]

        public IActionResult GetContact(int id)
        {
            var contact = context.Contacts.Include(x => x.Subject).FirstOrDefault(x => x.Id == id);
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


        //[HttpPut("{id}")]
        //public IActionResult UpdateContact(int id , ContactDto contactDto)
        //{
        //    var Subject = context.Subjects.Find(contactDto.SubjectId);
        //    if (Subject == null)
        //    {
        //        ModelState.AddModelError("Subject", "Please Enter A Valid Subject");
        //        return BadRequest(ModelState);
        //    }
        //    var contact = context.Contacts.Find(id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }


        //    contact.LastName = contactDto.LastName;
        //    contact.FirstName = contactDto.FirstName;
        //    contact.Email = contactDto.Email;
        //    contact.Subject = Subject;
        //    contact.Phone = contactDto.Phone??"";
        //    contact.Message = contactDto.Message;             
        //    context.SaveChanges();
        //    return Ok(contact);
        //}

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
