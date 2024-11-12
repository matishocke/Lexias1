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
        //Helper Methods to do all the complexity of identity like passwordHash, ... 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        //inject all with dependency injection
        public AuthService(AuthDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        //Login
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            //find the user from applicationUsers table with the same username as loginRequestDto username
            var foundUser =
                _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            //check the foundUser.Password is the same as loginRequestDto.Password
            bool isValid = await _userManager.CheckPasswordAsync(foundUser, loginRequestDto.Password);

            if (foundUser == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }


            //if user was found, Generate JWT Token
            var token = _jwtTokenGenerator.GenerateToken(foundUser);



            UserDto userDto = new()
            {
                Email = foundUser.Email,
                ID = foundUser.Id,
                Name = foundUser.Name,
                PhoneNumber = foundUser.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto() 
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }



        //Register
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
                ///UserManager///<ApplicationUser> class is used to create the user in the database,
                //and it also hashes the password before storing it.
                var userCreatesOrRegisteredWithPassword =
                    await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (userCreatesOrRegisteredWithPassword.Succeeded) //if created
                {
                    //XXXXXXX I think it is extra work XXXXXXXX
                    var userCreatedReturnes = _db.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userCreatedReturnes.Email,
                        ID = userCreatedReturnes.Id,
                        Name = userCreatedReturnes.Name,
                        PhoneNumber = userCreatedReturnes.PhoneNumber
                    };
                    //XXXXXXXXXXXXXX
                    return "";
                }
                else
                {
                    return userCreatesOrRegisteredWithPassword.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }





        //Role
        public async Task<bool> AssignRole(string email, string roleName)
        {
            var foundUser = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (foundUser != null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) //if role does NOT exist
                {
                    //create role
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }


                //Adding the user to the role with _userManager.AddToRoleAsync
                await _userManager.AddToRoleAsync(foundUser, roleName);
                return true;
            }
            return false;
        }


    }
}
