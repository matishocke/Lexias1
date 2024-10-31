namespace Lexias.Services.AuthAPI.Models.Dto
{
    public class UserDto
    {
        //these model are needed when we register a user
        public string ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
