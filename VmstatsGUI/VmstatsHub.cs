using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VmstatsGUI
{
    public class URL : Hub
    {
        // Names of all the environment variables needed by the application
        static readonly string ENV_VMSTATS_WEBSERVER_URL = "VMSTATS_WEBSERVER_URL";

        // Initialize the URL from the environment variables
        private string webserverUrl = GetEnvironmentVariable(ENV_VMSTATS_WEBSERVER_URL);

        // Data structure to keep track of all the users in case we need to get the ids to send them messages
        private static ConcurrentDictionary<string, string>connections = new ConcurrentDictionary<string, string>();

        // Method invoked by js client code
        public async Task Process(string fromDate, string toDate, string vmPattern, string dsl)
        {
            // Contact the vmstats command webserver and send it the details
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(webserverUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // Create a new ProcessCommand with the supplied data
            var pcmd = new ProcessCommand();
            pcmd.FromDate = Convert.ToDateTime(fromDate);
            pcmd.ToDate = Convert.ToDateTime(toDate);
            pcmd.VmPattern = vmPattern;
            pcmd.Dsl = dsl;
            string postBody = JsonConvert.SerializeObject(pcmd);

            try
            {
                // List data response.
                HttpResponseMessage response = await client.PostAsync(webserverUrl, new StringContent(postBody, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success!");
                } else
                {
                    Console.WriteLine("Failure!");
                }
            }
            catch (HttpRequestException hre)
            {
                Console.WriteLine("ERROR Calling vmstats webserver. Error is " + hre.Message);
            }
            catch (TaskCanceledException tce)
            {
                Console.WriteLine("ERROR Calling vmstats webserver. Error is " + tce.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Calling vmstats webserver. Error is " + ex.Message);
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                    client = null;
                }
            }




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

        private static string GetEnvironmentVariable(string envVarName)
        {
            string temp = Environment.GetEnvironmentVariable(envVarName);
            if (temp == null)
            {
                // Log an error and exit the program
                //                _log.Error($"ERROR: Missing environment variable named: {envVarName}");
                Console.WriteLine($"ERROR: Missing environment variable named: {envVarName}");
                System.Environment.Exit(-1);
            }

            // Output the env variable's value
            //            _log.Info($"Environment variable initialized. Variable: {envVarName}. Value: {temp}");
            Console.WriteLine($"Environment variable initialized. Variable: {envVarName}. Value: {temp}");
            return temp;
        }
    }
}
