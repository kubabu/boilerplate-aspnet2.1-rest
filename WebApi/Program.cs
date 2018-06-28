using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using WebApi.Models.Configuration;

namespace WebApi
{
    public class Program
    {
        private static readonly string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static readonly string currentDir = Directory.GetCurrentDirectory();


        public static void Main(string[] args)
        {
            Console.Title = "ONIX Web API";
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .AddJsonFile($"hosting.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var uris = config.GetSection("server.urls").Value.Split(";").Select(u => new Uri(u)).ToList();

            var host = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseContentRoot(currentDir)
                .UseIISIntegration()                // get URLs from launch settings
                .UseKestrel( options => {
                    foreach(var uri in uris)
                    {
                        if (uri.ToString().Contains("https://"))    // create HTTPS endpoint explicitly for srv urls from hosting.json
                        { 
                            options.Listen(System.Net.IPAddress.Any, uri.Port, listenOptions =>
                            {
                                listenOptions.UseHttps(LoadCertificate(config));
                            });
                        }
                    }
                })
                .UseStartup<Startup>()
                .Build();

            return host;
        }

        private static X509Certificate2 LoadCertificate(IConfiguration config)
        {
            var settings = config.GetSection(nameof(CertificateSettings)).Get<CertificateSettings>();
            var hash = GitHash();
            return new X509Certificate2(settings.Filename, settings.Password);
        }

        public static string GitHash()
        {
            var asm = typeof(Program).Assembly;
            var attrs = asm.GetCustomAttributes < AssemblyMetadataAttribute > ();
            return attrs.FirstOrDefault(a => a.Key == "GitHash")?.Value;
        }
    }
}
