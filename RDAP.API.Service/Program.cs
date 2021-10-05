using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace RDAP.API.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ConfigurationManager.AppSetting;
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(configuration)
                 .Enrich.FromLogContext()
                 .WriteTo.File("C:\\Logs\\log.txt")
                 .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
                logging.AddSerilog();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
