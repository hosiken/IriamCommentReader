using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.UI.Xaml.Media;
using static IriamCommentReader.FormMain;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace IriamCommentReader
{
    // Example usage in a Windows Forms application
    public partial class FormMain : Form
    {
        private const string TextString = "{{text}}";
        private bool _useOpenAI = false;
        private readonly OpenAIAPI _apiOpenAI;
        private readonly GeminiAPI _apiGemini;
        private LMBase CurrentAPI => _useOpenAI ? (LMBase)_apiOpenAI : _apiGemini;
        private static Image _shot;
        private int _apiCount = 0;
        static string _prevText = "";
        public List<string> _readText = new List<string>();


        private static GeminiSchema _commentSchema = new GeminiSchema
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
                                    },
                                 Required = new List<string> { "comment" }
                               }
                            } }
                        },
            Required = new List<string> { "comments" }
        };

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
            _apiOpenAI = new OpenAIAPI("APIキーをここに入れる"); // Replace with your actual API key
            _apiGemini = new GeminiAPI("APIキーをここに入れる"); // Replace with your actual API key
            _apiGemini.Schema = _commentSchema;
            _apiGemini.TopK = 40;
            _apiGemini.FrequencyPenalty = 1.0f;
            _apiGemini.PresencePenalty = 1.0f;
            ShowLeftTokens();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Preference.Instance.AutoExecEnable = checkBoxAuto.Checked;
            Preference.Instance.AutoExecInterval = (int)numericInterval.Value;
            Preference.Instance.CaptureRect = new Rect(int.Parse(textBoxLeft.Text), int.Parse(textBoxTop.Text), 
                int.Parse(textBoxWidth.Text), int.Parse(textBoxHeight.Text));
            Preference.Instance.Save();
        }

        public void ShowLeftTokens(string appendText = "")
        {
            labelTokens.Text = $"残りトークン: [{Preference.Instance.Model}] {Preference.Instance.LeftTokens} / [{Preference.Instance.MiniModel}] {Preference.Instance.LeftMiniTokens}{appendText}";
        }

        public static Image CaptureRegion(Rectangle region)
        {
            Bitmap bmp = new Bitmap(region.Width, region.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CopyFromScreen(region.Location, Point.Empty, region.Size);
            }

            // 2. リサイズ判定
            if (Preference.Instance.ImageResizeEnable && Preference.Instance.ImageResizeWidth < region.Width)
            {
                float rate = (float)Preference.Instance.ImageResizeWidth / (float)region.Width;
                int newWidth = Preference.Instance.ImageResizeWidth;
                int newHeight = (int)(region.Height * rate);

                // リサイズ用の空のBitmapを作成
                Bitmap resizedBitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);

                // リサイズ用BitmapのGraphicsを作成
                using (Graphics g = Graphics.FromImage(resizedBitmap))
                {
                    // 画質の設定 (HighQualityBicubic は綺麗ですが負荷は少し高いです。ドット絵ならNearestNeighborで)
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    // 元の画像(bmp)を、新しいサイズに合わせて描画
                    g.DrawImage(bmp, 0, 0, newWidth, newHeight);
                }

                // リサイズ版を作ったので、元の巨大な画像はメモリから開放する
                bmp.Dispose();

                _shot = resizedBitmap;
                return resizedBitmap;
            }

            // リサイズ不要な場合
            _shot = bmp;
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
                labelSimilarity.Text = $"似: {similarity:F2}/{Preference.Instance.Similarity:F2}";
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

            var leftTokens = 0;
            var model = "";
            bool miniModel = false;

            if (_useOpenAI)
            {
                Preference.Instance.CheckAndUpdateTokenDate();

                if (Preference.Instance.LeftTokens > 0 && !checkBoxMini.Checked)
                {
                    model = Preference.Instance.Model;
                    leftTokens = Preference.Instance.LeftTokens;
                }
                else if (Preference.Instance.LeftMiniTokens > 0)
                {
                    model = Preference.Instance.MiniModel;
                    miniModel = true;
                    leftTokens = Preference.Instance.LeftMiniTokens;
                }
                else if (Preference.Instance.LeftTokens > 0 && checkBoxMini.Checked)
                {
                    model = Preference.Instance.Model;
                    leftTokens = Preference.Instance.LeftTokens;
                }
                else
                {
                    textBoxChatLog.AppendText("本日分のトークンを使い切りました. 日本時間で午前9時にリセットされます.\r\n");
                    return;
                }
            }
            else
            {
                model = "gemma-4-31b-it";
                // model = "gemini-3.1-flash-lite-preview";
            }

            buttonQuery.Enabled = false;

            {
                string systemPrompt = Preference.Instance.SystemPrompt;
                string userPrompt = textBoxPrompt.Text ?? "."; // Get user prompt from a textbox
                try
                {
                    _apiOpenAI.APIKey = Preference.Instance.APIKey;
                    // _api.Model = Preference.Instance.Model;
                    CurrentAPI.Model = model;
                    CurrentAPI.Temperature = Preference.Instance.Temperature;
                    CurrentAPI.TopP = Preference.Instance.TopP;

                    textBoxResponse.Clear();

                    string jsonResponse = string.Empty;
                    // if (Preference.Instance.UseBase64)
                    {
                        using (var ms = new MemoryStream())
                        {
                            _shot.Save(ms, ImageFormat.Jpeg);
                            var base64 = Convert.ToBase64String(ms.ToArray());
                            var jsonText = CurrentAPI.GetRequestJson(systemPrompt, userPrompt, fileBase64: base64);
                            textBoxRequest.Text = jsonText;
                            await CurrentAPI.RequestStreamAsync(jsonText, token => {
                                // UIスレッドへの Dispatcher.Invoke などが必要な場合があります
                                textBoxResponse.AppendText(token);
                                jsonResponse += token;
                            });
                            // CurrentAPI.RequestStreamAsync(jsonText, (part) => textBoxResponse.AppendText(part));
                        }
                    }
                    // else
                    // {
                    //  string imageUri = await _geminiAPI.UploadImageAsync(_shot);
                    //  var jsonText = _geminiAPI.GetRequestJson(systemPrompt, userPrompt, fileUri: imageUri, schema: _commentSchema);
                    //  jsonResponse = await _geminiAPI.RequestAsync(jsonText);
                    // }

                    // textBoxResponse.Text = CurrentAPI.LastResponse;
                    textBox2.Text = jsonResponse.Trim('`'); // Display transcribed text in a textbox
                    _apiCount++;
                    labelAPICount.Text = $"API回数:{_apiCount}";

                    if (_useOpenAI)
                    {
                        leftTokens -= CurrentAPI.LastUsage.totalTokens;
                        if (miniModel)
                        {
                            Preference.Instance.LeftMiniTokens = leftTokens;
                        }
                        else
                        {
                            Preference.Instance.LeftTokens = leftTokens;
                        }
                        Preference.Instance.SaveTokens();
                        ShowLeftTokens($" | 使用: {CurrentAPI.Model} ↑{CurrentAPI.LastUsage.inputTokens} + ↓{CurrentAPI.LastUsage.outputTokens} = {CurrentAPI.LastUsage.totalTokens} (残 {(CurrentAPI.LastUsage.outputTokens > 0 ? leftTokens / CurrentAPI.LastUsage.totalTokens : 0)} 回)");
                    }

                    var commentList = JsonConvert.DeserializeObject<CommentList>(jsonResponse);

                    if (commentList != null && commentList.Comments != null && commentList.Comments.Count > 0)
                    {
                        string transcribedText = "";
                        foreach(var comment in commentList.Comments)
                        {
                            transcribedText += $"{comment.Name} | {comment.Message}\r\n";
                        }

                        // _readTextの末尾から最大10件を改行で連結
                        var recentRead = _readText.Skip(Math.Max(0, _readText.Count - 10));
                        var lastStr = string.Join("\r\n", recentRead);

                        // プロンプトには直近最大8件の履歴を使用する
                        var prompt = Preference.Instance.Prompt.Replace(TextString, lastStr);
                        textBoxPrompt.Text = prompt;
                        string speakText = ""; // transcribedText.Replace(" | ", "さん、");
                        string[] chats = transcribedText.Replace("\r\n", "\n").Split(new[] { '\n', '\r' });
                        var beforeTalker = "";
                        foreach (var chat in chats)
                        {
                            if (chat == "") { continue; }

                            if (Preference.Instance.SimilarityChatSkip)
                            {
                                bool skip = false;
                                foreach (var recentChat in recentRead)
                                {
                                    var similarity = TextCompare.Compare(chat, recentChat);
                                    if (similarity >= Preference.Instance.ChatSimilarity)
                                    {
                                        // textBoxChatLog.AppendText($"{chat}\r\n{recentChat}\r\n→{similarity}\r\n\r\n");
                                        skip = true;
                                        break;
                                    }
                                }

                                if (skip)
                                {
                                    continue;
                                }
                            }

                            string[] chatElem = chat.Split(new string[] { " | " }, StringSplitOptions.None);
                            if (chatElem.Length == 1)
                            {
                                speakText += $"{chatElem[0]}\n";
                                beforeTalker = "";
                                _readText.Add(chat);
                                textBoxChatLog.AppendText(chat + "\r\n");
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
                                _readText.Add(chat);
                                textBoxChatLog.AppendText(chat + "\r\n");
                            }
                        }
                        await BouyomiTalk.SpeakAsync(speakText, Preference.Instance.BouyomiURL, Preference.Instance.BouyomiParam);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is System.Net.Http.HttpRequestException && ex.Message.Contains("429"))
                    {
                        textBoxChatLog.AppendText("Gemini APIの呼び出し回数制限(429)に達しました.\r\n");
                        // 必要ならリトライ処理や待機処理を追加
                    }
                    else
                    {
                        textBoxChatLog.AppendText($"Error: {ex.Message}\r\n");
                    }
                    if (CurrentAPI.LastResponse != "")
                    {
                        textBoxResponse.Text = CurrentAPI.LastResponse;
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
            CurrentAPI.APIKey = ((TextBox)sender).Text;
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
            CurrentAPI.Model = ((ComboBox)sender).Text;
        }

        private void buttonPreference_Click(object sender, EventArgs e)
        {
            var result = new FormPreference().ShowDialog(this);
            ShowLeftTokens();
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

        private void MenuImageCopy_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Clipboard.SetImage(pictureBox1.Image);
            }
        }

        private void MenuImageSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG 画像|*.png|JPEG 画像|*.jpg|ビットマップ画像|*.bmp";
                    saveFileDialog.Title = "画像を保存";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 拡張子に応じて画像形式を決定して保存
                        var ext = Path.GetExtension(saveFileDialog.FileName).ToLower();
                        ImageFormat format = ImageFormat.Png;
                        switch (ext)
                        {
                            case ".jpg":
                            case ".jpeg":
                                format = ImageFormat.Jpeg;
                                break;
                            case ".bmp":
                                format = ImageFormat.Bmp;
                                break;
                            case ".png":
                            default:
                                format = ImageFormat.Png;
                                break;
                        }
                        pictureBox1.Image.Save(saveFileDialog.FileName, format);
                    }
                }
            }
        }

        private async void buttonDebugDirect_Click(object sender, EventArgs e)
        {
            buttonDebugDirect.Enabled = false;

            string systemPrompt = Preference.Instance.SystemPrompt;
            string userPrompt = textBoxPrompt.Text ?? "."; // Get user prompt from a textbox
            try
            {
                CurrentAPI.APIKey = Preference.Instance.APIKey;
                CurrentAPI.Model = Preference.Instance.Model;
                CurrentAPI.Temperature = Preference.Instance.Temperature;
                CurrentAPI.TopP = Preference.Instance.TopP;
                var jsonResponse = await CurrentAPI.RequestAsync(userPrompt);
                textBox2.Text = jsonResponse;
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

            buttonDebugDirect.Enabled = true;
        }

        private void MenuImagePaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                var img = Clipboard.GetImage();
                if (img != null)
                {
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = img;
                    _shot = img;
                }
            }
        }

        private void MenuImageLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "画像ファイル|*.png;*.jpg;*.jpeg;*.bmp";
                openFileDialog.Title = "画像を開く";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    _shot = pictureBox1.Image;
                }
            }
        }

        private void contextMenuImage_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MenuImagePaste.Enabled = Clipboard.ContainsImage();
            MenuImageCopy.Enabled = pictureBox1.Image != null;
            MenuImageSave.Enabled = pictureBox1.Image != null;
        }
    }
}