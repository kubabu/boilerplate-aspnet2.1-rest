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
using WebApi.Models.DbContexts;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        private readonly string _connectionString;
        private readonly string[] _corsClientUrls;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            _connectionString = Configuration.GetConnectionString("OrderShippingContext");
            _corsClientUrls = Configuration.GetValue<string>("CorsClientUrl").Split(" ");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(config =>
            {
                config.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddDbContext<OrderShippingContext>(
                options => options.UseNpgsql(_connectionString)
            );

            services.AddTransient<HeroService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder =>
                builder.WithOrigins(_corsClientUrls)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            //app.UseForwardedHeaders(new ForwardedHeadersOptions // this needs to preceed UseAuthentication
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});
            //app.UseAuthentication();  // unused for now

            var options = new RewriteOptions()
                .AddRedirectToHttps();      //  redirects all HTTP requests to HTTPS
            app.UseRewriter(options);       // if ignoring them is needed, add RequireHttpsAttribute to ConfSvc()
        }
    }
}
