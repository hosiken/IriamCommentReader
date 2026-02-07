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

        public LMBase(string apiKey)
        {
            APIKey = apiKey;
            _client = new HttpClient();
        }

        public abstract Task<string> UploadImageAsync(Image image);
        public abstract Task<string> UploadImageAsync(string filePath);
        public abstract Task<string> RequestAsync(string requestJson);
        public abstract Task<string> RequestAsync(string systemPrompt, string userPrompt, string fileUri = null, string fileBase64 = null);
    }
}
