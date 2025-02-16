namespace IriamCommentReader
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.textBoxPrompt = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.timerQuery = new System.Windows.Forms.Timer(this.components);
            this.textBoxChatLog = new System.Windows.Forms.TextBox();
            this.buttonReadTest = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxLeft = new System.Windows.Forms.TextBox();
            this.textBoxTop = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxAuto = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonPreference = new System.Windows.Forms.Button();
            this.numericInterval = new System.Windows.Forms.NumericUpDown();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.labelAPICount = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelSimilarity = new System.Windows.Forms.Label();
            this.timerSimilarRetry = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCapture
            // 
            this.buttonCapture.Location = new System.Drawing.Point(12, 50);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonCapture.TabIndex = 6;
            this.buttonCapture.Text = "画面キャプ";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(93, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(218, 151);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(12, 79);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(75, 23);
            this.buttonQuery.TabIndex = 7;
            this.buttonQuery.Text = "AIに読ませる";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxPrompt
            // 
            this.textBoxPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxPrompt.Location = new System.Drawing.Point(93, 226);
            this.textBoxPrompt.Multiline = true;
            this.textBoxPrompt.Name = "textBoxPrompt";
            this.textBoxPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPrompt.Size = new System.Drawing.Size(218, 100);
            this.textBoxPrompt.TabIndex = 14;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(317, 226);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(224, 100);
            this.textBox2.TabIndex = 16;
            // 
            // timerQuery
            // 
            this.timerQuery.Interval = 15000;
            this.timerQuery.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxChatLog
            // 
            this.textBoxChatLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChatLog.Location = new System.Drawing.Point(317, 50);
            this.textBoxChatLog.Multiline = true;
            this.textBoxChatLog.Name = "textBoxChatLog";
            this.textBoxChatLog.ReadOnly = true;
            this.textBoxChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChatLog.Size = new System.Drawing.Size(224, 151);
            this.textBoxChatLog.TabIndex = 12;
            // 
            // buttonReadTest
            // 
            this.buttonReadTest.Location = new System.Drawing.Point(12, 211);
            this.buttonReadTest.Name = "buttonReadTest";
            this.buttonReadTest.Size = new System.Drawing.Size(75, 23);
            this.buttonReadTest.TabIndex = 11;
            this.buttonReadTest.Text = "棒読みテスト";
            this.buttonReadTest.UseVisualStyleBackColor = true;
            this.buttonReadTest.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "範囲指定";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxLeft
            // 
            this.textBoxLeft.Location = new System.Drawing.Point(93, 14);
            this.textBoxLeft.Name = "textBoxLeft";
            this.textBoxLeft.Size = new System.Drawing.Size(59, 19);
            this.textBoxLeft.TabIndex = 1;
            this.textBoxLeft.Text = "0";
            // 
            // textBoxTop
            // 
            this.textBoxTop.Location = new System.Drawing.Point(158, 14);
            this.textBoxTop.Name = "textBoxTop";
            this.textBoxTop.Size = new System.Drawing.Size(59, 19);
            this.textBoxTop.TabIndex = 2;
            this.textBoxTop.Text = "0";
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(235, 14);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(59, 19);
            this.textBoxWidth.TabIndex = 3;
            this.textBoxWidth.Text = "100";
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(300, 14);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(59, 19);
            this.textBoxHeight.TabIndex = 4;
            this.textBoxHeight.Text = "100";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "プロンプト";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "レスポンス";
            // 
            // checkBoxAuto
            // 
            this.checkBoxAuto.AutoSize = true;
            this.checkBoxAuto.Location = new System.Drawing.Point(12, 121);
            this.checkBoxAuto.Name = "checkBoxAuto";
            this.checkBoxAuto.Size = new System.Drawing.Size(48, 16);
            this.checkBoxAuto.TabIndex = 8;
            this.checkBoxAuto.Text = "自動";
            this.checkBoxAuto.UseVisualStyleBackColor = true;
            this.checkBoxAuto.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "秒間隔";
            // 
            // buttonPreference
            // 
            this.buttonPreference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPreference.Location = new System.Drawing.Point(469, 12);
            this.buttonPreference.Name = "buttonPreference";
            this.buttonPreference.Size = new System.Drawing.Size(75, 23);
            this.buttonPreference.TabIndex = 5;
            this.buttonPreference.Text = "環境設定";
            this.buttonPreference.UseVisualStyleBackColor = true;
            this.buttonPreference.Click += new System.EventHandler(this.buttonPreference_Click);
            // 
            // numericInterval
            // 
            this.numericInterval.Location = new System.Drawing.Point(12, 143);
            this.numericInterval.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericInterval.Name = "numericInterval";
            this.numericInterval.Size = new System.Drawing.Size(75, 19);
            this.numericInterval.TabIndex = 9;
            this.numericInterval.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericInterval.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearLog.Location = new System.Drawing.Point(388, 12);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 17;
            this.buttonClearLog.Text = "ログクリア";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // labelAPICount
            // 
            this.labelAPICount.AutoSize = true;
            this.labelAPICount.Location = new System.Drawing.Point(10, 254);
            this.labelAPICount.Name = "labelAPICount";
            this.labelAPICount.Size = new System.Drawing.Size(49, 12);
            this.labelAPICount.TabIndex = 18;
            this.labelAPICount.Tag = "";
            this.labelAPICount.Text = "API回数:";
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReset.Location = new System.Drawing.Point(12, 303);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 19;
            this.buttonReset.Text = "リセット";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // labelSimilarity
            // 
            this.labelSimilarity.AutoSize = true;
            this.labelSimilarity.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSimilarity.Location = new System.Drawing.Point(10, 275);
            this.labelSimilarity.Name = "labelSimilarity";
            this.labelSimilarity.Size = new System.Drawing.Size(31, 12);
            this.labelSimilarity.TabIndex = 20;
            this.labelSimilarity.Tag = "";
            this.labelSimilarity.Text = "類似:";
            this.labelSimilarity.DoubleClick += new System.EventHandler(this.labelSimilarity_DoubleClick);
            // 
            // timerSimilarRetry
            // 
            this.timerSimilarRetry.Interval = 1000;
            this.timerSimilarRetry.Tick += new System.EventHandler(this.timerSimilarRetry_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 338);
            this.Controls.Add(this.labelSimilarity);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelAPICount);
            this.Controls.Add(this.buttonClearLog);
            this.Controls.Add(this.numericInterval);
            this.Controls.Add(this.buttonPreference);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBoxAuto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonReadTest);
            this.Controls.Add(this.textBoxChatLog);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.textBoxWidth);
            this.Controls.Add(this.textBoxTop);
            this.Controls.Add(this.textBoxLeft);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBoxPrompt);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonCapture);
            this.Name = "FormMain";
            this.Text = "IriamCommentReader 20250216";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.TextBox textBoxPrompt;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Timer timerQuery;
        private System.Windows.Forms.TextBox textBoxChatLog;
        private System.Windows.Forms.Button buttonReadTest;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxLeft;
        private System.Windows.Forms.TextBox textBoxTop;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxAuto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonPreference;
        private System.Windows.Forms.NumericUpDown numericInterval;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.Label labelAPICount;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label labelSimilarity;
        private System.Windows.Forms.Timer timerSimilarRetry;
    }
}

