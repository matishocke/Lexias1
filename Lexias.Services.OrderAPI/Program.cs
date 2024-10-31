using Dapr.Client;
using Dapr.Workflow;
using Lexias.Services.OrderAPI.DaprWorkflow;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDaprClient();
builder.Services.AddDaprWorkflow(options =>
{
    //Workflow
    options.RegisterWorkflow<OrderWorkflow>();

    //Activities
    options.RegisterActivity<CreateOrderActivity>();
    options.RegisterActivity<ReserveItemsActivity>();
    options.RegisterActivity<ProcessPaymentActivity>();
    options.RegisterActivity<ShipItemsActivity>();
    options.RegisterActivity<CompleteOrderActivity>();

    // Compensating Activities
    options.RegisterActivity<BackStockItemsActivity>();
    options.RegisterActivity<UnReserveItemsActivity>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCloudEvents();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapSubscribeHandler();
app.Run();
