using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SmartThingsHostPinger.Options;

namespace SmartThingsHostPinger
{
	internal class Program
	{
		/// <summary>
		/// Defines the entry point of the application.
		/// </summary>
		private static void Main()
		{
			// Setup the logging.
			LoggingLevelSwitch loggingSwitch = new LoggingLevelSwitch(LogEventLevel.Information);
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(loggingSwitch)
				.WriteTo.Console(
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Message}{NewLine}{Exception}",
					restrictedToMinimumLevel: LogEventLevel.Information)
				.WriteTo.Trace(restrictedToMinimumLevel: LogEventLevel.Debug)
				.WriteTo.RollingFile(
					@"logs\log-{Date}.txt",
					retainedFileCountLimit: 14,
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
				.CreateLogger();

			Console.Title = "SmartThings Host Pinger";
			Log.Information("---SmartThings Host Pinger V3.0---");

			// Attempt to load the configuration from config.json.
			// If that fails, fall back to config.config.
			if (!Config.TryLoadJsonConfig("config.json", out IAppOptions options))
			{
				if (!Config.TryLoadXmlConfig("config.config", out options))
				{
					Log.Error("Configuration file could not be loaded");
					return;
				}
			}

			if (options.Debug)
			{
				loggingSwitch.MinimumLevel = LogEventLevel.Debug;
				Log.Logger.Information("Debug logging is enabled");
			}
			Log.Logger.Information("Press enter to exit");
			Log.Logger.Information("Started");

			CancellationTokenSource cancelToken = new CancellationTokenSource();
			Task runner = Task.Run(async () =>
			{
				HostPinger pinger = new HostPinger(options);
				await pinger.Start(cancelToken.Token);
			}, cancelToken.Token);

			Console.ReadLine();

			// Cancel the task and wait for the task to complete.
			cancelToken.Cancel();
			Log.Logger.Information("Waiting for jobs to complete");
			while (!runner.IsCompleted) { }
			Log.Logger.Information("Finished and shutting down");
		}
	}
}