using Lexias.Services.AuthAPI.Data;
using Lexias.Services.AuthAPI.Models;
using Lexias.Services.AuthAPI.Models.Dto;
using Lexias.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Lexias.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //inject all with dependency injection
        public AuthService(AuthDbContext db, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;

            
        }
        


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }




        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };
            try
            {
                //UserManager<ApplicationUser> class is used to create the user in the database, and it also hashes the password before storing it.
                var userCreatesWithPassword = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (userCreatesWithPassword.Succeeded)
                {
                    var userCreatedReturnes = _db.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userCreatedReturnes.Email,
                        ID = userCreatedReturnes.Id,
                        Name = userCreatedReturnes.Name,
                        PhoneNumber = userCreatedReturnes.PhoneNumber
                    };

                    return "";
                }
                else 
                {
                    return userCreatesWithPassword.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
