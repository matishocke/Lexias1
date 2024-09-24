using Lexias.Web.Service.IService;
using Lexias.Web.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//HTTPCLIENT
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

//builder.Services.AddHttpClient<ICouponService, CouponService>(client =>
//client.BaseAddress = new Uri(builder.Configuration["BaseUrl"])
//);

builder.Services.AddHttpClient("CouponAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]); // Uses the BaseUrl from appsettings.json
});


//Interfaces
builder.Services.AddScoped<ICouponService, CouponService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
