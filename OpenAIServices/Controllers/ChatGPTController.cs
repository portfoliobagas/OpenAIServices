using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAIServices.Helper;
using System.Text.RegularExpressions;

namespace OpenAIServices.Controllers
{
    [Route("api")]
    [ApiController]
    public class ChatGPTController : Controller
    {
        private static string? session = null;
        private static DateTime sessionTime = DateTime.Now;

        [HttpGet("ChatFitalisa")]
        public async Task<IActionResult> Chat(string prompt)
        {
            #region Initial Object and Variable

            string outputResult = "";
            string conversationId = "";
            string originalPrompt = prompt;

            string[] trimStrings = { "Fitalisa:", "Program:", "Sistem:", "Bot:" };
            char[] trimChars = { ' ', ',', '.', '?', '!', ':', '\t', '\n', '\r' };

            var openai = new OpenAIAPI(Constan.STR_KEY_OPENAI);
            CompletionRequest completionRequest = new CompletionRequest();

            #endregion

            #region Validation

            DateTime timeoutTime = sessionTime.AddMinutes(1);

            if (timeoutTime < DateTime.Now)
            {
                outputResult = Constan.STR_MESSAGE_SESSION + session;
                return Unauthorized(outputResult);
            }

            #endregion

            #region Initial Personal Model GPT

            if (session == null)
            {
                prompt = Constan.STR_PERSONAL_1_MODEL_GPT + "User: " + prompt;
            }
            else
            {
                prompt = "Fitalisa: " + session + "\n\nUser: " + prompt;
            }

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

            foreach (var trimString in trimStrings)
            {
                outputResult = Regex.Replace(outputResult, Regex.Escape(trimString), "").Trim(trimChars);
            }

            //Isi data session agar percakapan bisa terus berlanjut.
            if (session != null)
            {
                session = session + "\n\n" + "User: " + originalPrompt + "\n\n" + "Fitalisa: " + outputResult;
            }
            else
            {
                session = "User: " + originalPrompt + "\n\n" + "Fitalisa: " + outputResult;
            }

            #endregion

            return Ok(outputResult);

        }

        [HttpGet("TanyaFitalisa")]
        public async Task<IActionResult> Tanya(string prompt)
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

        [HttpPost("ResetSession")]
        public async Task<IActionResult> ResetSession()
        {
            session = null;
            sessionTime = DateTime.Now;
            return Ok("Sesi berhasil di reset.");
        }
    }
}
