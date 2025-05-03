using Bank_System_PaySky.Auth;
using Bank_System_PaySky.Data;
using Bank_System_PaySky.Middleware;
using Bank_System_PaySky.Services.AccountCreation;
using Bank_System_PaySky.Services.AccountHelper;
using Bank_System_PaySky.Services.AccountTransactionService;
using Bank_System_PaySky.Services.Transactions;
using Bank_System_PaySky.Services.UserCreationService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();

var jwt = builder.Configuration.GetSection("jwt").Get<Jwt>();
builder.Services.AddSingleton(jwt);
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, Options =>
    {
        Options.SaveToken = true;
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.SigningKey)),
            ValidateLifetime = true
        };
    });

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
            "Auth" => "0",
            "Users" => "1",
            "Accounts" => "2",
            "AccountTransactions" => "3",
            "Transactions" => "4",
            _ => "5"
        };
    });

    // Add JWT Bearer Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT Token from Login"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
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
