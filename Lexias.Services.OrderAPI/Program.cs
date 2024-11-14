using Dapr.Client;
using Dapr.Workflow;
using Lexias.Services.OrderAPI.DaprWorkflow;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities;
using Microsoft.EntityFrameworkCore;
using Lexias.Services.OrderAPI.Data;


var builder = WebApplication.CreateBuilder(args);



#region Dapr setup
// Dapr uses a random port for gRPC by default. If we don't know what that port
// is (because this app was started separate from dapr), then assume 50001.
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");
if (string.IsNullOrEmpty(daprGrpcPort))
{
    Console.WriteLine("DAPR_GRPC_PORT not set. Assuming 50001.");
    daprGrpcPort = "50001";
    Environment.SetEnvironmentVariable("DAPR_GRPC_PORT", daprGrpcPort);
}

builder.Services.AddControllers()
    // Kun ved explicit pubsub 
    .AddDapr(config => config
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));
#endregion


// Register AppDbContext properly for Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  // Make sure connection string exists

//builder.Services.AddDaprClient();


builder.Services.AddDaprWorkflow(options =>
{
    //Workflow
    options.RegisterWorkflow<OrderWorkflow>();

    // Success Activity
    options.RegisterActivity<CreateOrderActivity>();
    options.RegisterActivity<NotifyActivity>();
    options.RegisterActivity<ReserveItemsActivity>();
    options.RegisterActivity<ProcessPaymentActivity>();
    //options.RegisterActivity<ShipItemsActivity>();
    //options.RegisterActivity<CompleteOrderActivity>();

    // Compensating Activities
    options.RegisterActivity<DeleteOrderActivity>();
    options.RegisterActivity<UnReserveItemsActivity>();
});

//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



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


//app.UseCloudEvents();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
//app.MapSubscribeHandler();
app.Run();
