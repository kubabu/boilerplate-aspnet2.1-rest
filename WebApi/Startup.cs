using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using WebApi.Hubs;
using WebApi.Models.Configuration;
using WebApi.Models.DbContexts;
using WebApi.Services;
using WebApi.Services.Interfaces;

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
            services.AddCors(options => options.AddPolicy(_settings.CorsPolicyName, builder =>
                {
                    builder.WithOrigins(_settings.CorsClientUrls.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                }));
            services.AddSignalR();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _settings.JwtSettings.Issuer,
                        ValidAudience = _settings.JwtSettings.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(_settings.JwtSettings.JwtKeyBytes)
                    };
                });
            services.AddMvc();

            services.AddDbContext<MainDbContext>(
                options => options.UseNpgsql(
                    Configuration.GetConnectionString(typeof(MainDbContext).FullName.Split(".").Last()))
            );

            services.AddTransient<WebApiSettings>(_ => _settings);

            services.AddTransient<IAuthorizeService, AuthService>();
            services.AddTransient<IGenerateSecurityTokens, GenerateSecurityTokens>();
            services.AddTransient<ICheckPasswordService, CheckPasswordService>();
            services.AddTransient<IServeUsers, UserService>();
            services.AddTransient<ICompleteOrdersService, CompleteOrdersService>();

            services.AddOptions();

            services.Configure<CertificateSettings>(Configuration.GetSection(nameof(CertificateSettings)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(_settings.CorsPolicyName);
            app.UseSignalR((options) => {
                options.MapHub<CompletationOrdersHub>(_settings.CompletationOrdersHubUrl);
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions // this needs to preceed UseAuthentication
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
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
