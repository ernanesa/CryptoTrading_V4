using System.Globalization;
using Trading.Infra;
using Trading.Services;

var builder = WebApplication.CreateBuilder(args);

// define en-US as default
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

NativeInjector.RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Trading API is running");

app.MapGet("users", async (UserService userService) =>
{
    return await userService.GetAllUsersAsync();
});

app.MapGet("activeUsers", async (UserService userService) =>
{
    return await userService.GetAllActiveUsersAsync();
});

app.MapGet("/authenticate", async (AuthenticationService authService, UserService userService) =>
{
    try
    {
        var users = await userService.GetAllActiveUsersAsync();
        var user = users.FirstOrDefault();
        var authResponse = await authService.AuthenticateAsync(user);
        return Results.Ok(authResponse);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("GetAccountBalances", async (AccountService accountService, AuthenticationService authService, UserService userService) =>
{
    var users = await userService.GetAllActiveUsersAsync();
    var user = users.FirstOrDefault();
    await authService.AuthenticateAsync(user);

    return await accountService.GetAccountBalancesAsync(user);
});

app.MapGet("GetTotalBalance", async (AccountService accountService, AuthenticationService authService, UserService userService) =>
{
    var users = await userService.GetAllActiveUsersAsync();
    var user = users.FirstOrDefault();
    await authService.AuthenticateAsync(user);

    return await accountService.GetTotalBalanceAsync(user);
});

app.MapGet("GetRealAvailable", async (AccountService accountService, AuthenticationService authService, UserService userService) =>
{
    var users = await userService.GetAllActiveUsersAsync();
    var user = users.FirstOrDefault();
    await authService.AuthenticateAsync(user);

    return await accountService.GetAccountBalanceAvailableInRealAsync(user);
});



// app.MapGet("/Buy", () =>
// {
//     using var scope = app.Services.CreateScope();
//     var tickerService = scope.ServiceProvider.GetRequiredService<TickerService>();

//     tickerService.Buy().Wait();
//     return Results.Ok();
// });

// app.MapGet("/Sell", () =>
// {
//     using var scope = app.Services.CreateScope();
//     var tickerService = scope.ServiceProvider.GetRequiredService<TickerService>();

//     tickerService.Sell().Wait();
//     return Results.Ok();
// });





// # region SystemConfiguration
// app.MapGet("/systemConfiguration", async (SystemConfigurationService service) =>
// {
//     return await service.GetAllAsync();
// });

// app.MapGet("/systemConfiguration/{id}", async (int id, SystemConfigurationService service) =>
// {
//     var config = await service.GetByIdAsync(id);
//     return config is not null ? Results.Ok(config) : Results.NotFound();
// });

// app.MapPost("/systemConfigurations", async (SystemConfiguration config, SystemConfigurationService service) =>
// {
//     await service.AddAsync(config);
//     return Results.Created($"/systemConfigurations/{config.Id}", config);
// });

// app.MapPut("/systemConfigurations/{id}", async (int id, SystemConfiguration config, SystemConfigurationService service) =>
// {
//     if (id != config.Id)
//     {
//         return Results.BadRequest();
//     }
//     await service.UpdateAsync(config);
//     return Results.NoContent();
// });

// app.MapDelete("/systemConfigurations/{id}", async (int id, SystemConfigurationService service) =>
// {
//     await service.DeleteAsync(id);
//     return Results.NoContent();
// });
// #endregion




app.Run();
