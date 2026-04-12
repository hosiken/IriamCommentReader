using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IriamCommentReader
{
    public class GeminiSchema
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, GeminiSchema> Properties { get; set; }

        [JsonProperty("required", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Required { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public GeminiSchema Items { get; set; }
    }

    public class GeminiAPI : LMBase
    {
        public bool IsModelGemini25 => Model != null && Model.Contains("-2.5");
        public bool IsThinkingConfigEnable => Model != null && (Model.StartsWith("gemini-3") || Model.StartsWith("gemma-4"));
        public bool IsModelGeminiPro => Model != null && Model.Contains("-pro");
        public GeminiSchema Schema { get; set; }
        public GeminiThinkingConfig ThinkingConfig { get; set; } = new GeminiThinkingConfig();

        public class GeminiThinkingConfig
        {
            [JsonProperty("thinkingLevel")]
            public string ThinkingLevel { get; set; } = "HIGH";
        }

        public GeminiAPI(string apiKey) : base(apiKey)
        {
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        { 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public override async Task<string> UploadImageAsync(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                var encParam = new EncoderParameters(1);
                encParam.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);
                image.Save(memoryStream, GetEncoder(ImageFormat.Jpeg), encParam); // PNG 形式で保存
                memoryStream.Position = 0; // ストリームの位置を先頭に戻す

                var contentLength = memoryStream.Length;
                var contentType = "image/jpeg"; // or get from file extension

                var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={APIKey}";

                var startUploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
                startUploadRequest.Headers.Add("X-Goog-Upload-Command", "start, upload, finalize");
                startUploadRequest.Headers.Add("X-Goog-Upload-Header-Content-Length", contentLength.ToString());
                startUploadRequest.Headers.Add("X-Goog-Upload-Header-Content-Type", contentType);
                startUploadRequest.Content = new StringContent($"{{\"file\": {{\"display_name\": \"image.jpg\"}}}}", Encoding.UTF8, "application/json");

                var startUploadResponse = await _client.SendAsync(startUploadRequest);
                startUploadResponse.EnsureSuccessStatusCode();

                var uploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
                uploadRequest.Content = new StreamContent(memoryStream); // memoryStream を使用する

                var uploadResponse = await _client.SendAsync(uploadRequest);
                uploadResponse.EnsureSuccessStatusCode();

                var jsonResponse = await uploadResponse.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                return responseObject.file.uri;
            }
        }

        public override async Task<string> UploadImageAsync(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var contentLength = fileStream.Length;
                var contentType = "image/jpeg"; // or get from file extension

                var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={APIKey}";

                var startUploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
                startUploadRequest.Headers.Add("X-Goog-Upload-Command", "start, upload, finalize");
                startUploadRequest.Headers.Add("X-Goog-Upload-Header-Content-Length", contentLength.ToString());
                startUploadRequest.Headers.Add("X-Goog-Upload-Header-Content-Type", contentType);
                startUploadRequest.Content = new StringContent($"{{\"file\": {{\"display_name\": \"{Path.GetFileName(filePath)}\"}}}}", Encoding.UTF8, "application/json");

                var startUploadResponse = await _client.SendAsync(startUploadRequest);
                startUploadResponse.EnsureSuccessStatusCode();

                var uploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
                uploadRequest.Content = new StreamContent(fileStream);

                var uploadResponse = await _client.SendAsync(uploadRequest);
                uploadResponse.EnsureSuccessStatusCode();

                var jsonResponse = await uploadResponse.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                return responseObject.file.uri;
            }
        }

        public override string GetRequestJson(string systemPrompt, string userPrompt, string fileBase64 = null)
        {
            return GetRequestJsonV25(systemPrompt, userPrompt, fileBase64: fileBase64);
        }

        public string GetRequestJsonV25(string systemPrompt, string userPrompt, string fileUri = null, string fileBase64 = null, GeminiSchema schema = null, GeminiThinkingConfig thinkingConfig = null)
        {
            var generationConfig = new Dictionary<string, object>
            {
                { "temperature", Temperature },
                // { "topK", 40 },
                { "topP", TopP },
                { "maxOutputTokens", 8192 }
            };

            if (TopK.HasValue) generationConfig["topK"] = TopK.Value;
            // if (FrequencyPenalty.HasValue) generationConfig["frequencyPenalty"] = FrequencyPenalty.Value;
            // if (PresencePenalty.HasValue) generationConfig["presencePenalty"] = PresencePenalty.Value;

            if (IsThinkingConfigEnable)
            {
                generationConfig.Add("thinkingConfig", thinkingConfig ?? ThinkingConfig ?? new GeminiThinkingConfig());
            }

            if (schema != null || Schema != null)
            {
                generationConfig["responseMimeType"] = "application/json";
                generationConfig["responseSchema"] = schema ?? Schema;
            }
            else
            {
                generationConfig["responseMimeType"] = "text/plain";
            }

            var contentsList = new List<object>();
            var userParts = new List<object>();

            userPrompt = userPrompt.Replace("\r\n", "\n").Replace("\r", "\n");
            systemPrompt = systemPrompt.Replace("\r\n", "\n").Replace("\r", "\n");

            userParts.Add(new { text = userPrompt });

            if (fileUri != null && fileUri != "")
            {
                userParts.Add(new
                {
                    fileData = new
                    {
                        fileUri = fileUri,
                        mimeType = "image/jpeg"
                    }
                });
            }
            else if (fileBase64 != null && fileBase64 != "")
            {
                userParts.Add(new
                {
                    inlineData = new
                    {
                        mimeType = "image/jpeg",
                        data = fileBase64
                    }
                });
            }

            contentsList.Add(new
            {
                role = "user",
                parts = userParts.ToArray()
            });

            var requestBody = new
            {
                contents = contentsList.ToArray(),
                systemInstruction = new
                {
                    parts = new object[]
                    {
                        new { text = systemPrompt }
                    }
                },
                generationConfig
            };

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(requestBody, settings);
        }

        public override async Task<string> RequestAsync(string requestJson)
        {
            var generateUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={APIKey}";
            var generateRequest = new HttpRequestMessage(HttpMethod.Post, generateUrl);
            generateRequest.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var generateResponse = await _client.SendAsync(generateRequest);
            generateResponse.EnsureSuccessStatusCode();

            var generateJsonResponse = await generateResponse.Content.ReadAsStringAsync();
            dynamic generateResponseObject = JsonConvert.DeserializeObject(generateJsonResponse);
            string transcribedText = "";
            if (generateResponseObject.candidates[0].content.parts != null)
            {
                foreach (var part in generateResponseObject.candidates[0].content.parts)
                {
                    transcribedText += part.text;
                }
            }

            transcribedText = transcribedText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
            return transcribedText;
        }

        public override async Task RequestStreamAsync(string requestJson, Action<string> onTokenReceived, System.Threading.CancellationToken cancellationToken = default)
        {
            // streamGenerateContent を使用。alt=sse は解析を容易にするため付与
            var generateUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:streamGenerateContent?alt=sse&key={APIKey}";
            var generateRequest = new HttpRequestMessage(HttpMethod.Post, generateUrl);
            generateRequest.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // ResponseHeadersRead を指定して、全体が終わる前にストリームを取得開始する
            using (var response = await _client.SendAsync(generateRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        var line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: ")) continue;

                        var dataStr = line.Substring(6).Trim();
                        if (dataStr == "[DONE]") break;

                        // dynamic でパース（Newtonsoft.Jsonを使用）
                        dynamic responseObject = JsonConvert.DeserializeObject(dataStr);

                        if (responseObject.candidates != null && responseObject.candidates.Count > 0)
                        {
                            var parts = responseObject.candidates[0].content.parts;
                            if (parts != null)
                            {
                                foreach (var part in parts)
                                {
                                    string text = part.text;
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        // ここでUIなどの呼び出し元へ通知
                                        onTokenReceived?.Invoke(text.Replace("\n", "\r\n"));
                                    }
                                }
                            }
                        }
                        // --- トークン情報の更新 ---
                        if (responseObject.usageMetadata != null)
                        {
                            this.LastUsage.inputTokens = responseObject.usageMetadata.promptTokenCount;
                            this.LastUsage.outputTokens = responseObject.usageMetadata.candidatesTokenCount;
                            this.LastUsage.totalTokens = responseObject.usageMetadata.totalTokenCount;
                        }
                    }
                }
            }
        }

        public override async Task<string> RequestAsync(string systemPrompt, string userPrompt, string fileUri = null, string fileBase64 = null)
        {
            var requestJson = GetRequestJsonV25(systemPrompt, userPrompt, fileUri, fileBase64);
            return await RequestAsync(requestJson);
        }
    }
}
