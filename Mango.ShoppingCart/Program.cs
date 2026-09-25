using Mango.Serives.Shared.Extensions;
using Mango.ShoppingCartAPI.Data;
using Mango.ShoppingCartAPI.Models;
using Mango.ShoppingCartAPI.Models.Dto;
using Mango.ShoppingCartAPI.Service;
using Mango.ShoppingCartAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
    o.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddHttpClient("Product", 
    u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
builder.Services.AddHttpClient("Coupon", 
    u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"]));

builder.AddJwtAuthentication();
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
