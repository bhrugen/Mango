using Mango.GatewaySolution.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();
builder.AddJwtAuthentication();
var ocelotFile = builder.Environment.IsDevelopment() ? "ocelot.json" : "ocelot.production.json";
builder.Configuration.AddJsonFile(ocelotFile, optional: false, reloadOnChange: true);

var app = builder.Build();
app.MapWhen(ctx => ctx.Request.Path == "/", root =>
    root.Run(ctx => ctx.Response.WriteAsync("Hello World!")));
app.UseOcelot();
app.Run();
