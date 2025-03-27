using Aggregator.Services;
using Banking;
using Banking.API;
using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register gRPC service with proper configuration
builder.Services.AddGrpcClient<BankingService.BankingServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["BankingApi:GrpcUrl"] ?? "http://localhost:5001");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});

// Register BankingGrpcService
builder.Services.AddScoped<IBankingGrpcService, BankingGrpcService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();