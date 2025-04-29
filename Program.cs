using DotNetEnv;
using TodoApi.Models;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
Env.Load();

// Read from .env
string mongoUri = Environment.GetEnvironmentVariable("MONGO_URI");

// Inject it into configuration system manually
builder.Configuration["TodoDatabase:ConnectionString"] = mongoUri;

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("TodoDatabase"));

builder.Services.AddSingleton(sp =>
{
    var settings = builder.Configuration.GetSection("TodoDatabase").Get<DatabaseSettings>();
    return new TodoService(settings);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Todo API",
        Version = "v1",
        Description = "A simple ASP.NET Core Web API for managing Todos",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Biswarup Naha",
            Email = "biswarupnaha@coding-junction.com",
            Url = new Uri("https://www.github.com/biswarup-naha")
        }
    });
});


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
