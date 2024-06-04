using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogCore.Areas.Cliente.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly HttpClient _httpClient;

        public ChatbotController()
        {
            _httpClient = new HttpClient();
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(string message)
        {
            var apiKey = "YOUR_OPENAI_API_KEY";
            var url = "https://api.openai.com/v1/engines/davinci-codex/completions";

            var requestBody = new
            {
                prompt = message,
                max_tokens = 150,
                n = 1,
                stop = (string)null,
                temperature = 0.7
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<dynamic>(responseString);
            var botResponse = responseData.choices[0].text;

            return Json(new { response = botResponse });
        }
    }
}
