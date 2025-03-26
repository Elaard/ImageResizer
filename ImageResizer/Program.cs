using Amazon.DynamoDBv2;
using Amazon.Runtime;
using ImageResizer.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAWSService<IAmazonDynamoDB>();

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var credentials = new BasicAWSCredentials("faceAccessKey", "fakeSecretPassword");

    // Determine if running in Docker
    var isInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    
    // When running in Docker, use host.docker.internal to access the host machine
    // When running locally, use localhost
    var dynamoDbHost = isInDocker ? "host.docker.internal" : "localhost";
    
    Console.WriteLine($"Connecting to DynamoDB at {dynamoDbHost}:8000");
    
    var clientConfig = new AmazonDynamoDBConfig
    {
        RegionEndpoint = Amazon.RegionEndpoint.USEast1,
        ServiceURL = $"http://{dynamoDbHost}:8000"
    };

    return new AmazonDynamoDBClient(credentials, clientConfig);
});

builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
