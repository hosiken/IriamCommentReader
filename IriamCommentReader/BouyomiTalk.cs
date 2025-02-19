using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IriamCommentReader
{
    public class BouyomiTalk
    {
        public static async Task<int> SpeakAsync(string text = "ゆっくりしていってね", string url = "http://localhost:50080/Talk", string query = "?text={{text}}")
        {
            // URLエンコードされると困る文字を全角に変換
            text = text.Replace(" ", "　").Replace("%", "％").Replace("/", "／");

            using (var client = new HttpClient())
            {
                string encodedText = WebUtility.UrlEncode(text);
                string queryString = query.Replace("{{text}}", encodedText);

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
