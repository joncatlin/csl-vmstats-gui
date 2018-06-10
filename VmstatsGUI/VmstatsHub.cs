using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VmstatsGUI
{
    public class VmstatsHub : Hub
    {
        private static ConcurrentDictionary<string, string>connections = new ConcurrentDictionary<string, string>();

        // Method invoked by js client code
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.All.SendAsync("DisplayRaw", "hellow world");
        }

        // Method invoked when new client connects
        public override async Task OnConnectedAsync()
        {
            // Add the connection to the list of connections
            connections.TryAdd(Context.ConnectionId, "");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the connection from the set of connections
            string value = "";
            connections.TryRemove(Context.ConnectionId, out value);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
