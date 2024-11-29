using Lexias.Services.PaymentAPI.Data;
using Lexias.Services.PaymentAPI.Data.Repository.IRepository;
using Lexias.Services.PaymentAPI.Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Aspire
builder.AddServiceDefaults();

// Add services to the container.

#region Dapr setup
// Dapr uses a random port for gRPC by default. If we don't know what that port
// is (because this app was started separate from dapr), then assume 50001.
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");
if (string.IsNullOrEmpty(daprGrpcPort))
{
    daprGrpcPort = "50001";
    Environment.SetEnvironmentVariable("DAPR_GRPC_PORT", daprGrpcPort);
}

// Kun ved explicit pubsub 
builder.Services.AddControllers()
    // Kun ved explicit pubsub 
    .AddDapr(config => config
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));
#endregion


builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();



builder.Services.AddDbContext<AppDbContextPayment>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.MigrationsAssembly("Lexias.Services.PaymentAPI"))
);


// Add Dapr client for interacting with Dapr services
builder.Services.AddDaprClient();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


#region Dapr middelware setup
// app.UseHttpsRedirection(); // Dapr uses HTTP, so no need for HTTPS
app.UseCloudEvents();
app.MapSubscribeHandler(); // Kun ved explicit pubsub
#endregion


//app.UseHttpsRedirection();
app.UseAuthorization();


app.MapControllers();
app.Run();
