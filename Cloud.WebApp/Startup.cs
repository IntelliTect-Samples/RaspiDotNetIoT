using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRChat.Hubs;
using System;
using System.IO;
using VueCliMiddleware;

namespace Cloud.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = Path.Combine("ClientApp", "dist");
#if DEBUG
                configuration.RootPath = "ClientApp";
#endif
            });

#if DEBUG
            //so that we can use IIS and refer to the Website via the Host name of the computer versus the IP which might change.
            services.AddCors();
#endif

            #region SignalR

            services.AddSignalR().AddHubOptions<CloudHub>(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(14);
                options.EnableDetailedErrors = true;
            })
           .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                //so that we can use IIS and refer to the Website via the Host name of the computer versus the IP which might change.
                app.UseCors(options => options
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
            }

            app.UseRouting();
            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CloudHub>($"/{nameof(CloudHub)}");
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "ClientApp/";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }

            });
        }
    }
}
