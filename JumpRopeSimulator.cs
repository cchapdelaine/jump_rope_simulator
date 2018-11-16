/*
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

 Code modifed from WiiBalanceScale by Bernhard Schelling
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
using System.Windows.Forms;
using WiimoteLib;

[assembly: System.Reflection.AssemblyTitle("JumpRope")]
[assembly: System.Reflection.AssemblyProduct("JumpRope")]
[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.0.0")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]

namespace JumpRope
{
    internal class JumpRopeSimulator
    {
        static JumpRopeForm form = null;
        static Wiimote balanceBoard = null;
        static ConnectionManager connectionManager = null;
        static Timer BoardTimer = null;
        static float threshold;
        static bool wentUp;

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            form = new JumpRopeForm();

            //connect to the Balance Board
            ConnectBalanceBoard();
            if (form== null) return; //connecting required application restart, end this process here.

            //Continue running BoardTimer_tick()
            BoardTimer = new System.Windows.Forms.Timer();
            BoardTimer.Interval = 50;
            BoardTimer.Tick += new System.EventHandler(BoardTimer_Tick);
            BoardTimer.Start();

            Application.Run(form);
            Shutdown();
        }


        static void Shutdown()
        {
            if (BoardTimer != null) {
                BoardTimer.Stop();
                BoardTimer = null;
            }

            if (connectionManager != null) {
                connectionManager.Cancel();
                connectionManager = null;
            }

            if (form!= null) {
                if (form.Visible) form.Close();
                form = null;
            }
        }

        static void ConnectBalanceBoard()
        {
            // Connect the Wii Balance Board to the computer.

            bool Connected = true;

            try {
                balanceBoard = new Wiimote();
                balanceBoard.Connect();
                balanceBoard.SetLEDs(1);
                balanceBoard.GetStatus();
            } catch {
                Connected = false;
            }

            if (!Connected || balanceBoard.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
            {
                // Close application if a device is not connected or the connected device is not a balance board.
                if (ConnectionManager.ElevateProcessNeedRestart()) {
                    Shutdown();
                    return;
                }

                if (connectionManager == null) connectionManager = new ConnectionManager();

                connectionManager.ConnectNextWiiMote();
                return;
            }
            if (connectionManager != null) {
                connectionManager.Cancel();
                connectionManager = null;
            }

            form.Refresh();
        }


        static void getWeight()
        {
            // Get the weight of the user.
            float kg = balanceBoard.WiimoteState.BalanceBoardState.WeightKg;
            threshold = (kg * 2.0F) + 5;  // Proportional to user's weight.
        }


        static void BoardTimer_Tick(object sender, System.EventArgs e)
        {
            // Get the current value of the jump counter from the UI.
            int jumpCounter = Int32.Parse(form.jumpCounter.Text);

            if (connectionManager != null)
            {
                if (connectionManager.IsRunning())
                {
                    form.connectingLabel.Visible = true;

                    return;
                }
                if (connectionManager.HadError())
                {
                    BoardTimer.Stop();
                    System.Windows.Forms.MessageBox.Show(form, "No compatible bluetooth adapter found - Quitting", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Shutdown();
                    return;
                }
                ConnectBalanceBoard();
                return;
            }

            // Sets the top and bottom thresholds for box to move between while jumping
            System.Drawing.Point topThreshold = new System.Drawing.Point(350, 200);
            System.Drawing.Point bottomThreshold = new System.Drawing.Point(350, 250);
            System.Drawing.Point loc = form.jumpMan.Location;

            int center = (form.Width / 2) - (form.jumpMan.Size.Width / 2);  // center the box on screen.

            // Animate the box when the user jumps up and down.
            if (wentUp && loc.Y >= topThreshold.Y)
            {
                // Animate going up
                form.jumpMan.Location = new System.Drawing.Point(center, form.jumpMan.Location.Y - 5);
            }
            else if (!wentUp && loc.Y <= bottomThreshold.Y)
            {
                // Animate going down, it travels faster down than up
                form.jumpMan.Location = new System.Drawing.Point(center, form.jumpMan.Location.Y + 10);
            }

            getWeight();

            // Don't display "connecting to wii board" text if the wii board is connected.
            form.connectingLabel.Visible = false;

            // Get the pressure from each of the four quadrants of the Balance Board
            float topLeft = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
            float topRight = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
            float bottomLeft = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;
            float bottomRight = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;

            // Keep values above 0; helps to normalize the numbers
            if (topLeft < 0) topLeft = 0;
            if (topRight < 0) topRight = 0;
            if (bottomLeft < 0) bottomLeft = 0;
            if (bottomRight < 0) bottomRight = 0;

            float topWeight = topLeft + topRight; // Weight of combined top quadrants.
            float bottomWeight = bottomLeft + bottomRight;  // Weight of combined bottom quadrants.
            float difference = topWeight - bottomWeight;


            /* If the weight on the top quadrants is twice the weight of the back quadrants (i.e., twice
               the user's weight), they have jumped. */
            if (difference >= threshold)
            {
                // The user jumped. 
                wentUp = true;
            }

            if (difference <= threshold && wentUp)
            {
                jumpCounter++;
                form.jumpCounter.Text = jumpCounter.ToString();
                wentUp = false;
            }
        }
    }
}
