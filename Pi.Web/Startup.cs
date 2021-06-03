using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pi.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using UnoPi = Unosquare.RaspberryIO.Pi;

namespace Pi.Web
{
    public class Startup
    {
        CancellationTokenSource _ButtonListenerCancellationTokenSource = new CancellationTokenSource();
        IO.IPWMServoController _PWMServoController;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            UnoPi.Init<BootstrapWiringPi>();
            _PWMServoController = new ServoController(BcmPin.Gpio19);

            var UP_PIN = BcmPin.Gpio23;
            var DOWN_PIN = BcmPin.Gpio24;
            _PWMServoController.ListenForButtons(UP_PIN, DOWN_PIN);

            services.AddSingleton<IO.IPWMServoController>((s) =>_PWMServoController);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Connected to RasPi Servo Server.");
                });
            });
        }

        private void OnShutdown()
        {
            _PWMServoController.TurnOff();
        }
    }
}
