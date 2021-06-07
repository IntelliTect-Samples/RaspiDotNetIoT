using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Pi.ConsoleApp
{

    public static class CloudHubConnection
    {
        static HubConnection _CloudHubConnection;
        private static IO.IPWMServoController _ServoController;


        public static void Initialize(string hubURl, IO.IPWMServoController servoController)
        {
            _ServoController = servoController;
            _ServoController.PropertyChanged += _ServoController_PropertyChanged;

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
            _ServoController.WriteAngle(angle);
        }

        private static void _ServoController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {

                case nameof(_ServoController.CurrentAngle):
                    DegreeChange();
                    break;
            }
        }
        public static void DegreeChange()
        {
            _CloudHubConnection.InvokeAsync("SetDegree_Pi", _ServoController.ReadAngle());
        }

        public static void OnConnected() {
            _CloudHubConnection.InvokeAsync("JoinPiClientGroup");
            _CloudHubConnection.InvokeAsync("SetDegree_Pi", _ServoController.ReadAngle()); //update the Hub with the current angle
        }

       
    }
}
