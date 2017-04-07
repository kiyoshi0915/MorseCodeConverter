namespace モールス信号変換機
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnConv = new System.Windows.Forms.Button();
            this.rdoEnglishMode = new System.Windows.Forms.RadioButton();
            this.rdoJapaneseMode = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.rdoAutoSelect = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.linkConvView = new System.Windows.Forms.LinkLabel();
            this.rtxtMorse = new System.Windows.Forms.RichTextBox();
            this.rtxtStringForMorse = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.txtFrequency = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(16, 104);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(235, 125);
            this.txtInput.TabIndex = 3;
            // 
            // btnConv
            // 
            this.btnConv.Location = new System.Drawing.Point(176, 235);
            this.btnConv.Name = "btnConv";
            this.btnConv.Size = new System.Drawing.Size(75, 27);
            this.btnConv.TabIndex = 4;
            this.btnConv.Text = "変換";
            this.btnConv.UseVisualStyleBackColor = true;
            this.btnConv.Click += new System.EventHandler(this.btnConv_Click);
            // 
            // rdoEnglishMode
            // 
            this.rdoEnglishMode.AutoSize = true;
            this.rdoEnglishMode.Location = new System.Drawing.Point(16, 59);
            this.rdoEnglishMode.Name = "rdoEnglishMode";
            this.rdoEnglishMode.Size = new System.Drawing.Size(138, 16);
            this.rdoEnglishMode.TabIndex = 2;
            this.rdoEnglishMode.TabStop = true;
            this.rdoEnglishMode.Text = "英字のみの入力（欧文）";
            this.rdoEnglishMode.UseVisualStyleBackColor = true;
            // 
            // rdoJapaneseMode
            // 
            this.rdoJapaneseMode.AutoSize = true;
            this.rdoJapaneseMode.Location = new System.Drawing.Point(16, 82);
            this.rdoJapaneseMode.Name = "rdoJapaneseMode";
            this.rdoJapaneseMode.Size = new System.Drawing.Size(150, 16);
            this.rdoJapaneseMode.TabIndex = 3;
            this.rdoJapaneseMode.TabStop = true;
            this.rdoJapaneseMode.Text = "日本語のみの入力（和文）";
            this.rdoJapaneseMode.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblErrMsg);
            this.groupBox1.Controls.Add(this.rdoJapaneseMode);
            this.groupBox1.Controls.Add(this.rdoAutoSelect);
            this.groupBox1.Controls.Add(this.rdoEnglishMode);
            this.groupBox1.Controls.Add(this.txtInput);
            this.groupBox1.Controls.Add(this.btnConv);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 340);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "入力言語";
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(16, 265);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(237, 58);
            this.lblErrMsg.TabIndex = 5;
            this.lblErrMsg.Text = "lblErrMsg\r\n1\r\n2\r\n3\r\n4";
            // 
            // rdoAutoSelect
            // 
            this.rdoAutoSelect.AutoSize = true;
            this.rdoAutoSelect.Location = new System.Drawing.Point(16, 37);
            this.rdoAutoSelect.Name = "rdoAutoSelect";
            this.rdoAutoSelect.Size = new System.Drawing.Size(119, 16);
            this.rdoAutoSelect.TabIndex = 1;
            this.rdoAutoSelect.TabStop = true;
            this.rdoAutoSelect.Text = "欧文和文自動判定";
            this.rdoAutoSelect.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 366);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(300, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "英数字、ひらがな、カタカナ、スペース、一部の記号のみ入力可";
            // 
            // linkConvView
            // 
            this.linkConvView.AutoSize = true;
            this.linkConvView.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkConvView.Location = new System.Drawing.Point(640, 392);
            this.linkConvView.Name = "linkConvView";
            this.linkConvView.Size = new System.Drawing.Size(121, 14);
            this.linkConvView.TabIndex = 7;
            this.linkConvView.TabStop = true;
            this.linkConvView.Text = "モールス符号表リンク";
            this.linkConvView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkConvView_LinkClicked);
            // 
            // rtxtMorse
            // 
            this.rtxtMorse.BackColor = System.Drawing.Color.PaleTurquoise;
            this.rtxtMorse.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtMorse.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rtxtMorse.Location = new System.Drawing.Point(300, 21);
            this.rtxtMorse.Name = "rtxtMorse";
            this.rtxtMorse.ReadOnly = true;
            this.rtxtMorse.Size = new System.Drawing.Size(461, 174);
            this.rtxtMorse.TabIndex = 8;
            this.rtxtMorse.TabStop = false;
            this.rtxtMorse.Text = "MORSE_STRING";
            // 
            // rtxtStringForMorse
            // 
            this.rtxtStringForMorse.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtStringForMorse.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rtxtStringForMorse.Location = new System.Drawing.Point(300, 201);
            this.rtxtStringForMorse.Name = "rtxtStringForMorse";
            this.rtxtStringForMorse.ReadOnly = true;
            this.rtxtStringForMorse.Size = new System.Drawing.Size(461, 151);
            this.rtxtStringForMorse.TabIndex = 9;
            this.rtxtStringForMorse.TabStop = false;
            this.rtxtStringForMorse.Text = "STRING_FOR_MORSE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 393);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "一点の音の長さ（1～1000ミリ秒）";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 365);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "音の高さ（32～32767Hz）";
            // 
            // txtDuration
            // 
            this.txtDuration.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtDuration.Location = new System.Drawing.Point(193, 390);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(53, 19);
            this.txtDuration.TabIndex = 6;
            this.txtDuration.Text = "100";
            this.txtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtFrequency
            // 
            this.txtFrequency.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtFrequency.Location = new System.Drawing.Point(193, 362);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.Size = new System.Drawing.Size(53, 19);
            this.txtFrequency.TabIndex = 5;
            this.txtFrequency.Text = "800";
            this.txtFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(252, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "Hz";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(252, 393);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "ミリ秒";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(298, 393);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(341, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "このアプリでは、和文中の欧文は全角 \'（\' と \'）\' で囲う仕様にしています。";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 429);
            this.Controls.Add(this.txtFrequency);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.rtxtStringForMorse);
            this.Controls.Add(this.rtxtMorse);
            this.Controls.Add(this.linkConvView);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "モールス信号変換機";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnConv;
        private System.Windows.Forms.RadioButton rdoEnglishMode;
        private System.Windows.Forms.RadioButton rdoJapaneseMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblErrMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkConvView;
        private System.Windows.Forms.RichTextBox rtxtMorse;
        private System.Windows.Forms.RichTextBox rtxtStringForMorse;
        private System.Windows.Forms.RadioButton rdoAutoSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.TextBox txtFrequency;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

