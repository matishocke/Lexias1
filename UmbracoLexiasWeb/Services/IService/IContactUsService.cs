using UmbracoLexiasWeb.Models;

namespace UmbracoLexiasWeb.Services.IService
{
    public interface IContactUsService
    {
        Task CreateContactMessageAsync(ContactUsDto contactUsDto);
        Task<IEnumerable<ContactUsDto>?> GetAllContactMessagesAsync();
    }
}
