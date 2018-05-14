using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using WebApi.Models.Configuration;
using WebApi.Models.DbContexts;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private WebApiSettings _settings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _settings = Configuration.GetSection(nameof(WebApiSettings)).Get<WebApiSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            services.AddDbContext<OrderShippingContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("OrderShippingContext"))
            );

            services.AddTransient<HeroService>();

            services.AddOptions();

            services.Configure<CertificateSettings>(Configuration.GetSection(nameof(CertificateSettings)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder =>
                builder.WithOrigins(_settings.CorsClientUrls.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var options = new RewriteOptions()
                    .AddRedirectToHttps();      // redirects all HTTP requests to HTTPS in dev mode
                app.UseRewriter(options);
            }
        }
    }
}
