namespace _i
{
    partial class EnterCaptchaEx
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
            this.components = new System.ComponentModel.Container();
            this.picCaptcha = new System.Windows.Forms.PictureBox();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.lblAnswer = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radAnswer4 = new System.Windows.Forms.RadioButton();
            this.radAnswer3 = new System.Windows.Forms.RadioButton();
            this.radAnswer2 = new System.Windows.Forms.RadioButton();
            this.radAnswer1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picCaptcha
            // 
            this.picCaptcha.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.picCaptcha.ErrorImage = global::MicroAuto.Properties.Resources.captcha;
            this.picCaptcha.Image = global::_i.Properties.Resources.loading;
            //this.picCaptcha.InitialImage = global::MicroAuto.Properties.Resources.captcha;
            this.picCaptcha.Location = new System.Drawing.Point(3, 23);
            this.picCaptcha.Name = "picCaptcha";
            this.picCaptcha.Size = new System.Drawing.Size(160, 45);
            this.picCaptcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCaptcha.TabIndex = 57;
            this.picCaptcha.TabStop = false;
            this.picCaptcha.Click += new System.EventHandler(this.picCaptcha_Click);
            this.picCaptcha.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picCaptcha_MouseClick);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 1000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // lblAnswer
            // 
            this.lblAnswer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblAnswer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnswer.ForeColor = System.Drawing.Color.Black;
            this.lblAnswer.Location = new System.Drawing.Point(3, 192);
            this.lblAnswer.Name = "lblAnswer";
            this.lblAnswer.Size = new System.Drawing.Size(160, 30);
            this.lblAnswer.TabIndex = 60;
            this.lblAnswer.Text = "1 or 2 or 3 or 4 is correct?";
            this.lblAnswer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAnswer.Click += new System.EventHandler(this.lblAnswer_Click);
            this.lblAnswer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblAnswer_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radAnswer4);
            this.groupBox1.Controls.Add(this.radAnswer3);
            this.groupBox1.Controls.Add(this.radAnswer2);
            this.groupBox1.Controls.Add(this.radAnswer1);
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(3, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 112);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Answers";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(26, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "1";
            // 
            // radAnswer4
            // 
            this.radAnswer4.AutoSize = true;
            this.radAnswer4.Location = new System.Drawing.Point(46, 88);
            this.radAnswer4.Name = "radAnswer4";
            this.radAnswer4.Size = new System.Drawing.Size(53, 17);
            this.radAnswer4.TabIndex = 3;
            this.radAnswer4.TabStop = true;
            this.radAnswer4.Text = "AAAA";
            this.radAnswer4.UseVisualStyleBackColor = true;
            // 
            // radAnswer3
            // 
            this.radAnswer3.AutoSize = true;
            this.radAnswer3.Location = new System.Drawing.Point(46, 65);
            this.radAnswer3.Name = "radAnswer3";
            this.radAnswer3.Size = new System.Drawing.Size(53, 17);
            this.radAnswer3.TabIndex = 2;
            this.radAnswer3.TabStop = true;
            this.radAnswer3.Text = "AAAA";
            this.radAnswer3.UseVisualStyleBackColor = true;
            // 
            // radAnswer2
            // 
            this.radAnswer2.AutoSize = true;
            this.radAnswer2.Location = new System.Drawing.Point(46, 42);
            this.radAnswer2.Name = "radAnswer2";
            this.radAnswer2.Size = new System.Drawing.Size(53, 17);
            this.radAnswer2.TabIndex = 1;
            this.radAnswer2.TabStop = true;
            this.radAnswer2.Text = "AAAA";
            this.radAnswer2.UseVisualStyleBackColor = true;
            // 
            // radAnswer1
            // 
            this.radAnswer1.AutoSize = true;
            this.radAnswer1.Location = new System.Drawing.Point(46, 19);
            this.radAnswer1.Name = "radAnswer1";
            this.radAnswer1.Size = new System.Drawing.Size(53, 17);
            this.radAnswer1.TabIndex = 0;
            this.radAnswer1.TabStop = true;
            this.radAnswer1.Text = "AAAA";
            this.radAnswer1.UseVisualStyleBackColor = true;
            this.radAnswer1.CheckedChanged += new System.EventHandler(this.radAnswer1_CheckedChanged);
            // 
            // EnterCaptchaEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picCaptcha);
            this.Controls.Add(this.lblAnswer);
            this.Name = "EnterCaptchaEx";
            this.Size = new System.Drawing.Size(166, 230);
            this.Load += new System.EventHandler(this.EnterCaptchaEx_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox picCaptcha;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Label lblAnswer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radAnswer4;
        private System.Windows.Forms.RadioButton radAnswer3;
        private System.Windows.Forms.RadioButton radAnswer2;
        private System.Windows.Forms.RadioButton radAnswer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}