using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class CloudHub : Hub
    {
        static int _CurrentDegree = -1;

        const string WEB_APP_CLIENT_GROUP_NAME = "Web_App_Client";
        public async Task JoinWebAppClientGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, WEB_APP_CLIENT_GROUP_NAME);
            await Clients.Client(Context.ConnectionId).SendAsync("Degree_Status", _CurrentDegree); // give the client the current value
        }

        const string Pi_CLIENT_GROUP_NAME = "Pi_Client";
        public async Task JoinPiClientGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Pi_CLIENT_GROUP_NAME);
        }

        //WebApp Client Can call this method 
        public async Task SetDegree(int degree)
        {            
            await Clients.Group(Pi_CLIENT_GROUP_NAME).SendAsync("Set_Degree", degree);
        }

        //Pi.Console App can call this method
        public async Task DegreeStatus(int degree)
        {
            await Clients.Group(WEB_APP_CLIENT_GROUP_NAME).SendAsync("Degree_Status", degree);
            await Clients.All.SendAsync("Degree_Status", degree);
            _CurrentDegree = degree;
            Console.WriteLine($"Degree Status: {degree}");
        }

        public override Task OnConnectedAsync()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            
            
            Clients.Client(Context.ConnectionId).SendAsync("Connected"); // Tell The client they are connected
            return base.OnConnectedAsync();
        }

    }
}