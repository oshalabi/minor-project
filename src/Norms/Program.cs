using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Norms.DAL;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configure DbContext
var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "Developer";
var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "password";
var databaseName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "NormDB";
var dbConnectionString =
    $"Host={postgresHost};Port={postgresPort};Database={databaseName};Username={postgresUser};Password={postgresPassword}";
builder.Services.AddDbContext<NormDB>(options =>
{
    options.UseNpgsql(dbConnectionString);
});

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: dbConnectionString,
        name: "postgresql",
        failureStatus: HealthStatus.Unhealthy,
        timeout: TimeSpan.FromSeconds(5))
    ;

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Create the database if it doesn't exist
CreateDbIfNotExists(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Use CORS middleware
app.UseCors("AllowAll");
app.UseHealthChecks("/hc");
app.MapControllers();

app.Run();

void CreateDbIfNotExists(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<NormDB>();
            context.Database.Migrate(); // Applies migrations or creates the database
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the database.");
        }
    }
}