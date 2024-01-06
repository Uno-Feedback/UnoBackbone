using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics;
using Uno.Api;
using Uno.Application;
using Uno.Application.Settings;
using Uno.Infrastructer;
using Uno.Infrastructer.ExternalServices;
using Uno.Shared.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .RegisterApplicationServices()
    .RegisterInfrastructerExternalServices()
    .RegisterInfrastructerServices(builder.Configuration)
    .RegisterPresentationServices(builder.Configuration);

builder.Services.Configure<HostSettings>(builder.Configuration.GetSection(nameof(HostSettings)));

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion('1');
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddMvc();


var app = builder.Build();


app.UseExceptionHandler(error =>
{
    error.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        await context.Response.WriteAsync(new Response(exception.Message).ToString());
    });
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
