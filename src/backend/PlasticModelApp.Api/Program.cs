using MediatR;
using PlasticModelApp.Application.Masters.Queries;
using PlasticModelApp.Infrastructure;
using PlasticModelApp.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetMastersQuery).Assembly));
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
