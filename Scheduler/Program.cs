using System.Globalization;
using Scheduler;
using Scheduler.Infra;

var builder = Host.CreateApplicationBuilder(args);

// define en-US como padrao
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddHostedService<Worker>();

NativeInjector.RegisterServices(builder.Services, builder.Configuration);

var host = builder.Build();
host.Run();