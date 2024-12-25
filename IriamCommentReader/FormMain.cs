using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace IriamCommentReader
{
    // Example usage in a Windows Forms application
    public partial class FormMain : Form
    {
        private readonly GeminiAPI _geminiAPI;
        private static Image shot;

        public FormMain()
        {
            InitializeComponent();
            // https://aistudio.google.com の左上「Gety API Key」から無料のAPIキーを取得する
            _geminiAPI = new GeminiAPI("APIキーをここに入れる"); // Replace with your actual API key
        }

        public static Image CaptureRegion(Rectangle region)
        {
            // 指定された領域のBitmapを作成
            Bitmap bmp = new Bitmap(region.Width, region.Height, PixelFormat.Format32bppArgb);
            shot = bmp;

            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                // デスクトップの指定された領域をBitmapにコピー
                graphics.CopyFromScreen(region.Location, Point.Empty, region.Size);
            }

            return bmp;
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
            buttonQuery.Enabled = false;
            {
                string systemPrompt = textBoxSystem.Text;
                string userPrompt = textBoxPrompt.Text ?? "."; // Get user prompt from a textbox
                try
                {
                    string imageUri = await _geminiAPI.UploadImageAsync(shot);
                    string transcribedText = await _geminiAPI.TranscribeImageAsync(imageUri, systemPrompt, userPrompt);
                    textBox2.Text = transcribedText; // Display transcribed text in a textbox

                    if (transcribedText.Replace("\r\n", "") != "なし")
                    {
                        textBoxPrompt.Text = $"- 以下の続きのみ出力してください\r\n- 新規の行なしの場合は「なし」と出力\r\n\r\n{transcribedText}";
                        textBoxChatLog.AppendText(transcribedText);
                        BouyomiTalk.SpeakAsync(transcribedText.Replace(" | ", "さん、"));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
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
            await BouyomiTalk.SpeakAsync("うんこ");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            button2_Click(sender, e);
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
    }
}
