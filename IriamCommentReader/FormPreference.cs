using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IriamCommentReader
{
    public partial class FormPreference : Form
    {
        public FormPreference()
        {
            InitializeComponent();

            comboBoxGeminiModel.Items.Clear();
            comboBoxGeminiModel.BeginUpdate();
            comboBoxGeminiModel.Items.AddRange(Preference.GeminiModelList?.ToArray());
            comboBoxGeminiModel.EndUpdate();

            comboBoxGeminiModel2.Items.Clear();
            comboBoxGeminiModel2.BeginUpdate();
            comboBoxGeminiModel2.Items.AddRange(Preference.GeminiModelList?.ToArray());
            comboBoxGeminiModel2.EndUpdate();

            comboBoxOpenAIModel.Items.Clear();
            comboBoxOpenAIModel.BeginUpdate();
            comboBoxOpenAIModel.Items.AddRange(Preference.OpenAIModelWhiteList?.ToArray());
            comboBoxOpenAIModel.EndUpdate();

            comboBoxOpenAIMiniModel.Items.Clear();
            comboBoxOpenAIMiniModel.BeginUpdate();
            comboBoxOpenAIMiniModel.Items.AddRange(Preference.OpenAIMiniModelWhiteList?.ToArray());
            comboBoxOpenAIMiniModel.EndUpdate();

            comboBoxCompatibleModel.Items.Clear();
            comboBoxCompatibleModel.BeginUpdate();
            comboBoxCompatibleModel.Items.AddRange(Preference.OpenModelList?.ToArray());
            comboBoxCompatibleModel.EndUpdate();
        }

        private void FormPreference_Load(object sender, EventArgs e)
        {
            textBoxGeminiAPIKey.Text = Preference.Instance.APIs[Provider.Gemini].APIKey;
            comboBoxGeminiModel.Text = Preference.Instance.APIs[Provider.Gemini].Model;
            comboBoxGeminiModel2.Text = Preference.Instance.APIs[Provider.Gemini].MiniModel;
            numericGeminiTemperature.Value = (Decimal)Preference.Instance.APIs[Provider.Gemini].Temperature;
            numericGeminiTopP.Value = (Decimal)Preference.Instance.APIs[Provider.Gemini].TopP;
            textBoxGeminiSystem.Text = Preference.Instance.APIs[Provider.Gemini].SystemPrompt;
            textBoxGeminiInitPrompt.Text = Preference.Instance.APIs[Provider.Gemini].InitPrompt;
            textBoxGeminiPrompt.Text = Preference.Instance.APIs[Provider.Gemini].Prompt;
 
            textBoxOpenAIAPIKey.Text = Preference.Instance.APIs[Provider.OpenAI].APIKey;
            comboBoxOpenAIModel.Text = Preference.Instance.APIs[Provider.OpenAI].Model;
            comboBoxOpenAIMiniModel.Text = Preference.Instance.APIs[Provider.OpenAI].MiniModel;
            numericOpenAITemperature.Value = (Decimal)Preference.Instance.APIs[Provider.OpenAI].Temperature;
            numericOpenAITopP.Value = (Decimal)Preference.Instance.APIs[Provider.OpenAI].TopP;
            textBoxOpenAISystem.Text = Preference.Instance.APIs[Provider.OpenAI].SystemPrompt;
            textBoxOpenAIInitPrompt.Text = Preference.Instance.APIs[Provider.OpenAI].InitPrompt;
            textBoxOpenAIPrompt.Text = Preference.Instance.APIs[Provider.OpenAI].Prompt;

            textBoxCompatibleBaseURL.Text = Preference.Instance.APIs[Provider.Compatible].BaseURL;
            textBoxCompatibleAPIKey.Text = Preference.Instance.APIs[Provider.Compatible].APIKey;
            comboBoxCompatibleModel.Text = Preference.Instance.APIs[Provider.Compatible].Model;
            // comboBoxCompatibleMiniModel.Text = Preference.Instance.APIs[Provider.Compatible].MiniModel;
            numericCompatibleTemperature.Value = (Decimal)Preference.Instance.APIs[Provider.Compatible].Temperature;
            numericCompatibleTopP.Value = (Decimal)Preference.Instance.APIs[Provider.Compatible].TopP;
            textBoxCompatibleSystem.Text = Preference.Instance.APIs[Provider.Compatible].SystemPrompt;
            textBoxCompatibleInitPrompt.Text = Preference.Instance.APIs[Provider.Compatible].InitPrompt;
            textBoxCompatiblePrompt.Text = Preference.Instance.APIs[Provider.Compatible].Prompt;

            checkBoxSkipNameAll.Checked = Preference.Instance.SkipNameAll;
            checkBoxSkipName.Checked = Preference.Instance.SkipName;
            checkBoxSimilarOnly.Checked = Preference.Instance.SimilarOnly;
            numericChatHistoryCount.Value = (Decimal)Preference.Instance.ChatHistoryCount;
            numericSimilarity.Value = (Decimal)Preference.Instance.Similarity;
            checkBoxSimilarityRetryEnable.Checked = Preference.Instance.SimilarityRetryEnable;
            numericSimilarRetryInterval.Value = (Decimal)Preference.Instance.SimilarRetryInterval;
            checkBoxSimilarityChatSkip.Checked = Preference.Instance.SimilarityChatSkip;
            numericChatSimilarity.Value = (Decimal)Preference.Instance.ChatSimilarity;
            checkBoxImageResizeEnable.Checked = Preference.Instance.ImageResizeEnable;
            numericImageResizeWidth.Value = (Decimal)Preference.Instance.ImageResizeWidth;
            checkBoxStream.Checked = Preference.Instance.IsStreaming;
            comboBoxBouyomiURL.Text = Preference.Instance.BouyomiURL;
            comboBoxBouyomiParam.Text = Preference.Instance.BouyomiParam;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Preference.Instance.APIs[Provider.Gemini].APIKey = textBoxGeminiAPIKey.Text;
            Preference.Instance.APIs[Provider.Gemini].Model = comboBoxGeminiModel.Text;
            Preference.Instance.APIs[Provider.Gemini].MiniModel = comboBoxGeminiModel2.Text;
            Preference.Instance.APIs[Provider.Gemini].Temperature = (float)numericGeminiTemperature.Value;
            Preference.Instance.APIs[Provider.Gemini].TopP = (float)numericGeminiTopP.Value;
            Preference.Instance.APIs[Provider.Gemini].SystemPrompt = textBoxGeminiSystem.Text;
            Preference.Instance.APIs[Provider.Gemini].InitPrompt = textBoxGeminiInitPrompt.Text;
            Preference.Instance.APIs[Provider.Gemini].Prompt = textBoxGeminiPrompt.Text;

            Preference.Instance.APIs[Provider.OpenAI].APIKey = textBoxOpenAIAPIKey.Text;
            Preference.Instance.APIs[Provider.OpenAI].Model = comboBoxOpenAIModel.Text;
            Preference.Instance.APIs[Provider.OpenAI].MiniModel = comboBoxOpenAIMiniModel.Text;
            Preference.Instance.APIs[Provider.OpenAI].Temperature = (float)numericOpenAITemperature.Value;
            Preference.Instance.APIs[Provider.OpenAI].TopP = (float)numericOpenAITopP.Value;
            Preference.Instance.APIs[Provider.OpenAI].SystemPrompt = textBoxOpenAISystem.Text;
            Preference.Instance.APIs[Provider.OpenAI].InitPrompt = textBoxOpenAIInitPrompt.Text;
            Preference.Instance.APIs[Provider.OpenAI].Prompt = textBoxOpenAIPrompt.Text;

            Preference.Instance.APIs[Provider.Compatible].BaseURL = textBoxCompatibleBaseURL.Text;
            Preference.Instance.APIs[Provider.Compatible].APIKey = textBoxCompatibleAPIKey.Text;
            Preference.Instance.APIs[Provider.Compatible].Model = comboBoxCompatibleModel.Text;
            // Preference.Instance.APIs[Provider.Compatible].MiniModel = comboBoxCompatibleMiniModel.Text;
            Preference.Instance.APIs[Provider.Compatible].Temperature = (float)numericCompatibleTemperature.Value;
            Preference.Instance.APIs[Provider.Compatible].TopP = (float)numericCompatibleTopP.Value;
            Preference.Instance.APIs[Provider.Compatible].SystemPrompt = textBoxCompatibleSystem.Text;
            Preference.Instance.APIs[Provider.Compatible].InitPrompt = textBoxCompatibleInitPrompt.Text;
            Preference.Instance.APIs[Provider.Compatible].Prompt = textBoxCompatiblePrompt.Text;

            Preference.Instance.SkipNameAll = checkBoxSkipNameAll.Checked;
            Preference.Instance.SkipName = checkBoxSkipName.Checked;
            Preference.Instance.SimilarityRetryEnable = checkBoxSimilarityRetryEnable.Checked;
            Preference.Instance.ChatHistoryCount = (int)numericChatHistoryCount.Value;
            Preference.Instance.Similarity = (float)numericSimilarity.Value;
            Preference.Instance.SimilarOnly = checkBoxSimilarOnly.Checked;
            Preference.Instance.SimilarRetryInterval = (int)numericSimilarRetryInterval.Value;
            Preference.Instance.SimilarityChatSkip = checkBoxSimilarityChatSkip.Checked;
            Preference.Instance.ChatSimilarity = (float)numericChatSimilarity.Value;
            Preference.Instance.ImageResizeEnable = checkBoxImageResizeEnable.Checked;
            Preference.Instance.ImageResizeWidth = (int)numericImageResizeWidth.Value;
            Preference.Instance.IsStreaming = checkBoxStream.Checked;
            Preference.Instance.BouyomiURL = comboBoxBouyomiURL.Text;
            Preference.Instance.BouyomiParam = comboBoxBouyomiParam.Text;
            Preference.Instance.Save();
        }

        private void buttonDefualt_Click(object sender, EventArgs e)
        {
            Preference.Instance.ResetToDefault();
            FormPreference_Load(sender, e);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxMiniModel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
