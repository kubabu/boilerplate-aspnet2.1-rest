using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using OnixWebApi.Models.DbContexts;
using OnixWebApi.Services;

namespace OnixWebApi
{
    public class Startup
    {
        private readonly string _connectionString;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _connectionString = Configuration.GetConnectionString("OrderShippingContext");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            //services.AddEntityFrameworkNpgsql()
            //    .AddDbContext<OrderShippingContext>(
            //    options => options.UseNpgsql(_connectionString)
            //    );

            services.AddDbContext<OrderShippingContext>(
                options => options.UseNpgsql(_connectionString)
            );

            services.AddTransient<HeroService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseCors(
            //    options => options.WithOrigins("http://localhost").AllowAnyMethod()
            //);
            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
