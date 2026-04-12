using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ration.DAL;
using Ration.RabbitMQ;
using Ration.Service;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<RationService>();
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
// Register the DbContext
var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "Developer";
var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "password";
var databaseName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "BasalFeedDB";

var dbConnectionString =
    $"Host={postgresHost};Port={postgresPort};Database={databaseName};Username={postgresUser};Password={postgresPassword}";

builder.Services.AddDbContext<RationDB>(options => { options.UseNpgsql(dbConnectionString); });

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

// // Configure RabbitMQ settings
// builder.Services.Configure<BasalRationMessageBroker>(options =>
// {
//     options.Host = rabbitHost;
//     options.Username = rabbitUser;
//     options.Port = Convert.ToInt32(rabbitPort);
//     options.Password = rabbitPass;
//     options.Exchange = rabbitExchange;
//     options.Queue = rabbitQueue;
//     options.RoutingKey = rabbitRoutingKey;
// });
//
// // Register RabbitMQ services
// builder.Services.AddHostedService<MessageService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

// auto migrate db
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetRequiredService<RationDB>().MigrateDb();
}

// Use CORS middleware
app.UseCors();

app.MapControllers();
app.UseHealthChecks("/hc");
app.Run();