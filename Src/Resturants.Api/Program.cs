using Resturants.Infrastructure.Extensions;
using Resturants.Infrastructure.Seeders;
using Resturants.Application.Extensions;
using Serilog;
using Serilog.Events;
using Resturants.Domain.Entities;
using Resturants.Api.Extensions;
using Resturants.Api.Constants;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentaion(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(_ => { });


var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
await seeder.Seed();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (httpContext, _, _) => 
    {
        int statusCode = httpContext.Response.StatusCode;
        if (statusCode >= 100 && statusCode <= 399)
        {
            return LogEventLevel.Debug;
        }
        if (statusCode >= 400 && statusCode <= 499)
        {
            return LogEventLevel.Warning;
        }
        return LogEventLevel.Error; 
    };
});

app.UseRouting();

app.UseCors(CorsPoliciesConstants.AllowAllMethodsAndOriginsAndHeaders);

app.MapPost("api/identity/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync().ConfigureAwait(false);
})
.WithTags("Identity")
.RequireAuthorization();


app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();


app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
