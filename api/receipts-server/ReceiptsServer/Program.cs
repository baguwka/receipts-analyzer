using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
#if !DEBUG
using Microsoft.Extensions.Configuration;
#endif

namespace ReceiptsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(
                args.Where(arg => arg != "--console").ToArray());

            var host = builder.Build();

            if (isService)
            {
                // To run the app without the CustomWebHostService change the
                // next line to host.RunAsService();
                host.RunAsCustomService();
            }
            else
            {
                host.Run();
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // ReSharper disable once RedundantAssignment
            var hostUrl = string.Empty;
#if !DEBUG
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            hostUrl = configuration["hosturl"];
            if (string.IsNullOrEmpty(hostUrl))
                hostUrl = "http://0.0.0.0:8200";
#else
            hostUrl = "http://0.0.0.0:5200";
#endif

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls(hostUrl)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    //logging.AddEventLog();

                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    //configure app here
                })
                .UseStartup<Startup>();
        }
    }
}
