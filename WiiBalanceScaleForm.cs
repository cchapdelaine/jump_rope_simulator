/*********************************************************************************
WiiBalanceScale

MIT License

Copyright (c) 2017 Bernhard Schelling

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
**********************************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WiiBalanceScale
{
    class WiiBalanceScaleForm : Form
    {
        public WiiBalanceScaleForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        internal Label jumpCounter;
        internal Graphics jumpMan;
        internal Label connectingLabel;  // Connecting to wiiboard.

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
            this.jumpCounter = new System.Windows.Forms.Label();
            this.connectingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // jumpCounter
            // 
            this.jumpCounter.Font = new System.Drawing.Font("Lucida Console", 25F);
            this.jumpCounter.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.jumpCounter.Location = new System.Drawing.Point(565, 304);
            this.jumpCounter.Name = "jumpCounter";
            this.jumpCounter.Size = new System.Drawing.Size(100, 100);
            this.jumpCounter.TabIndex = 1;
            this.jumpCounter.Text = "0";
            // 
            // connectingLabel
            // 
            this.connectingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectingLabel.Font = new System.Drawing.Font("Lucida Console", 80F);
            this.connectingLabel.Location = new System.Drawing.Point(38, 58);
            this.connectingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.connectingLabel.Name = "connectingLabel";
            this.connectingLabel.Size = new System.Drawing.Size(1164, 406);
            this.connectingLabel.TabIndex = 0;
            this.connectingLabel.Text = "Connecting to Wiiboard...";
            this.connectingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WiiBalanceScaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 514);
            this.Controls.Add(this.connectingLabel);
            this.Controls.Add(this.jumpCounter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "WiiBalanceScaleForm";
            this.Text = "Wii Balance Scale";
            this.Load += new System.EventHandler(this.WiiBalanceScaleForm_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private void WiiBalanceScaleForm_Load(object sender, EventArgs e)
        {

        }
    }
}