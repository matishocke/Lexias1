using UmbracoLexiasWeb.Services;
using UmbracoLexiasWeb.Services.IService;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();



// Add HttpClient service
builder.Services.AddHttpClient("CouponAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]); // BaseUrl from appsettings.json
});

// Add HttpClient Contact service
builder.Services.AddHttpClient("ContactUsAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrlContactUs"]); // BaseUrl from appsettings.json
});

// Register your custom services
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IContactUsService, ContactUsService>();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
