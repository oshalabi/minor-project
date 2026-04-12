using BasalRation.DAL;
using BasalRation.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// Configure DbContext
var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "Developer";
var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "password";
var databaseName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "BasalFeedDB";
var dbConnectionString =
    $"Host={postgresHost};Port={postgresPort};Database={databaseName};Username={postgresUser};Password={postgresPassword}";
builder.Services.AddDbContext<BasalFeedDB>(options =>
{
    options.UseNpgsql(dbConnectionString);
});

// Add Health Checks
var rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
var rabbitPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitPort = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
var rabbitConnectionType = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_TYPE") ?? "amqp";
var rabbitExchange = Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE") ?? "rantsoenwijzer";
var rabbitQueue = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE") ?? "ImportRation";
var rabbitRoutingKey = Environment.GetEnvironmentVariable("RABBITMQ_ROUTING_KEY") ?? "basalFeed";

var rabbitConnectionString =
    $"{rabbitConnectionType}://{Uri.EscapeDataString(rabbitUser)}:{Uri.EscapeDataString(rabbitPass)}@{rabbitHost}:{rabbitPort}";

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: dbConnectionString,
        name: "postgresql",
        failureStatus: HealthStatus.Unhealthy,
        timeout: TimeSpan.FromSeconds(5))
    .AddRabbitMQ(
        rabbitConnectionString: rabbitConnectionString,
        name: "rabbitmq",
        failureStatus: HealthStatus.Unhealthy,
        timeout: TimeSpan.FromSeconds(5));

// Configure RabbitMQ settings
builder.Services.Configure<BasalRationMessageBroker>(options =>
{
    options.Host = rabbitHost;
    options.Username = rabbitUser;
    options.Port = Convert.ToInt32(rabbitPort);
    options.Password = rabbitPass;
    options.Exchange = rabbitExchange;
    options.Queue = rabbitQueue;
    options.RoutingKey = rabbitRoutingKey;
});

// Register RabbitMQ services
builder.Services.AddSingleton<IRabbitMQConsumer, MessageService>();

// Add other services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Auto migrate database
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<BasalFeedDB>().MigrateDb();
}

try
{
    var messageService = app.Services.GetRequiredService<IRabbitMQConsumer>();
    await messageService.HandleMessageAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to start RabbitMQ consumer: {ex.Message}");
    throw;
}

// Use CORS middleware
app.UseCors();
app.MapControllers();
app.UseHealthChecks("/hc");
app.Run();