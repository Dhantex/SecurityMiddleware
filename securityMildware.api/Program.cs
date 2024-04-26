using Microsoft.Extensions.Configuration;
using SecurityMiddleware.api.Interface;
using SecurityMiddleware.api;
using SecurityMiddleware.api.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var settings = builder.Configuration.GetSection("ConfigurationSections").Get<ConfigurationSections>();

builder.Services.AddSingleton<ISecureDomainPolicy>(builder.Configuration.GetSection("SecureDomainPolicy").Get<SecureDomainPolicy>());

builder.Services.AddCors(options => options.AddPolicy("AllowPolicySecureDomains", x =>
{
    x.AllowAnyOrigin()
     .WithOrigins(settings.SecureDomains)
     .AllowAnyHeader()
     .AllowCredentials()
     .AllowAnyMethod();
}));

builder.Services.AddHsts(options =>
{
    options.IncludeSubDomains = true;//true,if we need to include subdomains
    options.MaxAge = TimeSpan.FromDays(365);//specify days
});

var app = builder.Build();


app.UseMiddleware<SecurityHeadersMiddleware>();


// Configure the HTTP request pipeline.

//Forcing HTTPS Redirection
app.UseHttpsRedirection();

app.UseAuthorization();


//Forcing HTTPS usage is crucial for security
app.UseHsts();

app.UseCors("AllowPolicySecureDomains");

app.MapControllers();

app.Run();
