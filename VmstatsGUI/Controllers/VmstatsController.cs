using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VmstatsGUI
{
    public class VmstatsController : Controller
    {
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

/*
        public IActionResult Process(string vmpattern = "", string date = "", string dsl = "")
        {
            ViewData["Vmpattern"] = vmpattern;
            ViewData["Date"] = date;
            ViewData["Dsl"] = dsl;

            return View();
        }
*/



        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int id, [Bind("ID,FromDate,ToDate,Dsl,VmPattern")] Request request)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }








    }
}
