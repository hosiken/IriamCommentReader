using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IriamCommentReader
{
    public class BouyomiTalk
    {
        public static async Task<int> SpeakAsync(string text = "ゆっくりしていってね", int voice = 0, int volume = -1, int speed = -1, int tone = -1)
        {
            using (var client = new HttpClient())
            {
                var url = "http://localhost:50080/Talk";

                string encodedText = WebUtility.UrlEncode(text);
                string queryString = $"?text={encodedText}&voice={voice}&volume={volume}&speed={speed}&tone={tone}";

                try
                {
                    var response = await client.GetAsync($"{url}{queryString}");
                    return (int)response.StatusCode;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"棒読みちゃんとの通信エラー: {ex.Message}");
                    return -1;
                }
            }
        }
    }
}
