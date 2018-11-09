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

        internal Label topLeft;
        internal Label topRight;
        internal Label bottomLeft;
        internal Label bottomRight;

        internal Button btnReset;
        internal Label lblQuality;
        internal Label lblUnit;

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
            this.topLeft = new System.Windows.Forms.Label();
            this.topRight = new System.Windows.Forms.Label();
            this.bottomLeft = new System.Windows.Forms.Label();
            this.bottomRight = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblQuality = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblWeight
            // 
            this.topLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topLeft.Font = new System.Drawing.Font("Lucida Console", 75F);
            this.topLeft.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.topLeft.Location = new System.Drawing.Point(702, 9);
            this.topLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.topLeft.Name = "lblWeight";
            this.topLeft.Size = new System.Drawing.Size(611, 133);
            this.topLeft.TabIndex = 0;
            this.topLeft.Text = "88.710";
            this.topLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // topRight
            // 
            this.topRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topRight.Font = new System.Drawing.Font("Lucida Console", 75F);
            this.topRight.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.topRight.Location = new System.Drawing.Point(10, 10);
            this.topRight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.topRight.Name = "topRight";
            this.topRight.Size = new System.Drawing.Size(659, 133);
            this.topRight.TabIndex = 0;
            this.topRight.Text = "88.710";
            this.topRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bottomLeft
            // 
            this.bottomLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomLeft.Font = new System.Drawing.Font("Lucida Console", 75F);
            this.bottomLeft.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.bottomLeft.Location = new System.Drawing.Point(10, 172);
            this.bottomLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.bottomLeft.Name = "bottomLeft";
            this.bottomLeft.Size = new System.Drawing.Size(659, 133);
            this.bottomLeft.TabIndex = 0;
            this.bottomLeft.Text = "88.710";
            this.bottomLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bottomRight
            // 
            this.bottomRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomRight.Font = new System.Drawing.Font("Lucida Console", 75F);
            this.bottomRight.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bottomRight.Location = new System.Drawing.Point(727, 172);
            this.bottomRight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.bottomRight.Name = "bottomRight";
            this.bottomRight.Size = new System.Drawing.Size(586, 133);
            this.bottomRight.TabIndex = 0;
            this.bottomRight.Text = "88.710";
            this.bottomRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnReset.Location = new System.Drawing.Point(132, 463);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(1065, 98);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Zero";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // lblQuality
            // 
            this.lblQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuality.Font = new System.Drawing.Font("Wingdings", 80F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(127)));
            this.lblQuality.Location = new System.Drawing.Point(0, 305);
            this.lblQuality.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(1326, 154);
            this.lblQuality.TabIndex = 8;
            this.lblQuality.Text = "®®®¡¡";
            this.lblQuality.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUnit
            // 
            this.lblUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnit.Location = new System.Drawing.Point(1206, 348);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(120, 92);
            this.lblUnit.TabIndex = 9;
            this.lblUnit.Text = "kg";
            // 
            // WiiBalanceScaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 586);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblQuality);
            this.Controls.Add(this.topLeft);
            this.Controls.Add(this.topRight);
            this.Controls.Add(this.bottomLeft);
            this.Controls.Add(this.bottomRight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "WiiBalanceScaleForm";
            this.Text = "Wii Balance Scale";
            this.ResumeLayout(false);

        }
        #endregion
    }
}