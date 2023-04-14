using intex2.Models;
using intex2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly PredictWrappingService _predictWrappingService;
        private readonly PredictSexService _predictSexService;


        public HomeController(ILogger<HomeController> logger, intexdataContext burialentrycontext, PredictWrappingService predictWrappingService, PredictSexService predictSexService)
        {
            _logger = logger;
            _context = burialentrycontext;
            _predictWrappingService = predictWrappingService;
            _predictSexService = predictSexService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Unsupervised()
        {

            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult Supervised()
        {
            var combinedInputData = new CombinedInputData
            {
                WrappingInputData = new WrappingInputData(),
                SexInputData = new SexInputData()
            };
            return View(combinedInputData);
        }
        [HttpPost]
        public async Task<IActionResult> PredictWrapping(CombinedInputData combinedInputData)
        {
            var wrappingInputData = combinedInputData.WrappingInputData;
            var input_data = new Dictionary<string, object>
            {
                { "depth", wrappingInputData.Depth},
                { "length", wrappingInputData.Length},
                { "headdirection_W", wrappingInputData.Headdirection_W},
                { "facebundles_Y", wrappingInputData.Facebundles_Y},
                { "goods_Y", wrappingInputData.Goods_Y},
                { "haircolor_B", wrappingInputData.Haircolor_B },
                { "haircolor_D", wrappingInputData.Haircolor_D },
                { "haircolor_K", wrappingInputData.Haircolor_K },
                { "haircolor_R", wrappingInputData.Haircolor_R },
                { "haircolor_unknown", wrappingInputData.Haircolor_unknown },
                { "samplescollected_true", wrappingInputData.Samplescollected_true },
                { "ageatdeath_C", wrappingInputData.Ageatdeath_C },
                { "ageatdeath_unknown", wrappingInputData.Ageatdeath_unknown },
            };
            {
                var result = await _predictWrappingService.CallPredictAPI(input_data);
                ViewBag.WrappingPrediction = result;
                return View("Supervised");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PredictSex(CombinedInputData combinedInputData)
        {
            var sexInputData = combinedInputData.SexInputData;
            var input_data = new Dictionary<string, object>
            {
                { "depth", sexInputData.Depth},
                { "length", sexInputData.Length},
                { "headdirection_W", sexInputData.Headdirection_W},
                { "facebundles_Y", sexInputData.Facebundles_Y},
                { "goods_Y", sexInputData.Goods_Y},
                { "wrapping_H", sexInputData.Wrapping_H},
                { "wrapping_W", sexInputData.Wrapping_W },
                { "haircolor_B", sexInputData.Haircolor_B },
                { "haircolor_D", sexInputData.Haircolor_D },
                { "haircolor_K", sexInputData.Haircolor_K },
                { "haircolor_R", sexInputData.Haircolor_R },
                { "haircolor_unknown", sexInputData.Haircolor_unknown },
                { "samplescollected_true", sexInputData.Samplescollected_true },
                { "ageatdeath_C", sexInputData.Ageatdeath_C },
                { "ageatdeath_unknown", sexInputData.Ageatdeath_unknown },
            };
            {
                var result = await _predictSexService.CallPredictSexAPI(input_data);
                ViewBag.SexPrediction = result;
                return View("Supervised");
            }
        }



        [HttpGet]
        public IActionResult Burials(int pageNum = 1)
        {
            int pageSize = 35;
            var x = new BurialsViewModel
            {
                BestFinalMergeds = _context.BestFinalMerged
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
        [Authorize]
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
        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [Authorize]
        //Post for Add page
        [HttpPost]
        public IActionResult Add(BestFinalMerged rd)
        {
            //if (ModelState.IsValid)
            //{
            //    //Add and Save input to db
            //    _context.BestFinalMerged.Add(rd);
            //    _context.SaveChanges();

            //    return View("Burials", rd);
            //}
            //else
            //{
            //    return View();
            //}
            return View("Confirmation");
        }

        [Authorize]
        //Edit View
        [HttpGet]
        public IActionResult Edit(int primaryid)
        {
            //ViewBag.Categories = _context.Categories.ToList();
            var burialEntry = _context.BestFinalMerged.Single(x => x.PrimaryId == primaryid);
            return View("Add", burialEntry);
        }
        [Authorize]
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
        [Authorize]
        //Delete View
        [HttpGet]
        public IActionResult Delete(int primaryid)
        {
            var burialEntry = _context.BestFinalMerged.Single(x => x.PrimaryId == primaryid);

            return View(burialEntry);
        }
        [Authorize]
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
