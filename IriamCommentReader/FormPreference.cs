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
        }

        private void FormPreference_Load(object sender, EventArgs e)
        {
            textBoxGeminiAPIKey.Text = Preference.Instance.APIKeys[APIProviderType.Gemini];
            comboBoxGeminiModel.Text = Preference.Instance.Models[APIProviderType.Gemini];
            comboBoxGeminiModel2.Text = Preference.Instance.MiniModels[APIProviderType.Gemini];
            numericGeminiTemperature.Value = (Decimal)Preference.Instance.Temperatures[APIProviderType.Gemini];
            numericGeminiTopP.Value = (Decimal)Preference.Instance.TopPs[APIProviderType.Gemini];
            textBoxGeminiSystem.Text = Preference.Instance.SystemPrompts[APIProviderType.Gemini];
            textBoxGeminiInitPrompt.Text = Preference.Instance.InitPrompts[APIProviderType.Gemini];
            textBoxGeminiPrompt.Text = Preference.Instance.Prompts[APIProviderType.Gemini];

            textBoxOpenAIAPIKey.Text = Preference.Instance.APIKeys[APIProviderType.OpenAI];
            comboBoxOpenAIModel.Text = Preference.Instance.Models[APIProviderType.OpenAI];
            comboBoxOpenAIMiniModel.Text = Preference.Instance.MiniModels[APIProviderType.OpenAI];
            numericOpenAITemperature.Value = (Decimal)Preference.Instance.Temperatures[APIProviderType.OpenAI];
            numericOpenAITopP.Value = (Decimal)Preference.Instance.TopPs[APIProviderType.OpenAI];
            textBoxOpenAISystem.Text = Preference.Instance.SystemPrompts[APIProviderType.OpenAI];
            textBoxOpenAIInitPrompt.Text = Preference.Instance.InitPrompts[APIProviderType.OpenAI];
            textBoxOpenAIPrompt.Text = Preference.Instance.Prompts[APIProviderType.OpenAI];

            checkBoxSkipNameAll.Checked = Preference.Instance.SkipNameAll;
            checkBoxSkipName.Checked = Preference.Instance.SkipName;
            checkBoxSimilarOnly.Checked = Preference.Instance.SimilarOnly;
            numericSimilarity.Value = (Decimal)Preference.Instance.Similarity;
            checkBoxSimilarityRetryEnable.Checked = Preference.Instance.SimilarityRetryEnable;
            numericSimilarRetryInterval.Value = (Decimal)Preference.Instance.SimilarRetryInterval;
            checkBoxSimilarityChatSkip.Checked = Preference.Instance.SimilarityChatSkip;
            numericChatSimilarity.Value = (Decimal)Preference.Instance.ChatSimilarity;
            checkBoxImageResizeEnable.Checked = Preference.Instance.ImageResizeEnable;
            numericImageResizeWidth.Value = (Decimal)Preference.Instance.ImageResizeWidth;
            comboBoxBouyomiURL.Text = Preference.Instance.BouyomiURL;
            comboBoxBouyomiParam.Text = Preference.Instance.BouyomiParam;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Preference.Instance.APIKeys[APIProviderType.Gemini] = textBoxGeminiAPIKey.Text;
            Preference.Instance.Models[APIProviderType.Gemini] = comboBoxGeminiModel.Text;
            Preference.Instance.MiniModels[APIProviderType.Gemini] = comboBoxGeminiModel2.Text;
            Preference.Instance.Temperatures[APIProviderType.Gemini] = (float)numericGeminiTemperature.Value;
            Preference.Instance.TopPs[APIProviderType.Gemini] = (float)numericGeminiTopP.Value;
            Preference.Instance.SystemPrompts[APIProviderType.Gemini] = textBoxGeminiSystem.Text;
            Preference.Instance.InitPrompts[APIProviderType.Gemini] = textBoxGeminiInitPrompt.Text;
            Preference.Instance.Prompts[APIProviderType.Gemini] = textBoxGeminiPrompt.Text;

            Preference.Instance.SkipNameAll = checkBoxSkipNameAll.Checked;
            Preference.Instance.SkipName = checkBoxSkipName.Checked;
            Preference.Instance.SimilarOnly = checkBoxSimilarOnly.Checked;
            Preference.Instance.Similarity = (float)numericSimilarity.Value;
            Preference.Instance.SimilarityRetryEnable = checkBoxSimilarityRetryEnable.Checked;
            Preference.Instance.SimilarRetryInterval = (int)numericSimilarRetryInterval.Value;
            Preference.Instance.SimilarityChatSkip = checkBoxSimilarityChatSkip.Checked;
            Preference.Instance.ChatSimilarity = (float)numericChatSimilarity.Value;
            Preference.Instance.ImageResizeEnable = checkBoxImageResizeEnable.Checked;
            Preference.Instance.ImageResizeWidth = (int)numericImageResizeWidth.Value;
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
