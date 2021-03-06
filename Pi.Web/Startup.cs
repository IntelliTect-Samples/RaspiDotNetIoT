using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pi.IO;
using Pi.Web.HubConnections;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using UnoPi = Unosquare.RaspberryIO.Pi;

namespace Pi.Web
{
    /// <summary>
    /// Instead of deploying the web server to a remote location, you can instead have the Raspberry Pi host the web server for interacting with itself.
    /// All in one solution for a local network access to you custom IoT devices.
    /// </summary>
    public class Startup
    {
        CancellationTokenSource _ButtonListenerCancellationTokenSource = new CancellationTokenSource();
        IO.IPWMServoController _PWMServoController;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

#if LOCALDEBUG
            _PWMServoController = new MockServoController(BcmPin.Gpio19);
#else

            UnoPi.Init<BootstrapWiringPi>(); 
            _PWMServoController = new ServoController(BcmPin.Gpio19);
#endif

            var UP_PIN = BcmPin.Gpio23;
            var DOWN_PIN = BcmPin.Gpio24;
            _PWMServoController.ListenForButtons(UP_PIN, DOWN_PIN); //listen for the physical buttons

            services.AddSingleton<IO.IPWMServoController>((s) => _PWMServoController);
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

            // string hubUrl = Configuration.GetValue<string>("CloudHubUrlProd");
            string hubUrl = Configuration.GetValue<string>("CloudHubUrlDev");

            CloudHubConnection.Initialize(hubUrl, _PWMServoController);
        }

        private void OnShutdown()
        {
            _PWMServoController.TurnOff();
        }
    }
}
