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
        public async Task Process(string fromDate, string toDate, string vmPattern, string dsl)
        {
            Random rand = new Random();
            var xData = new string[1500];
            var yData = new float[1500];
            for (int i=0; i < 1500; i++)
            {
                xData[i] = "10:52";
                yData[i] = 1.0F * (float)rand.Next(0, 100);
            }

            await ReturnResultToClient(Context.ConnectionId, "rawGraph", xData, yData);

            for (int i = 0; i < 1500; i++)
            {
                xData[i] = "10:52";
                yData[i] = 1.0F * (float)rand.Next(0, 100);
            }

            await ReturnResultToClient(Context.ConnectionId, "processedGraph", xData, yData);
        }

        // Method invoked by js client code
        public async Task ReturnResultToClient(string connectionId, string displayArea, string[] xData, float[] yData)
        {
            await Clients.Client(connectionId).SendAsync("DisplayGraph", displayArea, xData, yData);
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
