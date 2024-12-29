using Bank_System_PaySky.Data;
using Bank_System_PaySky.Middleware;
using Bank_System_PaySky.Services.AccountCreation;
using Bank_System_PaySky.Services.AccountHelper;
using Bank_System_PaySky.Services.AccountTransactionService;
using Bank_System_PaySky.Services.Transactions;
using Bank_System_PaySky.Services.UserCreationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure logging to output to both the console and debug
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Configure DbContext with the connection string
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register middleware for exception handling
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// Register services for dependency injection
builder.Services.AddScoped<IAccountHelperService, AccountHelperService>();
builder.Services.AddScoped<IUserCreationService, UserCreationService>();
builder.Services.AddScoped<ITransactionsService, TransactionsService>();
builder.Services.AddScoped<IAccountTransactionService, AccountTransactionService>();
builder.Services.AddScoped<IAccountCreationService, AccountCreationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API documentation
var info = new OpenApiInfo()
{
    Title = "Banking System API", // API title
    Version = "v1", // API version
    Description = "This API serves as the backbone of the Bank System platform, enabling seamless account and transaction management. It provides a range of endpoints for creating and managing user accounts, and tracking financial activities. Built with scalability and security in mind, the API supports robust error handling, logging, and integration with modern banking workflows.\r\n\r\nDesigned for developers, it includes detailed request and response structures, offering flexibility for integration into diverse applications. Whether for personal finance tools, corporate banking solutions, or payment platforms, this API facilitates efficient and reliable financial operations.",
    Contact = new OpenApiContact()
    {
        Name = "Ahmed Bassem Ramzy",
        Email = "rameya683@gmail.com",
    }
};

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", info);

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = "Bank-System-PaySky.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Use tags to group endpoints

    c.DocInclusionPredicate((name, api) => true);
    c.OrderActionsBy((apiDesc) =>
    {
        var groupName = apiDesc.GroupName ?? "Default";
        return groupName switch
        {
            "Users" => "1",
            "Accounts" => "2",
            "AccountTransactions" => "3",
            "Transactions" => "4",
            _ => "5"
        };
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(x =>
    {
        x.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank System PaySky API v1");
    });
}

app.UseHttpsRedirection();

// Use the custom exception middleware before other middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
