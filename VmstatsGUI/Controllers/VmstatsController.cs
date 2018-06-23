using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VmstatsGUI
{
    // TODO Figure out a better way of sharing classes for messages etc between vmstats and vmstatsgui
    public class VmstatsController : Controller
    {

        private IHubContext<VmstatsHub> context;

        public VmstatsController(IHubContext<VmstatsHub> hub)
        {
            this.context = hub;
        }



        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Vmstats/Process
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
/*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int id, [Bind("ID,FromDate,ToDate,Dsl,VmPattern")] Request request)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
*/

        [HttpPost]
        /// <summary>  
        ///  This method receives the metrics produced by the vmstats component, from the data it holds and the DSL passed to it from the client. This method
        ///  formats the correct response and then calls the client code in the broiwser using SignalR.
        /// </summary>  
        public async Task ReturnResultToClient([FromBody] Messages.TransformSeries msg)
        {
            Console.WriteLine("Received TransformSeries");

            // Extract the data to be displayed as a graph
            var xData = msg.Measurements.Values.Keys;
            var yData = msg.Measurements.Values.Values;


            // TODO plumb in the connection Id all the way through to vmstats and back again
            string connectionId = "1";


            // Send the correct information back to the client to display the graph
            await context.Clients.Client(connectionId).SendAsync("DisplayGraph", "processedGraph", xData, yData);
        }


    }
}
