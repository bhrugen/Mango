using Mango.MessageBus;
using Mango.OrderAPI.Data;
using Mango.OrderAPI.Models;
using Mango.OrderAPI.Models.Dto;
using Mango.OrderAPI.Service;
using Mango.OrderAPI.Service.IService;
using Mango.Serives.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Reflection.PortableExecutable;
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
    o.CreateMap<OrderHeaderDto, OrderHeader>().ReverseMap();
    o.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();

    o.CreateMap<OrderHeaderDto,CartHeaderDto>().ForMember(dest=>dest.CartTotal, opt=>opt.MapFrom(src=>src.OrderTotal)).ReverseMap();
    o.CreateMap<CartDetailsDto,OrderDetailsDto>()
    .ForMember(dest=>dest.ProductName,u=>u.MapFrom(src=>src.Product.Name))
    .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));
    o.CreateMap<OrderDetailsDto, CartDetailsDto>();
});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();


builder.Services.AddHttpClient("Product",
    u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));


builder.AddJwtAuthentication();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}