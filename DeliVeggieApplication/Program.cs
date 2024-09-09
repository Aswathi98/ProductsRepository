
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using DeliVeggieApplication;
using DeliVeggieApplication.Interfaces;
using DeliVeggieApplication.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
   builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//var logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.Console()
//    .WriteTo.File("C://Users//v-asadasivan//Downloads/logfile.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

//builder.Logging.ClearProviders();  
//builder.Logging.AddSerilog(logger);
builder.Logging.ClearProviders(); // Remove default providers
builder.Logging.AddConsole();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

// Register RabbitMQ connection and channel
builder.Services.AddSingleton<IConnection>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
    var factory = new ConnectionFactory() { HostName = settings.HostName };
    return factory.CreateConnection();
});

builder.Services.AddScoped<RabbitMqConsumer>();
// Register RabbitMQ producer
builder.Services.AddSingleton<RabbitMqProducer>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();
app.UseCors("AllowAllOrigins");
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

