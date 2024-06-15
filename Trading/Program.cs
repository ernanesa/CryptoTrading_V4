using System.Globalization;
using Trading.Infra;

var builder = WebApplication.CreateBuilder(args);

// define en-US como padrao
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

NativeInjector.RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();