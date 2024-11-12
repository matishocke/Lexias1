namespace Lexias.Services.AuthAPI.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        
        //token: validate a user that is logged in
        public string Token { get; set; }
    }
}
