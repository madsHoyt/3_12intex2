using intex2.Models;
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

        // private readonly PredictSexService _predictSexService;
        private readonly PredictWrappingService _predictWrappingService;

        public HomeController(ILogger<HomeController> logger, PredictSexService predictSexService, PredictWrappingService predictWrappingService)
        {
            _logger = logger;
            //_predictSexService = predictSexService;
            _predictWrappingService = predictWrappingService;
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

        [HttpGet]
        public IActionResult Supervised()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PredictWrapping(WrappingInputData wrappingInputData)
        {
            var input_data = new Dictionary<string, object>
            {
                {"depth", wrappingInputData.Depth},
                {"length", wrappingInputData.Length},
                {"headdirection_W", wrappingInputData.Headdirection_W},
                {"facebundles_Y", wrappingInputData.Facebundles_Y},
                {"goods_Y", wrappingInputData.Goods_Y},
                {"haircolor_B", wrappingInputData.Haircolor_B },
                {"haircolor_D", wrappingInputData.Haircolor_D },
                {"haircolor_K", wrappingInputData.Haircolor_K },
                {"haircolor_R", wrappingInputData.Haircolor_R },
                {"haircolor_unknown", wrappingInputData.Haircolor_unknown },
                {"samplescollected_true", wrappingInputData.Samplescollected_true },
                {"ageatdeath_C", wrappingInputData.Ageatdeath_C },
                {"ageatdeath_unknown", wrappingInputData.Ageatdeath_unknown },
            };
            {
                var result = await _predictWrappingService.CallPredictAPI(input_data);
                ViewBag.WrappingPrediction = result;
                return View("Supervised");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> PredictSex(SexInputData sexInputData)
        //{
        //    var input_data = new Dictionary<string, object>
        //    {
        //        {"depth", sexInputData.Depth},
        //        {"length", sexInputData.Length},
        //        {"headdirection_W", sexInputData.Headdirection_W},
        //        {"facebundles_Y", sexInputData.Facebundles_Y},
        //        {"goods_Y", sexInputData.Goods_Y},
        //        {"wrapping_H", sexInputData.Wrapping_H},
        //        {"wrapping_W", sexInputData.Wrapping_W },
        //        {"haircolor_B", sexInputData.Haircolor_B },
        //        {"haircolor_D", sexInputData.Haircolor_D },
        //        {"haircolor_K", sexInputData.Haircolor_K },
        //        {"haircolor_R", sexInputData.Haircolor_R },
        //        {"haircolor_unknown", sexInputData.Haircolor_unknown },
        //        {"samplescollected_true", sexInputData.Samplescollected_true },
        //        {"ageatdeath_C", sexInputData.Ageatdeath_C },
        //        {"ageatdeath_unknown", sexInputData.Ageatdeath_unknown },
        //    };
        //    {
        //        var result = await _predictSexService.CallPredictSexAPI(input_data);
        //        ViewBag.SexPrediction = result;
        //        return View();
        //    }
        //}

        //[HttpPost]
        //public ContentResult MakePrediction(string headdirection, float depth, string facebundles, string goods, string haircolor, string samplescollected, float length, string ageatdeath)
        //{
        //    // Set the path to the Python executable and the script
        //    string pythonExecutablePath = "C:/Users/kimba/AppData/Local/Programs/Python/Python310/python.exe";
        //    string pythonScriptPath = "C:/Users/kimba/OneDrive/Desktop/Python/predict.py";

        //    // Create the process
        //    ProcessStartInfo startInfo = new ProcessStartInfo
        //    {
        //        FileName = pythonExecutablePath,
        //        Arguments = $"\"{pythonScriptPath}\" \"{headdirection}\" \"{depth}\" \"{facebundles}\" \"{goods}\" \"{haircolor}\" \"{samplescollected}\" \"{length}\" \"{ageatdeath}\"",
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };

        //    using (Process process = new Process { StartInfo = startInfo })
        //    {
        //        process.Start();
        //        string prediction = process.StandardOutput.ReadToEnd().Trim();
        //        process.WaitForExit();
        //        string error = process.StandardError.ReadToEnd();
        //        System.Diagnostics.Debug.WriteLine("Error: " + error);
        //        System.Diagnostics.Debug.WriteLine("Output: " + prediction); // Use predMF instead of prediction in the MakePredictionMF action

        //        // Process the prediction result
        //        ViewBag.Prediction = prediction;
        //    }

        //    return Content(ViewBag.Prediction.Trim());
        //}

        //[HttpPost]
        //public ContentResult MakePredictionMF(string headdirection, float depth, string facebundles, string goods, string wrapping, string haircolor, string samplescollected, float length, string ageatdeath)
        //{
        //    // Set the path to the Python executable and the script
        //    string pythonExecutablePathMF = "C:/Users/kimba/AppData/Local/Programs/Python/Python310/python.exe";
        //    string pythonScriptPathMF = "C:/Users/kimba/OneDrive/Desktop/Python/predictMF.py";

        //    // Create the process
        //    ProcessStartInfo startInfoMF = new ProcessStartInfo
        //    {
        //        FileName = pythonExecutablePathMF,
        //        Arguments = $"\"{pythonScriptPathMF}\" \"{headdirection}\" \"{depth}\" \"{facebundles}\" \"{goods}\" \"{wrapping}\" \"{haircolor}\" \"{samplescollected}\" \"{length}\" \"{ageatdeath}\"",
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };

        //    using (Process processMF = new Process { StartInfo = startInfoMF })
        //    {
        //        processMF.Start();
        //        string predMF = processMF.StandardOutput.ReadToEnd().Trim();
        //        processMF.WaitForExit();
        //        string error = processMF.StandardError.ReadToEnd();
        //        System.Diagnostics.Debug.WriteLine("Error: " + error);
        //        System.Diagnostics.Debug.WriteLine("Output: " + predMF); // Use predMF instead of prediction in the MakePredictionMF action

        //        // Process the prediction result
        //        ViewBag.PredMF = predMF;
        //    }

        //    return Content(ViewBag.PredMF.Trim());
        //}

        [HttpGet]
        public IActionResult Burials()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Burials()
        //{
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}