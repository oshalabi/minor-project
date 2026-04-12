using DotNetEnv;
using EnergyFood.DAL;
using EnergyFood.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

Env.Load(); // Load environment variables from .env file

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
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
var databaseName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "EnergyFoodDB";
var dbConnectionString =
    $"Host={postgresHost};Port={postgresPort};Database={databaseName};Username={postgresUser};Password={postgresPassword}";
builder.Services.AddDbContext<EnergyFoodDB>(options =>
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
    options.Port = int.TryParse(rabbitPort, out var port) ? port : 5672;
    options.Username = rabbitUser;
    options.Password = rabbitPass;
    options.Exchange = rabbitExchange;
    options.Queue = rabbitQueue;
    options.RoutingKey = rabbitRoutingKey;
});

// Register RabbitMQ consumer service
builder.Services.AddSingleton<IRabbitMQConsumer, MessageService>();

// Register controllers and other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Use development tools if in development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EnergyFood API - v1"); });
}

// Automatically migrate the database
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<EnergyFoodDB>();
        dbContext.MigrateDb();
        Console.WriteLine("Database migration completed successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database migration failed: {ex.Message}");
    throw;
}

// Start RabbitMQ consumer
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

// Configure middleware
app.UseCors();
app.UseRouting();
app.MapControllers();
app.UseHealthChecks("/hc");

// Run the application
app.Run();
