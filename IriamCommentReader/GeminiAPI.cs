﻿using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace IriamCommentReader
{

    public class GeminiAPI
    {
        private string _apiKey;
        private readonly HttpClient _client;

        public string APIKey { set { _apiKey = value; } }

        public GeminiAPI(string apiKey)
        {
            _apiKey = apiKey;
            _client = new HttpClient();
        }

        public async Task<string> UploadImageAsync(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png); // PNG 形式で保存
                memoryStream.Position = 0; // ストリームの位置を先頭に戻す

                var contentLength = memoryStream.Length;
                var contentType = "image/png"; // or get from file extension

                var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_apiKey}";

                var startUploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
                startUploadRequest.Headers.Add("X-Goog-Upload-Command", "start, upload, finalize");
                startUploadRequest.Headers.Add("X-Goog-Upload-Header-Content-Length", contentLength.ToString());
                startUploadRequest.Headers.Add("X-Goog-Upload-Header-Content-Type", contentType);
                startUploadRequest.Content = new StringContent($"{{\"file\": {{\"display_name\": \"image.png\"}}}}", Encoding.UTF8, "application/json");

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

        public async Task<string> UploadImageAsync(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var contentLength = fileStream.Length;
                var contentType = "image/png"; // or get from file extension

                var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_apiKey}";

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

        public async Task<string> TranscribeImageAsync(string fileUri, string systemPrompt, string userPrompt)
        {
            var requestBody = new
            {
                contents = new object[]
                {
                new
                {
                    role = "user",
                    parts = new object[]
                    {
                        new
                        {
                            fileData = new
                            {
                                fileUri = fileUri,
                                mimeType = "image/png" // Or derive from file extension
                            }
                        }
                    }
                },
                new
                {
                    role = "user",
                    parts = new object[]
                    {
                        new { text = userPrompt }
                    }
                }
                },
                systemInstruction = new
                {
                    role = "user",
                    parts = new object[]
                    {
                    new { text = systemPrompt }
                    }
                },
                generationConfig = new
                {
                    temperature = 0,
                    topK = 40,
                    topP = 0,
                    maxOutputTokens = 8192,
                    responseMimeType = "text/plain"
                }
            };

            var generateUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={_apiKey}";
            var generateRequest = new HttpRequestMessage(HttpMethod.Post, generateUrl);
            generateRequest.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var generateResponse = await _client.SendAsync(generateRequest);
            generateResponse.EnsureSuccessStatusCode();


            var generateJsonResponse = await generateResponse.Content.ReadAsStringAsync();
            dynamic generateResponseObject = JsonConvert.DeserializeObject(generateJsonResponse);
            string transcribedText = "";
            foreach (var part in generateResponseObject.candidates[0].content.parts)
            {
                transcribedText += part.text;
            }
            transcribedText = transcribedText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
            return transcribedText;
        }
    }
}
