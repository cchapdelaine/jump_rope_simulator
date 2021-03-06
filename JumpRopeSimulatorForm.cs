﻿/*
Author:     Caitlin Chapdelaine and John Eastman
Class:      CSI-400-01
Assignment: App Prototype
Due date:   11/16/18 @ 11:59 pm

Description:
    An application that allows the user to simulate jump roping. The app utilizes a
    Wii Balance Board and (tentatively) a Wiimote.

Certification of Authenticity:
    I certify that this is entirely my own work, except where I have given
    fully-documented references to the work of others. I understand the definition
    and consequences of plagiarism and acknowledge that the assessor of this
    assignment may, for the purpose of assessing this assignment:
    - Reproduce this assignment and provide a copy to another member of academic
    - staff; and/or Communicate a copy of this assignment to a plagiarism checking
    - service (which may then retain a copy of this assignment on its database for
    - the purpose of future plagiarism checking)
*/

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

namespace JumpRope
{
    class JumpRopeForm : Form
    {
        public JumpRopeForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        internal Label jumpCounter;
        internal Label jumpCounterLabel;
        internal Label jumpMan;
        internal Label information;  
        internal Label ground;
        
        


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
            this.information = new System.Windows.Forms.Label();
            this.jumpCounterLabel = new System.Windows.Forms.Label();
            this.jumpMan = new System.Windows.Forms.Label();
            this.ground = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // jumpCounter
            // 
            this.jumpCounter.Font = new System.Drawing.Font("Arial", 25F);
            this.jumpCounter.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.jumpCounter.Location = new System.Drawing.Point(12, 69);
            this.jumpCounter.Name = "jumpCounter";
            this.jumpCounter.Size = new System.Drawing.Size(100, 100);
            this.jumpCounter.TabIndex = 1;
            this.jumpCounter.Text = "0";
            // 
            // information
            // 
            this.information.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.information.Font = new System.Drawing.Font("Arial", 50F);
            this.information.Location = new System.Drawing.Point(-15, -16);
            this.information.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.information.Name = "information";
            this.information.Size = new System.Drawing.Size(1385, 518);
            this.information.TabIndex = 0;
            this.information.Text = "Connecting to Wiiboard...\n(press sync button)";
            this.information.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jumpCounterLabel
            // 
            this.jumpCounterLabel.Font = new System.Drawing.Font("Arial", 25F);
            this.jumpCounterLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.jumpCounterLabel.Location = new System.Drawing.Point(-9, 0);
            this.jumpCounterLabel.Name = "jumpCounterLabel";
            this.jumpCounterLabel.Size = new System.Drawing.Size(421, 126);
            this.jumpCounterLabel.TabIndex = 1;
            this.jumpCounterLabel.Text = "Jump Counter";
            // 
            // jumpMan
            // 
            this.jumpMan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jumpMan.Font = new System.Drawing.Font("Wingdings", 150F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(127)));
            this.jumpMan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(82)))), ((int)(((byte)(255)))));
            this.jumpMan.Location = new System.Drawing.Point(607, 313);
            this.jumpMan.Name = "jumpMan";
            this.jumpMan.Size = new System.Drawing.Size(115, 123);
            this.jumpMan.TabIndex = 8;
            this.jumpMan.Text = "n";
            this.jumpMan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ground
            // 
            this.ground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ground.Font = new System.Drawing.Font("Arial", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(250)));
            this.ground.Location = new System.Drawing.Point(60, 251);
            this.ground.Name = "ground";
            this.ground.Size = new System.Drawing.Size(1209, 322);
            this.ground.TabIndex = 9;
            this.ground.Text = "_____________________________";
            this.ground.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JumpRopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 500);
            this.Controls.Add(this.information);
            this.Controls.Add(this.jumpCounter);
            this.Controls.Add(this.jumpCounterLabel);
            this.Controls.Add(this.jumpMan);
            this.Controls.Add(this.ground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "JumpRopeForm";
            this.Text = "Jump rope simulator";
            this.Load += new System.EventHandler(this.JumpRopeForm_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private void JumpRopeForm_Load(object sender, EventArgs e)
        {

        }
    }
}