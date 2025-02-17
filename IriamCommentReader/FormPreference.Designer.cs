namespace IriamCommentReader
{
    partial class FormPreference
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxModel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAPIKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSystem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxInitPrompt = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDefualt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numericTemperature = new System.Windows.Forms.NumericUpDown();
            this.numericTopP = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPrompt = new System.Windows.Forms.TextBox();
            this.checkBoxSkipNameAll = new System.Windows.Forms.CheckBox();
            this.checkBoxSkipName = new System.Windows.Forms.CheckBox();
            this.comboBoxBouyomiURL = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxBouyomiParam = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxSimilarOnly = new System.Windows.Forms.CheckBox();
            this.numericSimilarity = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericSimilarRetryInterval = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTopP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSimilarity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSimilarRetryInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxModel
            // 
            this.comboBoxModel.FormattingEnabled = true;
            this.comboBoxModel.Items.AddRange(new object[] {
            "gemini-2.0-flash",
            "gemini-2.0-flash-exp",
            "gemini-2.0-flash-lite-preview-02-05"});
            this.comboBoxModel.Location = new System.Drawing.Point(88, 36);
            this.comboBoxModel.Name = "comboBoxModel";
            this.comboBoxModel.Size = new System.Drawing.Size(172, 20);
            this.comboBoxModel.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "モデル";
            // 
            // textBoxAPIKey
            // 
            this.textBoxAPIKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAPIKey.Location = new System.Drawing.Point(88, 12);
            this.textBoxAPIKey.Name = "textBoxAPIKey";
            this.textBoxAPIKey.Size = new System.Drawing.Size(378, 19);
            this.textBoxAPIKey.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "APIキー";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "システムプロンプト";
            // 
            // textBoxSystem
            // 
            this.textBoxSystem.AcceptsReturn = true;
            this.textBoxSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSystem.Location = new System.Drawing.Point(15, 122);
            this.textBoxSystem.Multiline = true;
            this.textBoxSystem.Name = "textBoxSystem";
            this.textBoxSystem.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSystem.Size = new System.Drawing.Size(451, 136);
            this.textBoxSystem.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "初期プロンプト";
            // 
            // textBoxInitPrompt
            // 
            this.textBoxInitPrompt.AcceptsReturn = true;
            this.textBoxInitPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInitPrompt.Location = new System.Drawing.Point(15, 276);
            this.textBoxInitPrompt.Multiline = true;
            this.textBoxInitPrompt.Name = "textBoxInitPrompt";
            this.textBoxInitPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInitPrompt.Size = new System.Drawing.Size(451, 73);
            this.textBoxInitPrompt.TabIndex = 11;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(307, 575);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 25;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(388, 575);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 26;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonDefualt
            // 
            this.buttonDefualt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDefualt.Location = new System.Drawing.Point(12, 575);
            this.buttonDefualt.Name = "buttonDefualt";
            this.buttonDefualt.Size = new System.Drawing.Size(75, 23);
            this.buttonDefualt.TabIndex = 24;
            this.buttonDefualt.Text = "初期設定";
            this.buttonDefualt.UseVisualStyleBackColor = true;
            this.buttonDefualt.Click += new System.EventHandler(this.buttonDefualt_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Temperature";
            // 
            // numericTemperature
            // 
            this.numericTemperature.DecimalPlaces = 2;
            this.numericTemperature.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericTemperature.Location = new System.Drawing.Point(88, 74);
            this.numericTemperature.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            this.numericTemperature.Name = "numericTemperature";
            this.numericTemperature.Size = new System.Drawing.Size(72, 19);
            this.numericTemperature.TabIndex = 5;
            // 
            // numericTopP
            // 
            this.numericTopP.DecimalPlaces = 2;
            this.numericTopP.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericTopP.Location = new System.Drawing.Point(214, 74);
            this.numericTopP.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericTopP.Name = "numericTopP";
            this.numericTopP.Size = new System.Drawing.Size(72, 19);
            this.numericTopP.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(177, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "TopP";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 352);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "チャット更新時プロンプト";
            // 
            // textBoxPrompt
            // 
            this.textBoxPrompt.AcceptsReturn = true;
            this.textBoxPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPrompt.Location = new System.Drawing.Point(15, 367);
            this.textBoxPrompt.Multiline = true;
            this.textBoxPrompt.Name = "textBoxPrompt";
            this.textBoxPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPrompt.Size = new System.Drawing.Size(451, 73);
            this.textBoxPrompt.TabIndex = 13;
            // 
            // checkBoxSkipNameAll
            // 
            this.checkBoxSkipNameAll.AutoSize = true;
            this.checkBoxSkipNameAll.Location = new System.Drawing.Point(12, 446);
            this.checkBoxSkipNameAll.Name = "checkBoxSkipNameAll";
            this.checkBoxSkipNameAll.Size = new System.Drawing.Size(147, 16);
            this.checkBoxSkipNameAll.TabIndex = 14;
            this.checkBoxSkipNameAll.Text = "名前読みを全部省略する";
            this.checkBoxSkipNameAll.UseVisualStyleBackColor = true;
            // 
            // checkBoxSkipName
            // 
            this.checkBoxSkipName.AutoSize = true;
            this.checkBoxSkipName.Location = new System.Drawing.Point(12, 468);
            this.checkBoxSkipName.Name = "checkBoxSkipName";
            this.checkBoxSkipName.Size = new System.Drawing.Size(199, 16);
            this.checkBoxSkipName.TabIndex = 15;
            this.checkBoxSkipName.Text = "連続した発言の名前読みを省略する";
            this.checkBoxSkipName.UseVisualStyleBackColor = true;
            // 
            // comboBoxBouyomiURL
            // 
            this.comboBoxBouyomiURL.FormattingEnabled = true;
            this.comboBoxBouyomiURL.Items.AddRange(new object[] {
            "http://localhost:50080/Talk"});
            this.comboBoxBouyomiURL.Location = new System.Drawing.Point(88, 517);
            this.comboBoxBouyomiURL.Name = "comboBoxBouyomiURL";
            this.comboBoxBouyomiURL.Size = new System.Drawing.Size(375, 20);
            this.comboBoxBouyomiURL.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 520);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "棒読みURL";
            // 
            // comboBoxBouyomiParam
            // 
            this.comboBoxBouyomiParam.FormattingEnabled = true;
            this.comboBoxBouyomiParam.Items.AddRange(new object[] {
            "?text={{text}}",
            "?text={{text}}&voice=5"});
            this.comboBoxBouyomiParam.Location = new System.Drawing.Point(88, 543);
            this.comboBoxBouyomiParam.Name = "comboBoxBouyomiParam";
            this.comboBoxBouyomiParam.Size = new System.Drawing.Size(375, 20);
            this.comboBoxBouyomiParam.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 546);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "棒読みパラム";
            // 
            // checkBoxSimilarOnly
            // 
            this.checkBoxSimilarOnly.AutoSize = true;
            this.checkBoxSimilarOnly.Location = new System.Drawing.Point(12, 490);
            this.checkBoxSimilarOnly.Name = "checkBoxSimilarOnly";
            this.checkBoxSimilarOnly.Size = new System.Drawing.Size(217, 16);
            this.checkBoxSimilarOnly.TabIndex = 16;
            this.checkBoxSimilarOnly.Text = "類似度が高い場合だけAPIにアクセスする";
            this.checkBoxSimilarOnly.UseVisualStyleBackColor = true;
            // 
            // numericSimilarity
            // 
            this.numericSimilarity.DecimalPlaces = 2;
            this.numericSimilarity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericSimilarity.Location = new System.Drawing.Point(235, 489);
            this.numericSimilarity.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericSimilarity.Name = "numericSimilarity";
            this.numericSimilarity.Size = new System.Drawing.Size(72, 19);
            this.numericSimilarity.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(313, 491);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "リトライ間隔(秒)";
            // 
            // numericSimilarRetryInterval
            // 
            this.numericSimilarRetryInterval.Location = new System.Drawing.Point(400, 489);
            this.numericSimilarRetryInterval.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.numericSimilarRetryInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSimilarRetryInterval.Name = "numericSimilarRetryInterval";
            this.numericSimilarRetryInterval.Size = new System.Drawing.Size(63, 19);
            this.numericSimilarRetryInterval.TabIndex = 19;
            this.numericSimilarRetryInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FormPreference
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(478, 610);
            this.Controls.Add(this.numericSimilarRetryInterval);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numericSimilarity);
            this.Controls.Add(this.checkBoxSimilarOnly);
            this.Controls.Add(this.comboBoxBouyomiParam);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBoxBouyomiURL);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBoxSkipName);
            this.Controls.Add(this.checkBoxSkipNameAll);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPrompt);
            this.Controls.Add(this.numericTopP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericTemperature);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonDefualt);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxInitPrompt);
            this.Controls.Add(this.textBoxSystem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxModel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxAPIKey);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPreference";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "環境設定";
            this.Load += new System.EventHandler(this.FormPreference_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTopP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSimilarity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSimilarRetryInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxModel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAPIKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSystem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxInitPrompt;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDefualt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericTemperature;
        private System.Windows.Forms.NumericUpDown numericTopP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPrompt;
        private System.Windows.Forms.CheckBox checkBoxSkipNameAll;
        private System.Windows.Forms.CheckBox checkBoxSkipName;
        private System.Windows.Forms.ComboBox comboBoxBouyomiURL;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxBouyomiParam;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxSimilarOnly;
        private System.Windows.Forms.NumericUpDown numericSimilarity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericSimilarRetryInterval;
    }
}