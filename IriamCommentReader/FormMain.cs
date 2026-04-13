using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Image = System.Drawing.Image;

namespace IriamCommentReader
{
    // Example usage in a Windows Forms application
    public partial class FormMain : Form
    {
        private const string TextString = "{{text}}";
        private static Preference Pref => Preference.Instance;
        private bool UseOpenAI => Pref.CurrentProvider == Provider.OpenAI;
        private bool UseGemini => Pref.CurrentProvider == Provider.Gemini;
        private bool UseCompatible => Pref.CurrentProvider == Provider.Compatible;
        private readonly OpenAIAPI _apiOpenAI;
        private readonly GeminiAPI _apiGemini;
        private readonly CompatibleAPI _apiCompatible;
        private LMBase CurrentAPI
        {
            get
            {
                switch (Pref.CurrentProvider)
                {
                    case Provider.OpenAI:
                        return _apiOpenAI;
                    case Provider.Gemini:
                    default:
                        return _apiGemini;
                    case Provider.Compatible:
                        return _apiCompatible;
                }
            }
        }
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
            checkBoxAuto.Checked = Pref.AutoExecEnable;
            numericInterval.Value = Pref.AutoExecInterval;
            textBoxLeft.Text = Pref.CaptureRect.X.ToString();
            textBoxTop.Text = Pref.CaptureRect.Y.ToString();
            textBoxWidth.Text = Pref.CaptureRect.Width.ToString();
            textBoxHeight.Text = Pref.CaptureRect.Height.ToString();
            _apiOpenAI = new OpenAIAPI("");
            _apiGemini = new GeminiAPI("");
            _apiCompatible = new CompatibleAPI("");
            _apiGemini.Schema = _commentSchema;
            _apiGemini.TopK = 40;
            _apiGemini.FrequencyPenalty = 1.0f;
            _apiGemini.PresencePenalty = 1.0f;
            UpdateAPIProviderDisplay();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Pref.AutoExecEnable = checkBoxAuto.Checked;
            Pref.AutoExecInterval = (int)numericInterval.Value;
            Pref.CaptureRect = new Rect(int.Parse(textBoxLeft.Text), int.Parse(textBoxTop.Text), 
                int.Parse(textBoxWidth.Text), int.Parse(textBoxHeight.Text));
            Pref.Save();
        }

        public void ShowLeftTokens(string appendText = "")
        {
            if (Pref.ProviderAllEnded)
            {
                labelTokens.Text = $"API をすべて使い切りました{appendText}";
            }
            else if (UseOpenAI)
            {
                labelTokens.Text = $"残りトークン: [{Pref.Model}] {Pref.LeftTokens} / [{Pref.MiniModel}] {Pref.LeftMiniTokens}{appendText}";
            }
            else
            {
                if (string.IsNullOrEmpty(Pref.MiniModel))
                {
                    labelTokens.Text = $"現在のモデル: {Pref.Model}{appendText}";
                }
                else
                {
                    labelTokens.Text = $"現在のモデル: {Pref.Model} / {Pref.MiniModel}{appendText}";
                }
            }
        }

        public static Image CaptureRegion(Rectangle region)
        {
            Bitmap bmp = new Bitmap(region.Width, region.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CopyFromScreen(region.Location, Point.Empty, region.Size);
            }

            // 2. リサイズ判定
            if (Pref.ImageResizeEnable && Pref.ImageResizeWidth < region.Width)
            {
                float rate = (float)Pref.ImageResizeWidth / (float)region.Width;
                int newWidth = Pref.ImageResizeWidth;
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

            if (UseCompatible)
            {
                _apiCompatible.BaseURL = Pref.APIs[Provider.Compatible].BaseURL;
            }

            if (Pref.SimilarOnly)
            {
                var text = OCRPicture(_shot).Text.Replace(" ", "");
                var similarity = TextCompare.Compare(text, _prevText);
                labelSimilarity.Text = $"似: {similarity:F2}/{Pref.Similarity:F2}";
                if (similarity >= Pref.Similarity)
                {
                    labelSimilarity.ForeColor = Color.Blue;
                    timerSimilarRetry.Interval = Pref.SimilarRetryInterval * 1000;
                    timerSimilarRetry.Enabled = Pref.SimilarityRetryEnable;
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
            bool mainModelEnded = UseOpenAI
                ? Pref.LeftTokens <= 0
                : Pref.GemimiMainModelReachedLimit;
            bool subModelEnded = UseOpenAI
                ? Pref.LeftMiniTokens <= 0
                : false;
            bool miniModel = false;

            Pref.CheckAndUpdateTokenDate();

            if (!mainModelEnded && !checkBoxMini.Checked)
            {
                miniModel = false;
            }
            else if (!subModelEnded)
            {
                miniModel = true;
            }
            else if (!mainModelEnded)
            {
                miniModel = false;
            }
            else
            {
                if (UseOpenAI)
                {
                    textBoxChatLog.AppendText("OpenAI の本日分のトークンを使い切りました. 日本時間で午前9時にリセットされます.\r\n");
                    Pref.ProviderEnded = true;
                    if (!Pref.ProviderAllEnded && !string.IsNullOrEmpty(Pref.APIs[Provider.Gemini].MiniModel))
                    {
                        textBoxChatLog.AppendText("使用する AI を Gemini に切り替えます.\r\n");
                        Pref.CurrentProvider = Provider.Gemini;
                        UpdateAPIProviderDisplay();
                    }
                }
                return;
            }

            if (string.IsNullOrEmpty(Pref.MiniModel))
            {
                miniModel = false;
            }

            model = miniModel ? Pref.MiniModel : Pref.Model;
            leftTokens = miniModel ? Pref.LeftMiniTokens : Pref.LeftTokens;

            if (!UseCompatible &&  string.IsNullOrEmpty(Pref.APIKey))
            {
                _prevText = "";
                textBoxChatLog.AppendText($"{Pref.CurrentProvider.ToString()} : API キーが設定されていません.\r\n");
                checkBoxAuto.Checked = false;
                timerQuery.Enabled = false;
                return;
            }

            if (Pref.ProviderAllEnded)
            {
                textBoxChatLog.AppendText("本日の API はすべて使い切りました. 今日はもう読み上げられません.\r\n");
                checkBoxAuto.Checked = false;
                timerQuery.Enabled = false;
                return;
            }

            buttonQuery.Enabled = false;
            textBoxResponse.Clear();

            {
                string systemPrompt = Pref.SystemPrompt;
                string userPrompt = textBoxPrompt.Text ?? "."; // Get user prompt from a textbox
                try
                {
                    CurrentAPI.APIKey = Pref.APIKey;
                    // _api.Model = Pref.Model;
                    CurrentAPI.Model = model;
                    CurrentAPI.Temperature = Pref.Temperature;
                    CurrentAPI.TopP = Pref.TopP;

                    string jsonResponse = string.Empty;
                    // if (Pref.UseBase64)
                    {
                        using (var ms = new MemoryStream())
                        {
                            _shot.Save(ms, ImageFormat.Jpeg);
                            var base64 = Convert.ToBase64String(ms.ToArray());
                            var jsonText = CurrentAPI.GetRequestJson(systemPrompt, userPrompt, fileBase64: base64);
                            textBoxRequest.Text = jsonText;
                            if (Pref.IsStreaming)
                            {
                                await CurrentAPI.RequestStreamAsync(jsonText, token => {
                                    // UIスレッドへの Dispatcher.Invoke などが必要な場合があります
                                    textBoxResponse.AppendText(token);
                                    jsonResponse += token;
                                });
                            }
                            else
                            {
                                jsonResponse = await CurrentAPI.RequestAsync(jsonText);
                            }
                        }
                    }
                    // else
                    // {
                    //  string imageUri = await _geminiAPI.UploadImageAsync(_shot);
                    //  var jsonText = _geminiAPI.GetRequestJson(systemPrompt, userPrompt, fileUri: imageUri, schema: _commentSchema);
                    //  jsonResponse = await _geminiAPI.RequestAsync(jsonText);
                    // }

                    textBoxResponse.Text = CurrentAPI.LastResponse;
                    textBox2.Text = jsonResponse; // Display transcribed text in a textbox
                    _apiCount++;
                    labelAPICount.Text = $"API回数:{_apiCount}";

                    if (UseOpenAI)
                    {
                        leftTokens -= CurrentAPI.LastUsage.totalTokens;
                        if (miniModel)
                        {
                            Pref.LeftMiniTokens = leftTokens;
                        }
                        else
                        {
                            Pref.LeftTokens = leftTokens;
                        }
                        Pref.SaveTokens();
                        ShowLeftTokens($" | 使用: {CurrentAPI.Model} ↑{CurrentAPI.LastUsage.inputTokens} + ↓{CurrentAPI.LastUsage.outputTokens} = {CurrentAPI.LastUsage.totalTokens} (残 {(CurrentAPI.LastUsage.outputTokens > 0 ? leftTokens / CurrentAPI.LastUsage.totalTokens : 0)} 回)");
                    }
                    else
                    {
                        ShowLeftTokens($" | 使用: {CurrentAPI.Model} ↑{CurrentAPI.LastUsage.inputTokens} + ↓{CurrentAPI.LastUsage.outputTokens} = {CurrentAPI.LastUsage.totalTokens}");
                    }

                    var commentList = JsonConvert.DeserializeObject<CommentList>(jsonResponse);

                    if (commentList != null && commentList.Comments != null && commentList.Comments.Count > 0)
                    {
                        string transcribedText = "";
                        foreach(var comment in commentList.Comments)
                        {
                            transcribedText += $"{comment.Name} | {comment.Message}\r\n";
                        }

                        var recentRead = _readText.Skip(Math.Max(0, _readText.Count - 10));
                        string speakText = ""; // transcribedText.Replace(" | ", "さん、");
                        string[] chats = transcribedText.Replace("\r\n", "\n").Split(new[] { '\n', '\r' });
                        var beforeTalker = "";
                        foreach (var chat in chats)
                        {
                            if (chat == "") { continue; }

                            if (Pref.SimilarityChatSkip)
                            {
                                bool skip = false;
                                foreach (var recentChat in recentRead)
                                {
                                    var similarity = TextCompare.Compare(chat, recentChat);
                                    if (similarity >= Pref.ChatSimilarity)
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
                                if ((beforeTalker == chatElem[0] && Pref.SkipName) || Pref.SkipNameAll)
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

                        // _readTextの末尾から最大10件を改行で連結
                        recentRead = _readText.Skip(Math.Max(0, _readText.Count - 10));
                        var lastStr = string.Join("\r\n", recentRead);

                        // プロンプトには直近最大8件の履歴を使用する
                        var prompt = Pref.Prompt.Replace(TextString, lastStr);
                        textBoxPrompt.Text = prompt;

                        await BouyomiTalk.SpeakAsync(speakText, Pref.BouyomiURL, Pref.BouyomiParam);
                    }
                }
                catch (Exception ex)
                {
                    _prevText = "";

                    if (ex is System.Net.Http.HttpRequestException && ex.Message.Contains("429"))
                    {
                        textBoxChatLog.AppendText("Gemini APIの呼び出し回数制限(429)に達しました.\r\n");
                        if (UseGemini)
                        {
                            if (!Pref.GemimiMainModelReachedLimit)
                            {
                                Pref.GemimiMainModelReachedLimit = true;
                                textBoxChatLog.AppendText("メインモデルの制限に達したため、次のリクエストから予備モデルを使用します.\r\n");
                                UpdateAPIProviderDisplay();
                            }
                        }
                        else if (UseOpenAI && !string.IsNullOrEmpty(Pref.APIs[Provider.OpenAI].MiniModel))
                        {
                            Pref.ProviderEnded = true;
                            if (!Pref.ProviderAllEnded && !string.IsNullOrEmpty(Pref.APIs[Provider.OpenAI].MiniModel))
                            {
                                textBoxChatLog.AppendText("Geminiの全モデルを使い切ったため、OpenAI に切り替えます。.\r\n");
                                Pref.CurrentProvider = Provider.OpenAI;
                                UpdateAPIProviderDisplay();
                            }
                        }
                    }
                    else
                    {
                        textBoxChatLog.AppendText($"Error: {ex.Message}\r\n");
                    }
                    if (!string.IsNullOrEmpty(CurrentAPI.LastResponse))
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
            await BouyomiTalk.SpeakAsync("読み上げテスト", Pref.BouyomiURL, Pref.BouyomiParam);
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
            textBoxPrompt.Text = Pref.InitPrompt;
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            textBoxChatLog.Clear();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxPrompt.Text = Pref.InitPrompt;
            _prevText = "";
            _readText.Clear();
        }

        private void labelSimilarity_DoubleClick(object sender, EventArgs e)
        {
            if (Pref.SimilarOnly)
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

            string systemPrompt = Pref.SystemPrompt;
            string userPrompt = textBoxPrompt.Text ?? "."; // Get user prompt from a textbox
            try
            {
                CurrentAPI.APIKey = Pref.APIKey;
                CurrentAPI.Model = Pref.Model;
                CurrentAPI.Temperature = Pref.Temperature;
                CurrentAPI.TopP = Pref.TopP;
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

        private void toolStripMenuItemToggleAI_Click(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem)sender;
            var tag = menu.Tag.ToString();
            Provider type = (Provider)(int.Parse(tag));
            Pref.CurrentProvider = type;
            UpdateAPIProviderDisplay();
        }

        private void UpdateAPIProviderDisplay()
        {
            var providerText = $"{(UseCompatible ? "互換API" : Pref.CurrentProvider.ToString())}";
            toolStripDropDownButtonAPIProvider.Text = providerText;
            ShowLeftTokens();
        }

        private void toolStripDropDownButtonAPIProvider_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItemGemini.Checked = UseGemini;
            toolStripMenuItemOpenAI.Checked = UseOpenAI;
            toolStripMenuItemCompatible.Checked = UseCompatible;
            toolStripMenuItemCompatible.Visible = !string.IsNullOrEmpty(Pref.APIs[Provider.Compatible].BaseURL);
        }
    }
}