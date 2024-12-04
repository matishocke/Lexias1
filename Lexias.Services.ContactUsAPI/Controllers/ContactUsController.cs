using Lexias.Services.ContactUsAPI.Data;
using Lexias.Services.ContactUsAPI.Models;
using Lexias.Services.ContactUsAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lexias.Services.ContactUsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly ContactUsDbContext _context;

        public ContactUsController(ContactUsDbContext context)
        {
            _context = context;
        }








        // Endpoint to receive and store contact messages
        [HttpPost("submit")]
        public ActionResult PostContactMessage(ContactUsDto contactUsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            // Map DTO to Entity
            var contactUs = new ContactUs
            {
                Name = contactUsDto.Name,
                Email = contactUsDto.Email,
                Subject = contactUsDto.Subject,
                Message = contactUsDto.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ContactUs.Add(contactUs);
            _context.SaveChangesAsync();

            return Ok("Message received.");
        }





        // Endpoint to display all contact messages
        [HttpGet("index")]
        public ActionResult GetAllContactMessages()
        {
            var messages = _context.ContactUs.ToList();

            // Map Entity to DTO
            var messagesDto = messages.Select(m => new ContactUsDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Subject = m.Subject,
                Message = m.Message,
                CreatedAt = m.CreatedAt
            }).ToList();

            return Ok(messagesDto);
        }
    }
}
