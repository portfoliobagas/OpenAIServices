using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using OpenAI_API;
using Microsoft.VisualBasic;
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
            var openai = new OpenAIAPI(Constan.STR_KEY_OPENAI);
            CompletionRequest completionRequest = new CompletionRequest();
            completionRequest.Prompt = prompt;

            #endregion

            #region Set Model GPT

            completionRequest.Model = Constan.STR_MODEL_GPT;
            completionRequest.MaxTokens = 100;

            #endregion

            #region Execute..

            var completions = await openai.Completions.CreateCompletionAsync(completionRequest);

            foreach (var completion in completions.Completions)
            {
                outputResult += completion.Text;
            }

            #endregion

            return Ok(outputResult);

        }
    }
}
