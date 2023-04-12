using intex2.Models;
using intex2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private intexdataContext _context { get; set; }
        public HomeController(ILogger<HomeController> logger, intexdataContext burialentrycontext)
        {
            _logger = logger;
            _context = burialentrycontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Unsupervised()
        {

            return View();
        }

        public IActionResult Supervised()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Burials(int pageNum = 1)
        {
            int pageSize = 30;
            var x = new BurialsViewModel
            {
                BestFinalMerges = _context.BestFinalMerged
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumProjects = _context.BestFinalMerged.Count(),
                    BurialsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };
            return View(x);
        }

        [HttpPost]
        public IActionResult Burials(BestFinalMerged bfm)
        {
            _context.Add(bfm);
            _context.SaveChanges();
            return View("Confirmation", bfm);
        }

        public IActionResult Details(int PrimaryKey)
        {
            var burial = _context.BestFinalMerged.Find(PrimaryKey);

            if (burial == null)
            {
                return NotFound();
            }

            return View("BurialDetails", burial);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
