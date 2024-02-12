using Ergo.Api.Models.OpenAi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OpenAI_API;
using OpenAI_API.Chat;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ergo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatGptController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ChatGptController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ChatGptRequest request)
        {
            string messageContent = "";
            if(request.Type != "task" && request.Type != "project-tasks")
            {
                return BadRequest("Invalid type");
            }
            messageContent = request.Type switch
            {
                "task" => $"Generate me a task description maximum 300 tokens based on this title {request.Title} and on on this phrase {request.Description}",
                "project-tasks" => $"Generate a JSON file of type \"tasks\":[{{\"taskName\": string, \"taskDescription\": string}}] for a project named {request.Title} with the description {request.Description} where you divide the project in tasks. Include only the JSON file in the response, nothing else.",
                _ => $"Generate me a task description maximum 300 tokens based on this phrase {request.Description}",
            };
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { new { role = "user", content = messageContent } },
                max_tokens = 300
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var chatResponse = JObject.Parse(responseContent).ToObject<OpenAIChatResponse>();

                var firstChoiceContent = chatResponse.Choices.First().Message.Content;

                return Ok(firstChoiceContent);

            }

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }



    }
}
