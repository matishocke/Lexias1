using System.ComponentModel.DataAnnotations;

namespace UmbracoLexiasWeb.Models.ViewModels
{
    public class ContactUsViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "not valid email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
