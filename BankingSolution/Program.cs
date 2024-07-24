using BankingSolution.Repositories;
using BankingSolution.Data;
using BankingSolution.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure MongoDB settings
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings");
builder.Services.AddSingleton(new MongoDbContext(
    mongoDbSettings["ConnectionString"] ?? throw new InvalidOperationException(),
    mongoDbSettings["DatabaseName"] ?? throw new InvalidOperationException()
));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Banking API",
        Description = "A simple example ASP.NET Core Web API for Banking Solution",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Jurģis Viļums",
            Email = "vilumsjurgis.lv@gmail.com",
            Url = new Uri("https://github.com/jurgis-vilums/BankingSolution_Homework"),
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
