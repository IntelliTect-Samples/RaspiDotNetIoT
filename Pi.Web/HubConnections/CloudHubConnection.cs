using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Pi.Web.HubConnections
{

    public static class CloudHubConnection
    {
        static HubConnection _CloudHubConnection;
        private static IO.IPWMServoController _IOServoController;


        public static void Initalize(string hubURl, IO.IPWMServoController servoController)
        {
            _IOServoController = servoController;
            _IOServoController.PropertyChanged += _IOServoController_PropertyChanged;

            _CloudHubConnection = new HubConnectionBuilder()
                   .WithAutomaticReconnect()
                   .WithUrl(hubURl)
                   .Build();

            AddMethods();

            _CloudHubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _CloudHubConnection.StartAsync();
            };

            _CloudHubConnection.StartAsync().Wait();

            _CloudHubConnection.InvokeAsync("JoinPiClientGroup");
        }

        private static void AddMethods()
        {
            _CloudHubConnection.On("Increase_Angle", IncreaseAngle);
            _CloudHubConnection.On<int>("Set_Degree", SetDegree);
        }

        public static void IncreaseAngle()
        {
            _IOServoController.IncreasePulse();
        }

        public static void DecreaseAngle()
        {
            _IOServoController.DecreasePulse();
        }


        public static void SetDegree(int angle)
        {
            _IOServoController.WriteAngle(angle);
        }

        private static void _IOServoController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {

                case nameof(_IOServoController.CurrentAngle):
                    DegreeChange();
                    break;
            }
        }
        public static void DegreeChange()
        {
           // _CloudHubConnection.StartAsync();
            _CloudHubConnection.InvokeAsync("DegreeStatus", _IOServoController.ReadAngle());
       /*     HttpWebRequest myReq =
(HttpWebRequest)WebRequest.Create("https://10.10.8.127:45455");
            myReq.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
            var res = myReq.GetResponse();*/
        }

       
    }
}
