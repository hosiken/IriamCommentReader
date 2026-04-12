using System;
using System.ComponentModel;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace IriamCommentReader
{
    public abstract class LMBase
    {
        protected readonly HttpClient _client;

        public string APIKey { get; set; }
        public string Model { get; set; }
        public float Temperature { get; set; }
        public float TopP { get; set; }
        public int? TopK { get; set; } = 40;
        public float? FrequencyPenalty { get; set; }
        public float? PresencePenalty { get; set; }

        public string LastResponse { get; set; } = string.Empty;
        public UsageInfo LastUsage { get; set; } = new UsageInfo();

        public LMBase(string apiKey)
        {
            APIKey = apiKey;
            _client = new HttpClient();
        }

        public abstract Task<string> UploadImageAsync(Image image);
        public abstract Task<string> UploadImageAsync(string filePath);
        public abstract Task<string> RequestAsync(string requestJson);
        public abstract Task<string> RequestAsync(string systemPrompt, string userPrompt, string fileUri = null, string fileBase64 = null);
        public abstract string GetRequestJson(string systemPrompt, string userPrompt, string fileBase64 = null);
        public virtual async Task RequestStreamAsync(string requestJson, Action<string> onTokenReceived, System.Threading.CancellationToken cancellationToken = default)
        {
            // awaitÇ™Ç»Ç¢åèâåàÇ≥ÇπÇÈ
            throw new NotImplementedException("This method is not implemented for this API.");
        }
    }

    public class UsageInfo
    {
        public int inputTokens { get; set; }
        public int outputTokens { get; set; }
        public int totalTokens { get; set; }
    }
}
