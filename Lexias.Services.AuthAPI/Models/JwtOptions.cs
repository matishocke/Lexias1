namespace Lexias.Services.AuthAPI.Models
{

    public class JwtOptions
    {

        //Who created and signed this token
        public string Issuer { get; set; } = string.Empty;

        //who or what the token is needed for
        public string Audience { get; set; } = string.Empty;

        //Secret is a unique string used to sign and verify JWT tokens
        public string Secret { get; set; } = string.Empty;
    }
}
