using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GetOnBoard
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
						.UseStartup<Startup>()
						.ConfigureLogging(LogConfig)
						.ConfigureKestrel((context, options) =>
						{
							// Set properties and call methods on options
						});
		}

		private static void LogConfig(ILoggingBuilder log)
		{
			log.AddConsole();
			log.AddFilter("GetOnBoard", LogLevel.Information);
			log.AddFilter("Microsoft.AspNetCore.Mvc", LogLevel.Error);
			log.AddFilter("Microsoft.AspNetCore.Hosting", LogLevel.Error);
			log.AddFilter("Microsoft.AspNetCore.Routing", LogLevel.Error);
		}
	}
}
