using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PasswordManager.API.Middleware;
using PasswordManager.Application;
using PasswordManager.Infrastructure;
using PasswordManager.Infrastructure.Database.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure();

var connectionString = builder.Configuration.GetConnectionString("Default") ??
                       throw new NullReferenceException("No connection string");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:5057") // Blazor app URL
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseCors("AllowBlazorClient")
    .UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseExceptionHandler(appError =>
    {
        appError.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                var error = new { message = contextFeature.Error.Message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(error));

                // Logging
                Console.WriteLine($"Error: {contextFeature.Error}");
                Console.WriteLine($"Stack Trace: {contextFeature.Error.StackTrace}");
            }
        });
    });

app.Run();