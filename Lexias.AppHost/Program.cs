using Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

// Dapr Components
var stateStore = builder.AddDaprStateStore("statestore"); // State persistence
var workflowChannel = builder.AddDaprPubSub("workflowchannel"); // Workflow events
var paymentChannel = builder.AddDaprPubSub("paymentchannel");   // Payment communication
var warehouseChannel = builder.AddDaprPubSub("warehousechannel"); // Inventory reservation

// Add OrderAPI service
builder.AddProject<Projects.Lexias_Services_OrderAPI>("lexias-services-orderapi")
    .WithDaprSidecar()                               // Starts Dapr sidecar alongside the service
    .WithReference(stateStore)       // Enables state storage for OrderAPI
    .WithReference(workflowChannel)  // OrderAPI interacts with the workflow channel
    .WithReference(paymentChannel)   // Publishes payment requests
    .WithReference(warehouseChannel); // Publishes inventory reservation requests

// Add PaymentAPI service
builder.AddProject<Projects.Lexias_Services_PaymentAPI>("lexias-services-paymentapi")
    .WithDaprSidecar()               // Starts Dapr sidecar for PaymentAPI
    .WithReference(workflowChannel)  // Sends payment results back to the workflow
    .WithReference(paymentChannel);  // Processes payment requests

// Add WarehouseAPI service
builder.AddProject<Projects.Lexias_Services_WarehouseAPI>("lexias-services-warehouseapi")
    .WithDaprSidecar()               // Starts Dapr sidecar for WarehouseAPI
    .WithReference(workflowChannel)  // Sends reservation results back to the workflow
    .WithReference(warehouseChannel); // Processes inventory reservation requests



builder.AddProject<Projects.API_Gateway>("api-gateway");


builder.AddProject<Projects.Lexias_Services_CouponAPI>("lexias-services-couponapi");


builder.AddProject<Projects.Lexias_Services_ContactUsAPI>("lexias-services-contactusapi");


builder.Build().Run();
