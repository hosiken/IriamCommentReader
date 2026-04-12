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
            this.contextMenuImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuImageCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuImagePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuImageLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuImageSave = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.textBoxPrompt = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.timerQuery = new System.Windows.Forms.Timer(this.components);
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxMini = new System.Windows.Forms.CheckBox();
            this.buttonDebugDirect = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxChatLog = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxRequest = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxResponse = new System.Windows.Forms.TextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButtonAPIProvider = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemGemini = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpenAI = new System.Windows.Forms.ToolStripMenuItem();
            this.labelTokens = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCapture
            // 
            this.buttonCapture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCapture.Location = new System.Drawing.Point(11, 6);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(81, 23);
            this.buttonCapture.TabIndex = 0;
            this.buttonCapture.Text = "画面キャプ";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.ContextMenuStrip = this.contextMenuImage;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 92);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // contextMenuImage
            // 
            this.contextMenuImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuImageCopy,
            this.MenuImagePaste,
            this.toolStripSeparator1,
            this.MenuImageLoad,
            this.MenuImageSave});
            this.contextMenuImage.Name = "contextMenuImage";
            this.contextMenuImage.Size = new System.Drawing.Size(135, 98);
            this.contextMenuImage.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuImage_Opening);
            // 
            // MenuImageCopy
            // 
            this.MenuImageCopy.Name = "MenuImageCopy";
            this.MenuImageCopy.Size = new System.Drawing.Size(134, 22);
            this.MenuImageCopy.Text = "コピー(&C)";
            this.MenuImageCopy.Click += new System.EventHandler(this.MenuImageCopy_Click);
            // 
            // MenuImagePaste
            // 
            this.MenuImagePaste.Name = "MenuImagePaste";
            this.MenuImagePaste.Size = new System.Drawing.Size(134, 22);
            this.MenuImagePaste.Text = "貼り付け(&P)";
            this.MenuImagePaste.Click += new System.EventHandler(this.MenuImagePaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // MenuImageLoad
            // 
            this.MenuImageLoad.Name = "MenuImageLoad";
            this.MenuImageLoad.Size = new System.Drawing.Size(134, 22);
            this.MenuImageLoad.Text = "読み込み(&L)";
            this.MenuImageLoad.Click += new System.EventHandler(this.MenuImageLoad_Click);
            // 
            // MenuImageSave
            // 
            this.MenuImageSave.Name = "MenuImageSave";
            this.MenuImageSave.Size = new System.Drawing.Size(134, 22);
            this.MenuImageSave.Text = "保存(&S)";
            this.MenuImageSave.Click += new System.EventHandler(this.MenuImageSave_Click);
            // 
            // buttonQuery
            // 
            this.buttonQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuery.Location = new System.Drawing.Point(11, 35);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(81, 23);
            this.buttonQuery.TabIndex = 1;
            this.buttonQuery.Text = "AI読み上げ";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxPrompt
            // 
            this.textBoxPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPrompt.Location = new System.Drawing.Point(0, 18);
            this.textBoxPrompt.MaxLength = 256000;
            this.textBoxPrompt.Multiline = true;
            this.textBoxPrompt.Name = "textBoxPrompt";
            this.textBoxPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPrompt.Size = new System.Drawing.Size(213, 113);
            this.textBoxPrompt.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(0, 18);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(226, 113);
            this.textBox2.TabIndex = 1;
            // 
            // timerQuery
            // 
            this.timerQuery.Interval = 15000;
            this.timerQuery.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonReadTest
            // 
            this.buttonReadTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReadTest.Location = new System.Drawing.Point(11, 64);
            this.buttonReadTest.Name = "buttonReadTest";
            this.buttonReadTest.Size = new System.Drawing.Size(81, 23);
            this.buttonReadTest.TabIndex = 2;
            this.buttonReadTest.Text = "棒読みテスト";
            this.buttonReadTest.UseVisualStyleBackColor = true;
            this.buttonReadTest.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "範囲指定";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxLeft
            // 
            this.textBoxLeft.Location = new System.Drawing.Point(102, 13);
            this.textBoxLeft.Name = "textBoxLeft";
            this.textBoxLeft.Size = new System.Drawing.Size(59, 23);
            this.textBoxLeft.TabIndex = 1;
            this.textBoxLeft.Text = "0";
            // 
            // textBoxTop
            // 
            this.textBoxTop.Location = new System.Drawing.Point(167, 13);
            this.textBoxTop.Name = "textBoxTop";
            this.textBoxTop.Size = new System.Drawing.Size(59, 23);
            this.textBoxTop.TabIndex = 2;
            this.textBoxTop.Text = "0";
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(244, 13);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(59, 23);
            this.textBoxWidth.TabIndex = 3;
            this.textBoxWidth.Text = "100";
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(309, 13);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(59, 23);
            this.textBoxHeight.TabIndex = 4;
            this.textBoxHeight.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "プロンプト";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "レスポンス";
            // 
            // checkBoxAuto
            // 
            this.checkBoxAuto.AutoSize = true;
            this.checkBoxAuto.Location = new System.Drawing.Point(11, 116);
            this.checkBoxAuto.Name = "checkBoxAuto";
            this.checkBoxAuto.Size = new System.Drawing.Size(50, 19);
            this.checkBoxAuto.TabIndex = 4;
            this.checkBoxAuto.Text = "自動";
            this.checkBoxAuto.UseVisualStyleBackColor = true;
            this.checkBoxAuto.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "秒間隔";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonPreference
            // 
            this.buttonPreference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPreference.Location = new System.Drawing.Point(469, 12);
            this.buttonPreference.Name = "buttonPreference";
            this.buttonPreference.Size = new System.Drawing.Size(75, 23);
            this.buttonPreference.TabIndex = 6;
            this.buttonPreference.Text = "環境設定";
            this.buttonPreference.UseVisualStyleBackColor = true;
            this.buttonPreference.Click += new System.EventHandler(this.buttonPreference_Click);
            // 
            // numericInterval
            // 
            this.numericInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericInterval.Location = new System.Drawing.Point(11, 136);
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
            this.numericInterval.Size = new System.Drawing.Size(81, 23);
            this.numericInterval.TabIndex = 5;
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
            this.buttonClearLog.TabIndex = 5;
            this.buttonClearLog.Text = "ログクリア";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // labelAPICount
            // 
            this.labelAPICount.AutoSize = true;
            this.labelAPICount.Location = new System.Drawing.Point(12, 187);
            this.labelAPICount.Name = "labelAPICount";
            this.labelAPICount.Size = new System.Drawing.Size(56, 15);
            this.labelAPICount.TabIndex = 7;
            this.labelAPICount.Tag = "";
            this.labelAPICount.Text = "API回数:";
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(11, 265);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(81, 23);
            this.buttonReset.TabIndex = 10;
            this.buttonReset.Text = "リセット";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // labelSimilarity
            // 
            this.labelSimilarity.AutoSize = true;
            this.labelSimilarity.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSimilarity.Location = new System.Drawing.Point(12, 204);
            this.labelSimilarity.Name = "labelSimilarity";
            this.labelSimilarity.Size = new System.Drawing.Size(24, 15);
            this.labelSimilarity.TabIndex = 8;
            this.labelSimilarity.Tag = "";
            this.labelSimilarity.Text = "似:";
            this.labelSimilarity.DoubleClick += new System.EventHandler(this.labelSimilarity_DoubleClick);
            // 
            // timerSimilarRetry
            // 
            this.timerSimilarRetry.Interval = 1000;
            this.timerSimilarRetry.Tick += new System.EventHandler(this.timerSimilarRetry_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonPreference);
            this.panel1.Controls.Add(this.buttonClearLog);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.textBoxHeight);
            this.panel1.Controls.Add(this.textBoxWidth);
            this.panel1.Controls.Add(this.textBoxTop);
            this.panel1.Controls.Add(this.textBoxLeft);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 44);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBoxMini);
            this.panel2.Controls.Add(this.buttonDebugDirect);
            this.panel2.Controls.Add(this.buttonCapture);
            this.panel2.Controls.Add(this.buttonQuery);
            this.panel2.Controls.Add(this.buttonReset);
            this.panel2.Controls.Add(this.buttonReadTest);
            this.panel2.Controls.Add(this.checkBoxAuto);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.numericInterval);
            this.panel2.Controls.Add(this.labelAPICount);
            this.panel2.Controls.Add(this.labelSimilarity);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(102, 300);
            this.panel2.TabIndex = 0;
            // 
            // checkBoxMini
            // 
            this.checkBoxMini.AutoSize = true;
            this.checkBoxMini.Location = new System.Drawing.Point(11, 93);
            this.checkBoxMini.Name = "checkBoxMini";
            this.checkBoxMini.Size = new System.Drawing.Size(75, 19);
            this.checkBoxMini.TabIndex = 3;
            this.checkBoxMini.Text = "mini優先";
            this.checkBoxMini.UseVisualStyleBackColor = true;
            // 
            // buttonDebugDirect
            // 
            this.buttonDebugDirect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDebugDirect.Location = new System.Drawing.Point(11, 236);
            this.buttonDebugDirect.Name = "buttonDebugDirect";
            this.buttonDebugDirect.Size = new System.Drawing.Size(81, 23);
            this.buttonDebugDirect.TabIndex = 9;
            this.buttonDebugDirect.Text = "直接リクエスト";
            this.buttonDebugDirect.UseVisualStyleBackColor = true;
            this.buttonDebugDirect.Visible = false;
            this.buttonDebugDirect.Click += new System.EventHandler(this.buttonDebugDirect_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxPrompt);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Size = new System.Drawing.Size(443, 154);
            this.splitContainer1.SplitterDistance = 213;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.Controls.Add(this.panel4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(443, 142);
            this.splitContainer2.SplitterDistance = 213;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(213, 142);
            this.panel4.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(226, 142);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxChatLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(218, 114);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ログ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxChatLog
            // 
            this.textBoxChatLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxChatLog.Location = new System.Drawing.Point(3, 3);
            this.textBoxChatLog.Multiline = true;
            this.textBoxChatLog.Name = "textBoxChatLog";
            this.textBoxChatLog.ReadOnly = true;
            this.textBoxChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChatLog.Size = new System.Drawing.Size(212, 108);
            this.textBoxChatLog.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxRequest);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(218, 114);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "リクエスト";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxRequest
            // 
            this.textBoxRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRequest.Location = new System.Drawing.Point(3, 3);
            this.textBoxRequest.Multiline = true;
            this.textBoxRequest.Name = "textBoxRequest";
            this.textBoxRequest.ReadOnly = true;
            this.textBoxRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxRequest.Size = new System.Drawing.Size(212, 108);
            this.textBoxRequest.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBoxResponse);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(218, 114);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "レスポンス";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResponse.Location = new System.Drawing.Point(3, 3);
            this.textBoxResponse.Multiline = true;
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.ReadOnly = true;
            this.textBoxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResponse.Size = new System.Drawing.Size(212, 108);
            this.textBoxResponse.TabIndex = 2;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(102, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer3.Panel2.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.splitContainer3.Size = new System.Drawing.Size(443, 300);
            this.splitContainer3.SplitterDistance = 142;
            this.splitContainer3.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonAPIProvider,
            this.labelTokens});
            this.statusStrip1.Location = new System.Drawing.Point(0, 344);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(553, 22);
            this.statusStrip1.TabIndex = 1;
            // 
            // toolStripDropDownButtonAPIProvider
            // 
            this.toolStripDropDownButtonAPIProvider.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonAPIProvider.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemGemini,
            this.toolStripMenuItemOpenAI});
            this.toolStripDropDownButtonAPIProvider.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonAPIProvider.Name = "toolStripDropDownButtonAPIProvider";
            this.toolStripDropDownButtonAPIProvider.Size = new System.Drawing.Size(54, 20);
            this.toolStripDropDownButtonAPIProvider.Text = "Gemni";
            this.toolStripDropDownButtonAPIProvider.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripMenuItemGemini
            // 
            this.toolStripMenuItemGemini.Name = "toolStripMenuItemGemini";
            this.toolStripMenuItemGemini.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemGemini.Tag = "0";
            this.toolStripMenuItemGemini.Text = "Gemini";
            this.toolStripMenuItemGemini.Click += new System.EventHandler(this.toolStripMenuItemToggleAI_Click);
            // 
            // toolStripMenuItemOpenAI
            // 
            this.toolStripMenuItemOpenAI.Name = "toolStripMenuItemOpenAI";
            this.toolStripMenuItemOpenAI.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemOpenAI.Tag = "1";
            this.toolStripMenuItemOpenAI.Text = "OpenAI";
            this.toolStripMenuItemOpenAI.Click += new System.EventHandler(this.toolStripMenuItemToggleAI_Click);
            // 
            // labelTokens
            // 
            this.labelTokens.Name = "labelTokens";
            this.labelTokens.Size = new System.Drawing.Size(453, 17);
            this.labelTokens.Spring = true;
            this.labelTokens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer3);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 44);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panel3.Size = new System.Drawing.Size(553, 300);
            this.panel3.TabIndex = 2;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(553, 366);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "FormMain";
            this.Text = "IriamCommentReader 20260412";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ContextMenuStrip contextMenuImage;
        private System.Windows.Forms.ToolStripMenuItem MenuImageCopy;
        private System.Windows.Forms.ToolStripMenuItem MenuImageSave;
        private System.Windows.Forms.Button buttonDebugDirect;
        private System.Windows.Forms.ToolStripMenuItem MenuImagePaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuImageLoad;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBoxChatLog;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBoxRequest;
        private System.Windows.Forms.TextBox textBoxResponse;
        private System.Windows.Forms.ToolStripStatusLabel labelTokens;
        private System.Windows.Forms.CheckBox checkBoxMini;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonAPIProvider;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGemini;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenAI;
    }
}

