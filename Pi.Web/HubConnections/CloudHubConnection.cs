using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Pi.Web.HubConnections
{

    public static class CloudHubConnection
    {
        static HubConnection _CloudHubConnection;
        private static IO.IPWMServoController _IOServoController;


        public static void Initialize(string hubURl, IO.IPWMServoController servoController)
        {
            _IOServoController = servoController;
            _IOServoController.PropertyChanged += _ServoController_PropertyChanged;

            _CloudHubConnection = new HubConnectionBuilder()
                   .WithAutomaticReconnect()
                   .WithUrl(hubURl)
                   .Build();

            _CloudHubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _CloudHubConnection.StartAsync();
            };

            AddMethods();

            _CloudHubConnection.StartAsync();

            
        }

        private static void AddMethods()
        {
            _CloudHubConnection.On<int>("Set_Degree", SetDegree);
            _CloudHubConnection.On("Connected", OnConnected);
        }


        public static void SetDegree(int angle)
        {
            _IOServoController.WriteAngle(angle);
        }

        private static void _ServoController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
            _CloudHubConnection.InvokeAsync("SetDegree_Pi", _IOServoController.ReadAngle());
        }

        public static void OnConnected() {
            _CloudHubConnection.InvokeAsync("JoinPiClientGroup");
            _CloudHubConnection.InvokeAsync("SetDegree_Pi", _IOServoController.ReadAngle()); //update the Hub with the current angle
        }

       
    }
}
