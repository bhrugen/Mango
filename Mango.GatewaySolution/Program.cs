using Mango.GatewaySolution.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddOcelot();
builder.AddJwtAuthentication();


app.MapGet("/", () => "Hello World!");
app.UseOcelot();
app.Run();
