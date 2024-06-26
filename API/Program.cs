using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using API.Error;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Add the extension class for service in Extension folder
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

//Error Handling Before HTTP request always

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var storeContext = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await storeContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsnyc(storeContext);
}
catch (Exception ex)
{
    logger.LogError(ex, "An Error occured during migrations");
    //logger.LogError(throw);
}


app.Run();
