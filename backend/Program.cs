using System.Text;
using HelpDesk.Database;
using HelpDesk.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Db connection.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string" + "'DefaultConnection' not found");

// Cors.

builder.Services.AddCors(options =>
{
    options.AddPolicy("Localhost", 
                        policy =>
                        {
                            policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        }
                    ); 
});

// Jwt.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
       options.TokenValidationParameters = new TokenValidationParameters
       {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)
            )
       };
       options.Events = new JwtBearerEvents
       {
           OnMessageReceived = request =>
           {
               request.Token = request.Request.Cookies["accessToken"];

               return Task.CompletedTask;
           }
       };
        
    });

// Controllers.

builder.Services.AddControllers();

// DI

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<RefreshTokenService>();

var app = builder.Build();

// Seed database

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    DataSeeder.Seed(db);
}

// Cors

app.UseCors("Localhost");

// Auth

app.UseAuthentication();

app.UseAuthorization();

// Controllers

app.MapControllers();

app.Run();
