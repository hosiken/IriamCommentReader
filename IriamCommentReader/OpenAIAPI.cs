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
            // 1. input配列の構築
            var inputList = new List<object>();

            // Developer Role (System Prompt)
            if (!string.IsNullOrEmpty(systemPrompt))
            {
                inputList.Add(new
                {
                    role = "developer",
                    content = new List<object>
            {
                new { type = "input_text", text = systemPrompt }
            }
                });
            }

            // User Role
            var userContent = new List<object>();
            userContent.Add(new { type = "input_text", text = userPrompt });

            if (!string.IsNullOrEmpty(fileBase64))
            {
                // 画像がある場合
                // ※Playground JSONには画像の具体例が含まれていませんでしたが、
                //  テキストが "input_text" なので、画像は "input_image" であると推測されます。
                userContent.Add(new
                {
                    type = "input_image",
                    image_url = $"data:image/jpeg;base64,{fileBase64}"
                });
            }

            inputList.Add(new
            {
                role = "user",
                content = userContent
            });

            // 2. リクエストボディの構築
            // PlaygroundのJSONにある項目を全て網羅します
            var requestBody = new
            {
                model = this.Model, // ★重要: ここに "gpt-5-mini" が入っていることを確認してください

                input = inputList,

                text = new
                {
                    format = new { type = "text" },
                    verbosity = "medium"
                },

                reasoning = new
                {
                    effort = "minimal",
                    summary = "concise"
                },

                // ★ここが前回のコードで足りなかった部分です
                // APIによっては空配列でも明示的に送らないと400になることがあります
                tools = new List<object>(),

                store = true,

                include = new[]
                {
                    "reasoning.encrypted_content",
                    "web_search_call.action.sources"
                }
            };

            return JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public override async Task<string> RequestAsync(string requestJson)
        {
            var requestUrl = "https://api.openai.com/v1/responses";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {this.APIKey}");
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            // 成功しなかった場合 (200 OK 以外)
            if (!response.IsSuccessStatusCode)
            {
                // サーバーからのエラーメッセージ(JSON)を読み取る
                string errorJson = await response.Content.ReadAsStringAsync();

                // デバッグしやすいように、ステータスコードとJSON本文を結合して例外を投げる
                throw new HttpRequestException($"API Error {response.StatusCode}: {errorJson}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

            string responseText = responseObject.output[1].content[0].text;
            
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
