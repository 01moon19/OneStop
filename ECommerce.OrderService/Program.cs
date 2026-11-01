using ECommerce.OrderService.Clients;
using ECommerce.OrderService.Repositories;
using ECommerce.OrderService.Repositories.Interfaces;
using ECommerce.OrderService.Services;
using ECommerce.OrderService.Services.Interfaces;
using ECommerce.OrderService.Data;

using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ECommerce.OrderService.Clients.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("OrderConnection")));

builder.Services.AddOpenApi();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();


var productServiceUrl =
    builder.Configuration["Services:ProductService"]
    ?? throw new InvalidOperationException(
        "Product Service URL is missing.");

builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(
    client =>
    {
        client.BaseAddress = new Uri(productServiceUrl);
        client.Timeout = TimeSpan.FromSeconds(10);
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT issuer missing."),

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT audience missing."),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:Key"])),

            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,

            ClockSkew = TimeSpan.FromSeconds(30),

            NameClaimType = "sub",
            RoleClaimType = ClaimTypes.Role



        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
