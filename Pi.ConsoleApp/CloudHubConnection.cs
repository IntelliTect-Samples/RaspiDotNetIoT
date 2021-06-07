﻿using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Pi.ConsoleApp
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

            _CloudHubConnection.StartAsync();

            
        }

        private static void AddMethods()
        {
            _CloudHubConnection.On("Increase_Angle", IncreaseAngle);
            _CloudHubConnection.On<int>("Set_Degree", SetDegree);
            _CloudHubConnection.On("Connected", OnConnected);
        }

        public static void IncreaseAngle()
        {
            _IOServoController.IncreaseAngle();
        }

        public static void DecreaseAngle()
        {
            _IOServoController.DecreaseAngle();
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
            _CloudHubConnection.InvokeAsync("DegreeStatus", _IOServoController.ReadAngle());
        }

        public static void OnConnected() {
            _CloudHubConnection.InvokeAsync("JoinPiClientGroup");
        }

       
    }
}