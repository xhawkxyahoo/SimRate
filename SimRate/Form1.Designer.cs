
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimRate
{
    partial class Form1
    {
        private Label titleLabel;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick_1);
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(100, 60);
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 10, 10);
            this.BackColor = Color.Black;
            this.Opacity = 0.7;

            this.textBox1.Text = "---";
            this.textBox1.ForeColor = Color.White;
            this.textBox1.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.textBox1.AutoSize = false;
            this.textBox1.Width = this.Width;
            this.textBox1.Height = 25;
            this.textBox1.Location = new Point(0, 25);
            this.textBox1.TextAlign = ContentAlignment.MiddleCenter;

            titleLabel = new Label
            {
                Text = "SimRate",
                ForeColor = Color.White,
                Font = new Font("Arial", 12f, FontStyle.Bold),
                AutoSize = false,
                Width = this.Width,
                Height = 25,
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new EventHandler(form1_Shown);
            this.ResumeLayout(false);

            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label textBox1;
    }
}

