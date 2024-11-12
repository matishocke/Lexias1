using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoLexiasWeb.Models;
using UmbracoLexiasWeb.Models.ViewModels;
using UmbracoLexiasWeb.Services.IService;

namespace UmbracoLexiasWeb.Controller
{
    public class ContactUsSurfaceController : SurfaceController
    {
        private readonly IContactUsService _contactUsService;

        public ContactUsSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory, 
            ServiceContext services, 
            AppCaches appCaches, 
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider, 
            IContactUsService contactUsService) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _contactUsService = contactUsService;
        }

        //// GET: Display all contact messages
        //public async Task<IActionResult> Index()
        //{
        //    var messagesDto = await _contactUsService.GetAllContactMessagesAsync();

        //    var contactUsViewModels = messagesDto?.Select(x => new ContactUsViewModel
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        Email = x.Email,
        //        Subject = x.Subject,
        //        Message = x.Message,
        //        CreatedAt = x.CreatedAt
        //    }).ToList();

        //    return View("ContactUs", contactUsViewModels); // Ensure this view exists
        //}

        // POST: Handle form submission to submit a contact message
        [HttpPost]
        public async Task<IActionResult> Submit(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contactUsDto = new ContactUsDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Email = model.Email,
                    Subject = model.Subject,
                    Message = model.Message
                };

                await _contactUsService.CreateContactMessageAsync(contactUsDto);
                TempData["Success"] = "Message submitted successfully!";

                return Redirect("/contact-us");
            }

            return CurrentUmbracoPage(); // Return to the current page with validation errors
        }
    }
}
