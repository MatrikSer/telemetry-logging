using Serilog;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File(".logs/start-host-log-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

try
{
	Log.Information("App running...");

	var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    	loggerConfiguration.ReadFrom.Configuration(context.Configuration));
    
    builder.Services.AddRazorPages();

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Error");
		app.UseHsts();
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

	app.UseRouting();

	app.UseAuthorization();

	app.MapRazorPages();

	app.UseSerilogRequestLogging();

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex.Message, "Fatal error");
}
finally
{
	Log.CloseAndFlush();
}