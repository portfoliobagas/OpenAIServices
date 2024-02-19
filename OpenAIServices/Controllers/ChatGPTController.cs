using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAIServices.Helper;

namespace OpenAIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Chat(string prompt)
        {
            #region Initial Object and Variable

            string outputResult = "";
            string conversationId = "";

            char[] trimChars = { ' ', ',', '.', '?', '!', ':', '\t', '\n', '\r' };

            var openai = new OpenAIAPI(Constan.STR_KEY_OPENAI);
            CompletionRequest completionRequest = new CompletionRequest();

            #endregion

            #region Initial Personal Model GPT

            prompt = Constan.STR_PERSONAL_1_MODEL_GPT + prompt;

            #endregion

            #region Set Model GPT

            completionRequest.Model = Constan.STR_MODEL_GPT;
            completionRequest.MaxTokens = 1024;
            completionRequest.Prompt = prompt;

            #endregion

            #region Execute..

            var completions = await openai.Completions.CreateCompletionAsync(completionRequest);
            conversationId = completions.Id; // Assuming a property exists



            foreach (var completion in completions.Completions)
            {
                outputResult += completion.Text.Trim(trimChars);
            }

            #endregion

            return Ok(outputResult);

        }
    }
}
