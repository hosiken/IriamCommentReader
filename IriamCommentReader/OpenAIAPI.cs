using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace IriamCommentReader
{
    public class OpenAIAPI : LMBase
    {
        public OpenAIAPI(string apiKey) : base(apiKey)
        {
        }

        public override Task<string> UploadImageAsync(Image image)
        {
            throw new NotImplementedException();
        }

        public override Task<string> UploadImageAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public override Task<string> RequestAsync(string requestJson)
        {
            throw new NotImplementedException();
        }

        public override Task<string> RequestAsync(string systemPrompt, string userPrompt, string fileUri = null, string fileBase64 = null)
        {
            throw new NotImplementedException();
        }
    }
}
