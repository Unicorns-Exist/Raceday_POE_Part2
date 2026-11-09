using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Raceday_POE2.Data;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// 1. REGISTER SERVICES
// ----------------------------------------------------

// Allows the API to use controller classes to handle
// requests such as GET /api/events and POST /api/events.
builder.Services.AddControllers();

// Registers the services needed for role and permission
// checks. We will configure authentication later.
builder.Services.AddAuthorization();

// Adds Swagger support so we can inspect and test API
// endpoints in a browser.
builder.Services.AddEndpointsApiExplorer();

// Adds Swagger support so we can inspect and test API
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token. Example: Bearer eyJhbGciOi..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});// end of Swagger configuration from Meta

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is missing from configuration.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.FromSeconds(30)
            };
    });
// ----------------------------------------------------
// 2. REGISTER THE DATABASE CONNECTION
// ----------------------------------------------------

// Register RaceDayContext so controllers and other
// classes can request it when they need database access.
//
// GetConnectionString("DefaultConnection") reads the
// matching setting from appsettings.json.
//
// UseSqlServer tells Entity Framework Core to use SQL
// Server as the database provider.
builder.Services.AddDbContext<RaceDayContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    )
);

// ----------------------------------------------------
// 3. BUILD THE APPLICATION
// ----------------------------------------------------

var app = builder.Build();

// Swagger is enabled while we develop the application.
// This allows us to test endpoints from the browser.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Authentication middleware will be added when we
// implement login and authenticated user sessions.

// Checks authorization rules when we add protected
// endpoints and role restrictions.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Makes our controller routes available to requests.
app.MapControllers();

// Starts the API web server.
app.Run();