using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Pi.Web.HubConnections
{

    public static class CloudToPiHubConnection
    {
        static HubConnection _CloudHubConnection;
        private static IO.IPWMServoController _IOServoController;


        public static void Initalize(string hubURl, IO.IPWMServoController servoController)
        {
            _IOServoController = servoController;

            _CloudHubConnection = new HubConnectionBuilder()
                   .WithAutomaticReconnect()
                   .WithUrl(hubURl)
                   .AddJsonProtocol(options =>
                   {
                       options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                   })
                   .Build();

            AddMethods();

            _CloudHubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _CloudHubConnection.StartAsync();
            };

            _CloudHubConnection.StartAsync();

        }

        private static void AddMethods()
        {
            _CloudHubConnection.On("Up", Up);
            _CloudHubConnection.On<int>("Set_Degree", SetDegree);

        }

        public static void Up()
        {

            _IOServoController.IncreasePulse();

            _CloudHubConnection.InvokeAsync("Degree_Status", _IOServoController.ReadAngle());

        }


        public static void SetDegree(int angle)
        {

            _IOServoController.WriteAngle(angle);

            _CloudHubConnection.InvokeAsync("Degree_Status", _IOServoController.ReadAngle());

        }

    }
}
