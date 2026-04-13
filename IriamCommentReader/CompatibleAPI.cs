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
            // 1. messages配列の構築 (旧: input配列)
            var messagesList = new List<object>();

            // System Role (旧: Developer Role)
            if (!string.IsNullOrEmpty(systemPrompt))
            {
                messagesList.Add(new
                {
                    role = "system",
                    content = new List<object>
                    {
                        // 旧: input_text -> 新: text
                        new { type = "text", text = systemPrompt }
                    }
                });
            }

            // User Role
            var userContent = new List<object>();
            // 旧: input_text -> 新: text
            userContent.Add(new { type = "text", text = userPrompt });

            if (!string.IsNullOrEmpty(fileBase64))
            {
                userContent.Add(new
                {
                    // 旧: input_image -> 新: image_url
                    type = "image_url",
                    // chat/completions では image_url の中に url プロパティを持つオブジェクトを渡す必要があります
                    image_url = new { url = $"data:image/jpeg;base64,{fileBase64}" }
                });
            }

            messagesList.Add(new
            {
                role = "user",
                content = userContent
            });

            // ★修正版：json_schemaプロパティで中身をラップする
            var responseSchema = new
            {
                type = "json_schema",
                json_schema = new  // ← 【重要】このラップが完全に抜けていました！
                {
                    name = "comment_extraction_schema",
                    strict = true,
                    schema = new
                    {
                        type = "object",
                        properties = new
                        {
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
                                    required = new[] { "name", "comment" },
                                    additionalProperties = false
                                }
                            }
                        },
                        required = new[] { "comments" },
                        additionalProperties = false
                    }
                }
            };
            // 2. リクエストボディの構築
            var requestObject = new Dictionary<string, object>
            {
                { "model", this.Model },
                { "messages", messagesList }, // 旧: input
                { "stream", true },
                { "stream_options", new { include_usage = true } }, // chat/completions のストリームでトークン数を取得するために必要
                { "response_format", responseSchema }, // 旧: text = new { format = ... }
                { "temperature", Temperature },
                { "top_p", TopP },
                // { "tools", new List<object>() }, // chat/completionsで空配列を渡すとエラーになるモデルがあるため除外推奨
                // { "store", true } // 必要に応じて追加
            };

            return JsonConvert.SerializeObject(requestObject, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public override async Task<string> RequestAsync(string requestJson)
        {
            // エンドポイントを chat/completions に変更
            var requestUrl = $"{BaseURL}/v1/chat/completions";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            // request.Headers.Add("Authorization", $"Bearer {this.APIKey}");
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorJson = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API Error {response.StatusCode}: {errorJson}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            LastResponse = jsonResponse;
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

            // パースのパスを chat/completions 仕様に変更
            string responseText = responseObject.choices[0].message.content;

            if (responseObject.usage != null)
            {
                LastUsage.inputTokens = responseObject.usage.prompt_tokens; // input_tokens -> prompt_tokens
                LastUsage.outputTokens = responseObject.usage.completion_tokens; // output_tokens -> completion_tokens
                LastUsage.totalTokens = responseObject.usage.total_tokens;
            }

            return responseText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        public override async Task<string> RequestAsync(string systemPrompt, string userPrompt, string fileUri = null, string fileBase64 = null)
        {
            var requestJson = GetRequestJson(systemPrompt, userPrompt, fileBase64);
            return await RequestAsync(requestJson);
        }

        public override async Task RequestStreamAsync(string requestJson, Action<string> onTokenReceived, System.Threading.CancellationToken cancellationToken = default)
        {
            // エンドポイントを chat/completions に変更
            var requestUrl = $"{BaseURL}/v1/chat/completions";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {this.APIKey}");
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            LastResponse = string.Empty;

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"API Error {response.StatusCode}: {errorJson}");
                }

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

                            // テキストチャンクの抽出 (chat/completions 仕様)
                            if (ev.choices != null && ev.choices.Count > 0)
                            {
                                var delta = ev.choices[0].delta;
                                if (delta != null && delta.content != null)
                                {
                                    string chunk = delta.content;
                                    if (!string.IsNullOrEmpty(chunk))
                                    {
                                        onTokenReceived?.Invoke(chunk.Replace("\n", "\r\n"));
                                        LastResponse += chunk.Replace("\n", "\r\n");
                                    }
                                }
                            }

                            // トークン情報の更新 (stream_options: { include_usage: true } を指定した場合、最後のチャンクに含まれる)
                            if (ev.usage != null)
                            {
                                this.LastUsage.inputTokens = ev.usage.prompt_tokens;
                                this.LastUsage.outputTokens = ev.usage.completion_tokens;
                                this.LastUsage.totalTokens = ev.usage.total_tokens;
                            }
                        }
                        catch { /* パースエラーはスキップ */ }
                    }
                }
            }
        }
    }
}