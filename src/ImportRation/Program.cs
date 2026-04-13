using DotNetEnv;
using ImportRation.Data;
using ImportRation.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

Env.Load();

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
var databaseName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "BasalRationDB";

var dbConnectionString =
    $"Host={postgresHost};Port={postgresPort};Database={databaseName};Username={postgresUser};Password={postgresPassword}";

builder.Services.AddDbContext<BasalRationDBContext>(options => { options.UseNpgsql(dbConnectionString); });

builder.Services.Configure<ImportRationMessageBroker>(builder.Configuration.GetSection("RabbitMQ"));


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
builder.Services.Configure<ImportRationMessageBroker>(options =>
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
// builder.Services.AddScoped<IRabbitMQPublisher, IMessageService>();
builder.Services.AddSingleton<IMessageService, MessageService>();

// Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-migrate the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BasalRationDBContext>();
        context.MigrateDB(); // Applies migrations or creates the database
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImportRation API - v1"); });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseHealthChecks("/hc");
app.MapControllers();

app.Run();