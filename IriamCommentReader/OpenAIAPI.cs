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
        public bool IsGPT52 => Model == "gpt-5.1" || Model == "gpt-5.2";

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

            // ★ここが JSON Schema の定義部分です
            // C#の匿名オブジェクトで階層構造を作ります
            var responseSchema = new
            {
                type = "json_schema",
                name = "comment_extraction_schema", // 任意のスキーマ名
                strict = true, // 構造を強制する（重要）
                schema = new
                {
                    type = "object",
                    properties = new
                    {
                        // 【重要】
                        // 元のJSONでは "list" でしたが、C#側のクラス(CommentList)が
                        // "Comments" というプロパティを持っていると推測されるため、
                        // ここを "comments" にしておくと DeserializeObject でそのまま吸えます。
                        comments = new
                        {
                            type = "array",
                            description = "抽出されたコメントのリスト",
                            items = new
                            {
                                type = "object",
                                properties = new
                                {
                                    name = new { type = "string", description = "名前 (システムメッセージの場合は空文字)" },
                                    comment = new { type = "string", description = "コメント本文" }
                                },
                                // strict: true の場合、全フィールドが必須である必要があります
                                required = new[] { "name", "comment" },
                                additionalProperties = false
                            }
                        }
                    },
                    required = new[] { "comments" },
                    additionalProperties = false
                }
            };

            // 2. リクエストボディの構築
            var requestBody = new
            {
                model = this.Model, // "gpt-5-mini" など

                input = inputList,

                // テキスト生成設定にスキーマを埋め込む
                text = new
                {
                    format = responseSchema, // 作成したスキーマをセット
                    verbosity = "low"
                },

                reasoning = new
                {
                    effort = IsGPT52 ? "none" : "minimal",
                    summary = "concise"
                },

                tools = new List<object>(),

                store = true,

                include = new string[]
                {
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
            LastResponse = jsonResponse;
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

            string responseText = responseObject.output[IsGPT52 ? 0 : 1].content[0].text;

            LastUsage.inputTokens = responseObject.usage.input_tokens;
            LastUsage.outputTokens = responseObject.usage.output_tokens;
            LastUsage.totalTokens = responseObject.usage.total_tokens;

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
