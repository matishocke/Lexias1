using Lexias.Services.AuthAPI.Models;
using Lexias.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lexias.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        //_jwtOptions is carring (Secret, Issuer, Audience) values from appsettings.json
        private readonly JwtOptions _jwtOptions; //retrieve Secret Issuer Audience
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;  //_jwtOptions will have all the key values from appsettings.json
        }



        public string GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();         //JwtSecurityTokenHandler is a helper class to create and manage JWTs

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);    //Converts the Secret into a byte array, which will be used to sign the token securely.

            //Claim Inside token = store value we like which could be (Email, Id(Sub), Name)
            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),         //go to jwt.io you can see { "email": "user@example.com" } --- JwtRegisteredClaimNames.Email = "email" this is a part of making a token the token has an email that should has a value which our user can give a value 
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName)
            };


            var tokenDescripter = new SecurityTokenDescriptor          //SecurityTokenDescriptor is a class that defines the properties and settings of a JWT (JSON Web Token) before it’s created. It’s like a blueprint that tells the JWT generator what information to include in the token and how to sign it
            {
                Audience = _jwtOptions.Audience,                //_jwtOptions from appsettings Audience
                Issuer = _jwtOptions.Issuer,                    //_jwtOptions from appsettings Issuer
                Subject = new ClaimsIdentity(claimList),        //claimList from our user values
                Expires = DateTime.UtcNow.AddDays(7),              
                SigningCredentials = new SigningCredentials     //_jwtOptions from appsettings Secret
                                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(token);
        }
    }
}
