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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        [HttpPost]
        public ContentResult MakePrediction(string headdirection, float depth, string facebundles, string goods, string haircolor, string samplescollected, float length, string ageatdeath)
        {
            // Set the path to the Python executable and the script
            string pythonExecutablePath = "C:/Users/kimba/AppData/Local/Programs/Python/Python310/python.exe";
            string pythonScriptPath = "C:/Users/kimba/OneDrive/Desktop/Python/predict.py";

            // Create the process
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonExecutablePath,
                Arguments = $"\"{pythonScriptPath}\" \"{headdirection}\" \"{depth}\" \"{facebundles}\" \"{goods}\" \"{haircolor}\" \"{samplescollected}\" \"{length}\" \"{ageatdeath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string prediction = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();
                string error = process.StandardError.ReadToEnd();
                System.Diagnostics.Debug.WriteLine("Error: " + error);
                System.Diagnostics.Debug.WriteLine("Output: " + prediction); // Use predMF instead of prediction in the MakePredictionMF action

                // Process the prediction result
                ViewBag.Prediction = prediction;
            }

            return Content(ViewBag.Prediction.Trim());
        }

        [HttpPost]
        public ContentResult MakePredictionMF(string headdirection, float depth, string facebundles, string goods, string wrapping, string haircolor, string samplescollected, float length, string ageatdeath)
        {
            // Set the path to the Python executable and the script
            string pythonExecutablePathMF = "C:/Users/kimba/AppData/Local/Programs/Python/Python310/python.exe";
            string pythonScriptPathMF = "C:/Users/kimba/OneDrive/Desktop/Python/predictMF.py";

            // Create the process
            ProcessStartInfo startInfoMF = new ProcessStartInfo
            {
                FileName = pythonExecutablePathMF,
                Arguments = $"\"{pythonScriptPathMF}\" \"{headdirection}\" \"{depth}\" \"{facebundles}\" \"{goods}\" \"{wrapping}\" \"{haircolor}\" \"{samplescollected}\" \"{length}\" \"{ageatdeath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process processMF = new Process { StartInfo = startInfoMF })
            {
                processMF.Start();
                string predMF = processMF.StandardOutput.ReadToEnd().Trim();
                processMF.WaitForExit();
                string error = processMF.StandardError.ReadToEnd();
                System.Diagnostics.Debug.WriteLine("Error: " + error);
                System.Diagnostics.Debug.WriteLine("Output: " + predMF); // Use predMF instead of prediction in the MakePredictionMF action

                // Process the prediction result
                ViewBag.PredMF = predMF;
            }

            return Content(ViewBag.PredMF.Trim());
        }

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