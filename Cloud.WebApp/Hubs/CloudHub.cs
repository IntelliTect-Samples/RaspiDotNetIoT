using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class CloudHub : Hub
    {

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        const string WEB_APP_CLIENT_GROUP_NAME = "Web_App_Client";
        public async Task JoinWebAppClientGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, WEB_APP_CLIENT_GROUP_NAME);
        }

        const string Pi_CLIENT_GROUP_NAME = "Pi_Client";
        public async Task JoinPiClientGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Pi_CLIENT_GROUP_NAME);
        }

        public async Task SetDegree(int degree)
        {
            await Clients.Group(Pi_CLIENT_GROUP_NAME).SendAsync("Set_Degree", degree);
        }

        public async Task IncreaseAngle(int degree)
        {
            await Clients.Group(Pi_CLIENT_GROUP_NAME).SendAsync("Increase_Angle", degree);
        }

        public async Task DegreeStatus(int degree)
        {
            await Clients.Group(WEB_APP_CLIENT_GROUP_NAME).SendAsync("Degree_Status", degree);
            await Clients.All.SendAsync("Degree_Status", degree);
            Console.WriteLine($"Degree Status: {degree}");
        }
    }
}