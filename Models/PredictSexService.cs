using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace intex2.Models
{
    public class PredictSexService
    {
        public async Task<string> CallPredictSexAPI(Dictionary<string, object> inputData)
        {
            var httpClient = new HttpClient();
            string apiBaseUrl = "http://3.95.10.114"; // Replace with your EC2 instance IP and port where the FastAPI app is running
            string apiEndpoint = "/predictsex";
            string apiUrl = apiBaseUrl + apiEndpoint;

            var jsonContent = JsonConvert.SerializeObject(inputData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                return result["prediction"];
            }
            else
            {
                // Handle the error response here
                return null;
            }
        }
    }
}