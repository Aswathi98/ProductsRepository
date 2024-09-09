
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Ocelot services
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Use CORS before Ocelot
app.UseCors("AllowAllOrigins");

// Add Ocelot middleware
await app.UseOcelot();

// Define other middleware as needed
app.MapGet("/", () => "Hello World!");

app.Run();
