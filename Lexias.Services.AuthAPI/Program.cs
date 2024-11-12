using Lexias.Services.AuthAPI.Data;
using Lexias.Services.AuthAPI.Models;
using Lexias.Services.AuthAPI.Service;
using Lexias.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//we want that JwtOptions class has same Values from appsettings : Secret, Issuer, and Audience values in appsettings.json will be set as properties in the JwtOptions class
//this will also Map the Values to the JwtOptions Class from appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));


//here we configuring identity //this will act as a bridge between entity framework and .Net Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().             
    AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();  



builder.Services.AddControllers();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseHttpsRedirection();

//Authentication comes before Authorization
app.UseAuthentication();   //this been added cuz we need for authentication //Enables JWT authentication.
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();






//Auto Migration 
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    };
}