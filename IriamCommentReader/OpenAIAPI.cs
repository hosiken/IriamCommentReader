using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace IriamCommentReader
{
    public class OpenAIAPI : LMBase
    {
        public OpenAIAPI(string apiKey) : base(apiKey)
        {
        }

        public override Task<string> UploadImageAsync(Image image)
        {
            // ファイルアップロードは実装しない
            throw new NotImplementedException("This method is not implemented for OpenAIAPI.");
        }

        public override Task<string> UploadImageAsync(string filePath)
        {
            // ファイルアップロードは実装しない
            throw new NotImplementedException("This method is not implemented for OpenAIAPI.");
        }

        public override string GetRequestJson(string systemPrompt, string userPrompt, string fileBase64 = null)
        {
            var messages = new List<object>();

            // System Prompt
            messages.Add(new
            {
                role = "system",
                content = systemPrompt
            });

            // User Prompt
            var userContent = new List<object>();
            userContent.Add(new { type = "text", text = userPrompt });

            if (!string.IsNullOrEmpty(fileBase64))
            {
                userContent.Add(new
                {
                    type = "image_url",
                    image_url = new { url = $"data:image/jpeg;base64,{fileBase64}" }
                });
            }
            
            messages.Add(new
            {
                role = "user",
                content = userContent
            });

            var requestBody = new
            {
                model = this.Model,
                messages = messages,
                temperature = this.Temperature,
                top_p = this.TopP,
                max_tokens = 4096
            };

            return JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public override async Task<string> RequestAsync(string requestJson)
        {
            var requestUrl = "https://api.openai.com/v1/chat/completions";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {this.APIKey}");
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

            string responseText = responseObject.choices[0].message.content;
            
            return responseText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        public override async Task<string> RequestAsync(string systemPrompt, string userPrompt, string fileUri = null, string fileBase64 = null)
        {
            // fileUriはOpenAI APIでは直接使用しないため、fileBase64のみを考慮
            var requestJson = GetRequestJson(systemPrompt, userPrompt, fileBase64);
            return await RequestAsync(requestJson);
        }
    }
}