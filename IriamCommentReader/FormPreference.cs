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
        }

        private void FormPreference_Load(object sender, EventArgs e)
        {
            textBoxAPIKey.Text = Preference.Instance.APIKey;
            comboBoxModel.Text = Preference.Instance.Model;
            numericTemperature.Value = (Decimal)Preference.Instance.Temperature;
            numericTopP.Value = (Decimal)Preference.Instance.TopP;
            textBoxSystem.Text = Preference.Instance.SystemPrompt;
            textBoxInitPrompt.Text = Preference.Instance.InitPrompt;
            textBoxPrompt.Text = Preference.Instance.Prompt;
            checkBoxSkipNameAll.Checked = Preference.Instance.SkipNameAll;
            checkBoxSkipName.Checked = Preference.Instance.SkipName;
            comboBoxBouyomiURL.Text = Preference.Instance.BouyomiURL;
            comboBoxBouyomiParam.Text = Preference.Instance.BouyomiParam;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Preference.Instance.APIKey = textBoxAPIKey.Text;
            Preference.Instance.Model = comboBoxModel.Text;
            Preference.Instance.Temperature = (float)numericTemperature.Value;
            Preference.Instance.TopP = (float)numericTopP.Value ;
            Preference.Instance.SystemPrompt = textBoxSystem.Text;
            Preference.Instance.InitPrompt = textBoxInitPrompt.Text;
            Preference.Instance.Prompt = textBoxPrompt.Text;
            Preference.Instance.SkipNameAll = checkBoxSkipNameAll.Checked;
            Preference.Instance.SkipName = checkBoxSkipName.Checked;
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
    }
}
