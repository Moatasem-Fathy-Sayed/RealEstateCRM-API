using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstateCRM.API.Helpers;
using RealEstateCRM.API.Middlewares;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Interfaces;
using RealEstateCRM.Core.Validators;
using RealEstateCRM.Infrastructure.Data;
using RealEstateCRM.Infrastructure.Repositories;
using RealEstateCRM.Infrastructure.Services;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers
builder.Services.AddControllers();

// 2. Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 4. Add Dependency Injection Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 5. Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

// 6. Configure Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Real Estate CRM API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePropertyDtoValidator>();
builder.Services.AddMemoryCache();
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddHostedService<RealEstateCRM.Infrastructure.Services.AppointmentReminderService>();
// Health Checks for Database and API Status
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
// Configure Rate Limiting to prevent API abuse
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("FixedPolicy", policy =>
    {
        policy.PermitLimit = 60; // 60 requests
        policy.Window = TimeSpan.FromMinutes(1); // per 1 minute
        policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        policy.QueueLimit = 2;
    });
});

var app = builder.Build();

// Configure HTTP Request Pipeline
app.UseMiddleware<ExceptionMiddleware>();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealEstateCRM API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.UseMiddleware<RealEstateCRM.API.Middlewares.ExceptionMiddleware>();
app.MapHealthChecks("/health");
app.Run();