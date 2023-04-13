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
        //public IActionResult Burials(int pageNum = 1)
        public IActionResult Burials(int pageNum, string adultsubadultX, string sexX, string haircolorX, string facebundlesX, string headdirectionX, string ageatdeathX)

        {
            int pageSize = 35;
            var x = new BurialsViewModel
            {
                BestFinalMerges = _context.BestFinalMerged
                .Where(d => d.AdultsubadultX == adultsubadultX || adultsubadultX == null)
                .Where(d => d.SexX == sexX || sexX == null)
                .Where(d => d.FacebundlesX == facebundlesX || facebundlesX == null)
                .Where(d => d.HaircolorX == haircolorX || haircolorX == null)
                .Where(d => d.HeaddirectionX == headdirectionX || headdirectionX == null)
                .Where(d => d.AgeatdeathX == ageatdeathX || ageatdeathX == null)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {

                    TotalNumProjects = _context.BestFinalMerged
                    .Where(d => d.AdultsubadultX == adultsubadultX || adultsubadultX == null)
                    .Where(d => d.SexX == sexX || sexX == null)
                    .Where(d => d.FacebundlesX == facebundlesX || facebundlesX == null)
                    .Where(d => d.HaircolorX == haircolorX || haircolorX == null)
                    .Where(d => d.HeaddirectionX == headdirectionX || headdirectionX == null)
                    .Where(d => d.AgeatdeathX == ageatdeathX || ageatdeathX == null)
                    .Count(),
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

        public IActionResult Details(int primaryid)
        {
            var burial = _context.BestFinalMerged.Find(primaryid);

            if (burial == null)
            {
                return NotFound();
            }

            return View("BurialDetails", burial);
        }

        //Get for Add page
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }
        //Post for Movies page
        [HttpPost]
        public IActionResult Add(BestFinalMerged rd)
        {
            if (ModelState.IsValid)
            {
                //Add and Save input to db
                _context.Add(rd);
                _context.SaveChanges();

                return View("confirmation", rd);
            }
            else
            {
                return View();
            }
        }

        //Edit View
        [HttpGet]
        public IActionResult Edit(int primaryid)
        {
            //ViewBag.Categories = _context.Categories.ToList();
            var burialEntry = _context.BestFinalMerged.Single(x => x.PrimaryId == primaryid);
            return View("Add", burialEntry);
        }
        [HttpPost]
        public IActionResult Edit(BestFinalMerged mc)
        {
            if (ModelState.IsValid)
            {
                //Add and Save input to db
                _context.Update(mc);
                _context.SaveChanges();

                return RedirectToAction("Burials");
            }
            else
            {
               // ViewBag.Categories = _context.Categories.ToList();
                return View("Add");
            }
        }
        //Delete View
        [HttpGet]
        public IActionResult Delete(int primaryid)
        {
            var burialEntry = _context.BestFinalMerged.Single(x => x.PrimaryId == primaryid);

            return View(burialEntry);
        }
        [HttpPost]
        public IActionResult Delete(BestFinalMerged md)
        {
            _context.BestFinalMerged.Remove(md);
            _context.SaveChanges();

            return RedirectToAction("Burials");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
