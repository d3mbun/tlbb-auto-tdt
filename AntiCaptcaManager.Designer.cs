using System.Windows.Forms;

namespace _i
{
    partial class AntiCaptcaManager
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
            this.panelCapcha = new System.Windows.Forms.Panel();
            this.tmrHide = new System.Windows.Forms.Timer(this.components);
            this.txtAnswer = new _i.NumericTextBox();
            this.SuspendLayout();
            // 
            // panelCapcha
            // 
            this.panelCapcha.AutoScroll = true;
            this.panelCapcha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(240)))));
            this.panelCapcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelCapcha.Location = new System.Drawing.Point(0, 0);
            this.panelCapcha.Name = "panelCapcha";
            this.panelCapcha.Size = new System.Drawing.Size(160, 45);
            this.panelCapcha.TabIndex = 1;
            // 
            // tmrHide
            // 
            this.tmrHide.Enabled = true;
            this.tmrHide.Tick += new System.EventHandler(this.tmrHide_Tick);
            // 
            // txtAnswer
            // 
            this.txtAnswer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAnswer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnswer.Location = new System.Drawing.Point(160, 0);
            this.txtAnswer.Margin = new System.Windows.Forms.Padding(0);
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.Size = new System.Drawing.Size(100, 45);
            this.txtAnswer.TabIndex = 54;
            this.txtAnswer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAnswer.WaterMark = "answer...";
            this.txtAnswer.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAnswer.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnswer.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAnswer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAnswer_KeyDown);
            // 
            // AntiCaptcaManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 45);
            this.ControlBox = false;
            this.Controls.Add(this.txtAnswer);
            this.Controls.Add(this.panelCapcha);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(260, 85);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(260, 45);
            this.Name = "AntiCaptcaManager";
            this.ShowInTaskbar = false;
            this.Text = "Captcha";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AntiCaptcaManager_FormClosing);
            this.Load += new System.EventHandler(this.AntiCaptcaManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Panel panelCapcha;
        private Timer tmrHide;
        private NumericTextBox txtAnswer;
    }
}