using UmbracoLexiasWeb.Models;
using UmbracoLexiasWeb.Services.IService;

namespace UmbracoLexiasWeb.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactUsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }




        public async Task CreateContactMessageAsync(ContactUsDto contactUsDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ContactUsAPI");
                var response = await client.PostAsJsonAsync("api/ContactUs/submit", contactUsDto);

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating contact message: {message}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }




        public async Task<IEnumerable<ContactUsDto>?> GetAllContactMessagesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ContactUsAPI");
                var messages = await client.GetFromJsonAsync<IEnumerable<ContactUsDto>>("api/ContactUs/index");
                return messages;
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
