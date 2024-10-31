using Microsoft.AspNetCore.Identity;

namespace Lexias.Services.AuthAPI.Models
{
    //all columns from IdentityUser + new Name column = ApplicationUser
    //IdentityUser has many other columns but we want to add more columns in need
    //EF core is smart cuz it knows that we are just extending IdentityUser so it will add our columns to table
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
