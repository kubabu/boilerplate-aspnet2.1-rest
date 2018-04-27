using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApi
{
    public class Program
    {
        private static string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static string currentDir = Directory.GetCurrentDirectory();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = LoadConfig();
            var port = ConfigUri(config).Port;
            var cert = LoadCertificate(config);

            var host = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseContentRoot(currentDir)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(System.Net.IPAddress.Any, port, listenOptions =>
                        listenOptions.UseHttps(cert));
                }) // .UseUrls("http://*:5000")
                .Build();

            return host;
        }

        private static IConfigurationRoot LoadConfig()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("hosting.json", optional: true)
              .AddJsonFile($"hosting.{environment}.json", optional: true)
              .AddEnvironmentVariables()
              .Build();

            return config;
        }

        private static Uri ConfigUri(IConfigurationRoot config)
        {
            var urlSettings = config.GetSection("urls");
            var urls = urlSettings.Value.Split(';');
            var url = urls.First();

            return new Uri(url);
        }

        private static X509Certificate2 LoadCertificate(IConfiguration config)
        {
            var certificateSettings = config.GetSection("certificateSettings");
            var certificateFileName = certificateSettings.GetValue<string>("filename");
            var certificatePassword = certificateSettings.GetValue<string>("password");

            return new X509Certificate2(certificateFileName, certificatePassword);
        }
    }
}
