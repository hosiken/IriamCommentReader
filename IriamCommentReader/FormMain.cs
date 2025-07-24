using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IriamCommentReader
{
    // Example usage in a Windows Forms application
    public partial class FormMain : Form
    {
        private const string TextString = "{{text}}";
        private readonly GeminiAPI _geminiAPI;
        private static Image _shot;
        private int _apiCount = 0;
        static string _prevText = "";

        public class Comment
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("comment")]
            public string Message { get; set; }
        }

        public class CommentList
        {
            [JsonProperty("comments")]
            public List<Comment> Comments { get; set; }
        }

        public FormMain()
        {
            InitializeComponent();
            Preference.Instance = Preference.Load();
            checkBoxAuto.Checked = Preference.Instance.AutoExecEnable;
            numericInterval.Value = Preference.Instance.AutoExecInterval;
            textBoxLeft.Text = Preference.Instance.CaptureRect.X.ToString();
            textBoxTop.Text = Preference.Instance.CaptureRect.Y.ToString();
            textBoxWidth.Text = Preference.Instance.CaptureRect.Width.ToString();
            textBoxHeight.Text = Preference.Instance.CaptureRect.Height.ToString();
            _geminiAPI = new GeminiAPI("APIキーをここに入れる"); // Replace with your actual API key
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Preference.Instance.AutoExecEnable = checkBoxAuto.Checked;
            Preference.Instance.AutoExecInterval = (int)numericInterval.Value;
            Preference.Instance.CaptureRect = new Rect(int.Parse(textBoxLeft.Text), int.Parse(textBoxTop.Text), 
                int.Parse(textBoxWidth.Text), int.Parse(textBoxHeight.Text));
            Preference.Instance.Save();
        }

        public static Image CaptureRegion(Rectangle region)
        {
            // 指定された領域のBitmapを作成
            Bitmap bmp = new Bitmap(region.Width, region.Height, PixelFormat.Format32bppArgb);
            _shot = bmp;

            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                // デスクトップの指定された領域をBitmapにコピー
                graphics.CopyFromScreen(region.Location, Point.Empty, region.Size);
            }

            return bmp;
        }

        private static OcrResult OCRPicture(Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var stream = ms.AsRandomAccessStream();
            var ocrResult = Task.Run(async () =>
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                var bitmap = await decoder.GetSoftwareBitmapAsync();
                OcrEngine ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
                // OCR実行
                return await ocrEngine.RecognizeAsync(bitmap);
            }).GetAwaiter().GetResult();
            return ocrResult;
        }

        public Bitmap BinarizeBitmap(Bitmap original, byte threshold = 128)
        {
            Bitmap binarized = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color color = original.GetPixel(x, y);
                    // 輝度の計算（重み付け平均）
                    int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    // 閾値により白か黒に設定
                    Color newColor = (gray >= threshold) ? Color.White : Color.Black;
                    binarized.SetPixel(x, y, newColor);
                }
            }

            return binarized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pic = CaptureRegion(new Rectangle(int.Parse(textBoxLeft.Text), int.Parse(textBoxTop.Text), int.Parse(textBoxWidth.Text), int.Parse(textBoxHeight.Text)));
            pictureBox1.Image = pic;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private async void button2_Click(object sender, EventArgs e)
        {
            if (_shot == null)
            {
                return;
            }

            if (Preference.Instance.SimilarOnly)
            {
                var text = OCRPicture(_shot).Text.Replace(" ", "");
                var similarity = TextCompare.Compare(text, _prevText);
                labelSimilarity.Text = $"類似: {similarity:F2}/{Preference.Instance.Similarity:F2}";
                if (similarity >= Preference.Instance.Similarity)
                {
                    labelSimilarity.ForeColor = Color.Blue;
                    timerSimilarRetry.Interval = Preference.Instance.SimilarRetryInterval * 1000;
                    timerSimilarRetry.Enabled = Preference.Instance.SimilarityRetryEnable;
                    return;
                }
                _prevText = text;
                labelSimilarity.ForeColor = SystemColors.ControlText;
            }
            else
            {
                labelSimilarity.Text = "";
            }

            timerSimilarRetry.Enabled = false;

            timerQuery.Enabled = false;
            timerQuery.Enabled = checkBoxAuto.Checked;

            buttonQuery.Enabled = false;
            {
                string systemPrompt = Preference.Instance.SystemPrompt;
                string userPrompt = textBoxPrompt.Text ?? "."; // Get user prompt from a textbox
                try
                {
                    _geminiAPI.APIKey = Preference.Instance.APIKey;
                    _geminiAPI.Model = Preference.Instance.Model;
                    _geminiAPI.Temperature = Preference.Instance.Temperature;
                    _geminiAPI.TopP = Preference.Instance.TopP;

                    var commentSchema = new GeminiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, GeminiSchema>
                        {
                            { "comments", new GeminiSchema {
                                Type = "array",
                                Description = "コメントのリスト",
                                Items = new GeminiSchema {
                                    Type = "object",
                                    Description = "コメント",
                                    Properties = new Dictionary<string, GeminiSchema> {
                                        { "name", new GeminiSchema { Type = "string", Description = "ユーザー名" } },
                                        { "comment", new GeminiSchema { Type = "string", Description = "コメント" } }
                                    }
                                }
                            } }
                        },
                        Required = new List<string> { "comments" }
                    };

                    string jsonResponse;
                    if (Preference.Instance.UseBase64)
                    {
                        using (var ms = new MemoryStream())
                        {
                            _shot.Save(ms, ImageFormat.Jpeg);
                            var base64 = Convert.ToBase64String(ms.ToArray());
                            jsonResponse = await _geminiAPI.TranscribeImageAsync(_geminiAPI.GetRequestJson(systemPrompt, userPrompt, fileBase64: base64, schema: commentSchema));
                        }
                    }
                    else
                    {
                        string imageUri = await _geminiAPI.UploadImageAsync(_shot);
                        jsonResponse = await _geminiAPI.TranscribeImageAsync(_geminiAPI.GetRequestJson(systemPrompt, userPrompt, fileUri: imageUri, schema: commentSchema));
                    }

                    textBox2.Text = jsonResponse; // Display transcribed text in a textbox
                    _apiCount++;
                    labelAPICount.Text = $"API回数:{_apiCount}";

                    var commentList = JsonConvert.DeserializeObject<CommentList>(jsonResponse);

                    if (commentList != null && commentList.Comments != null && commentList.Comments.Count > 0)
                    {
                        string transcribedText = "";
                        foreach(var comment in commentList.Comments)
                        {
                            transcribedText += $"{comment.Name} | {comment.Message}\r\n";
                        }

                        var prompt = Preference.Instance.Prompt.Replace(TextString, transcribedText);
                        textBoxPrompt.Text = prompt;
                        if (transcribedText != "")
                        {
                            textBoxChatLog.AppendText(transcribedText + (transcribedText[transcribedText.Length - 1] == '\n' ? "" : "\r\n"));
                        }
                        string speakText = ""; // transcribedText.Replace(" | ", "さん、");
                        string[] chats = transcribedText.Replace("\r\n", "\n").Split(new[] { '\n', '\r' });
                        var beforeTalker = "";
                        foreach (var chat in chats)
                        {
                            string[] chatElem = chat.Split(new string[] { " | " }, StringSplitOptions.None);
                            if (chatElem.Length == 1)
                            {
                                speakText += $"{chatElem[0]}\n";
                                beforeTalker = "";
                            }
                            else if (chatElem.Length > 1)
                            {
                                if ((beforeTalker == chatElem[0] && Preference.Instance.SkipName) || Preference.Instance.SkipNameAll)
                                {
                                    speakText += $"{chatElem[1]}\n";
                                }
                                else
                                {
                                    beforeTalker = chatElem[0];
                                    speakText += $"{chatElem[0]}さん、{chatElem[1]}\n";
                                }
                            }
                        }
                        await BouyomiTalk.SpeakAsync(speakText, Preference.Instance.BouyomiURL, Preference.Instance.BouyomiParam);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is System.Net.Http.HttpRequestException && ex.Message.Contains("429"))
                    {
                        textBoxChatLog.AppendText("APIの呼び出し回数制限(429)に達しました.\r\n");
                        // 必要ならリトライ処理や待機処理を追加
                    }
                    else
                    {
                        textBoxChatLog.AppendText($"Error: {ex.Message}\r\n");
                    }
                }
            }
            buttonQuery.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rectangle selectedArea = AreaSelector.GetSelectedArea();

            if (!selectedArea.IsEmpty)
            {
                textBoxLeft.Text = selectedArea.Left.ToString();
                textBoxTop.Text = selectedArea.Top.ToString();
                textBoxWidth.Text = selectedArea.Width.ToString();
                textBoxHeight.Text = selectedArea.Height.ToString();
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await BouyomiTalk.SpeakAsync("読み上げテスト", Preference.Instance.BouyomiURL, Preference.Instance.BouyomiParam);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (buttonQuery.Enabled && !timerSimilarRetry.Enabled)
            {
                button1_Click(sender, e);
                button2_Click(sender, e);
            }
        }

        private void timerSimilarRetry_Tick(object sender, EventArgs e)
        {
            if (buttonQuery.Enabled && timerQuery.Enabled)
            {
                button1_Click(sender, e);
                button2_Click(sender, e);
            }
        }

        private void textBoxAPIKey_TextChanged(object sender, EventArgs e)
        {
            _geminiAPI.APIKey = ((TextBox)sender).Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timerQuery.Enabled = ((CheckBox)sender).Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int interval;
            int.TryParse(((TextBox)sender).Text, out interval);
            if (interval < 10) { interval = 10; }

            timerQuery.Interval = interval * 1000;
        }

        private void comboBoxModel_TextChanged(object sender, EventArgs e)
        {
            _geminiAPI.Model = ((ComboBox)sender).Text;
        }

        private void buttonPreference_Click(object sender, EventArgs e)
        {
            var result = new FormPreference().ShowDialog(this);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timerQuery.Interval = (int)(((NumericUpDown)sender).Value) * 1000;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            textBoxPrompt.Text = Preference.Instance.InitPrompt;
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            textBoxChatLog.Clear();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxPrompt.Text = Preference.Instance.InitPrompt;
        }

        private void labelSimilarity_DoubleClick(object sender, EventArgs e)
        {
            if (Preference.Instance.SimilarOnly)
            {
                _prevText = "";
                labelSimilarity.Text = "類似リセット済";
                labelSimilarity.ForeColor = Color.Red;
            }
        }
    }
}