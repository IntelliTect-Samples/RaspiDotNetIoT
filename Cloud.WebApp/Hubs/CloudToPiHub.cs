using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class CloudToPiHub : Hub
    {
       

        public async Task SendToAll(string endpoint, string arg, int value)
        {
            await Clients.All.SendAsync(endpoint, arg, value);
        }

        public async Task OnResponse(string endpoint, string arg, int value)
        {
            await Clients.All.SendAsync(endpoint, arg, value);
        }

        public async Task SetColor(int color)
        {
           
            await Clients.All.SendAsync("SetColor_All", color);
        }

        public void DegreeStatus(int degree)
        {
            
        }
    }
}