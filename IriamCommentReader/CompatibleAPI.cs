using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IriamCommentReader
{
    public class CompatibleAPI : LMBase
    {
        public string BaseURL { get; set; }
        public bool IsGPT5 => Model.StartsWith("gpt-5") && !Model.Contains("chat");
        public bool IsLaterGPT51 => Model == "gpt-5.1" || Model == "gpt-5.2" || Model.StartsWith("gpt-5.4");
        public bool IsOx => Model.StartsWith("o");

        public CompatibleAPI(string apiKey) : base(apiKey)
        {
        }

        public override Task<string> UploadImageAsync(Image image)
        {
            // ファイルアップロードは実装しない
            throw new NotImplementedException("This method is not implemented for CompatibleAPI.");
        }

        public override Task<string> UploadImageAsync(string filePath)
        {
            // ファイルアップロードは実装しない
            throw new NotImplementedException("This method is not implemented for CompatibleAPI.");
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
                strict = true, // 構造を強制する（重要）
                json_schema = new
                {
                    name = "comment_extraction_schema", // 任意のスキーマ名
                    type = "object",
                    schema = new
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
            var requestObject = new Dictionary<string, object>
            {
                { "model", this.Model },
                { "input", inputList },
                { "stream", true }, // ここで切り替え
                { "response_format", responseSchema },
                { "tools", new List<object>() },
                { "store", true }
            };

            return JsonConvert.SerializeObject(requestObject, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public override async Task<string> RequestAsync(string requestJson)
        {
            var requestUrl = $"{BaseURL}/v1/responses";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            // request.Headers.Add("Authorization", $"Bearer {this.APIKey}");
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

            string responseText = responseObject.output[0].content[0].text;

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

        public override async Task RequestStreamAsync(string requestJson, Action<string> onTokenReceived, System.Threading.CancellationToken cancellationToken = default)
        {
            var requestUrl = $"{BaseURL}/v1/responses";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {this.APIKey}");
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            LastResponse = string.Empty;

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
                    {
                        var line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: ")) continue;

                        var dataStr = line.Substring(6).Trim();
                        if (dataStr == "[DONE]") break;

                        try
                        {
                            dynamic ev = JsonConvert.DeserializeObject(dataStr);
                            // Debug.WriteLine($"{ev.type}");
                            // OpenAI v1/responses API の仕様: テキストは response.text_delta で届く
                            if (ev.type == "response.output_text.delta")
                            {
                                string chunk = ev.delta;
                                if (!string.IsNullOrEmpty(chunk))
                                {
                                    onTokenReceived?.Invoke(chunk.Replace("\n", "\r\n"));
                                    LastResponse += chunk.Replace("\n", "\r\n");
                                }
                            }
                            // --- トークン情報の更新 ---
                            else if (ev.type == "response.completed")
                            {
                                if (ev.response?.usage != null)
                                {
                                    this.LastUsage.inputTokens = ev.response.usage.input_tokens;
                                    this.LastUsage.outputTokens = ev.response.usage.output_tokens;
                                    this.LastUsage.totalTokens = ev.response.usage.total_tokens;
                                }
                            }
                        }
                        catch { /* パースエラーはスキップ */ }
                    }
                }
            }
        }

        // public override Task RequestStreamAsync(string systemPrompt, string userPrompt, string fileBase64, Action<string> onTokenReceived, System.Threading.CancellationToken cancellationToken = default)
        // {
        // var requestJson = GetRequestJsonInternal(systemPrompt, userPrompt, fileBase64, true);
        //     return RequestStreamAsync(requestJson, onTokenReceived, cancellationToken);
        // }
    }
}