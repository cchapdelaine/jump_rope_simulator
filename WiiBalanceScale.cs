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

[assembly: System.Reflection.AssemblyTitle("WiiBalanceScale")]
[assembly: System.Reflection.AssemblyProduct("WiiBalanceScale")]
[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.0.0")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]

namespace WiiBalanceScale
{
    internal class WiiBalanceScale
    {
        static bool bbConnected = false;
        static bool wmConnected = false;

        static WiiBalanceScaleForm f = null;
        static Wiimote bb = new Wiimote();
        static Wiimote wm = new Wiimote();

        static ConnectionManager BalanceCM = null;
        static ConnectionManager WiimoteCM = null;

        static Timer BoardTimer = null;
        static Timer WiiMoteTimer = null;

        static float[] History = new float[100];
        static float threshold;
        static bool wentUp;

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            f = new WiiBalanceScaleForm();

            ConnectBalanceBoard();
            ConnectWiimote();

            if (f == null) return; //connecting required application restart, end this process here

            BoardTimer = new System.Windows.Forms.Timer();
            BoardTimer.Interval = 50;
            BoardTimer.Tick += new System.EventHandler(BoardTimer_Tick);
            BoardTimer.Start();

            WiiMoteTimer = new System.Windows.Forms.Timer();
            WiiMoteTimer.Interval = 50;
            WiiMoteTimer.Tick += new System.EventHandler(WiiMoteTimer_Tick);
            WiiMoteTimer.Start();

            Application.Run(f);
            Shutdown();
        }

        static void Shutdown()
        {
            if (WiiMoteTimer != null) { WiiMoteTimer.Stop(); WiiMoteTimer = null; }
            if (BoardTimer != null) { BoardTimer.Stop(); BoardTimer = null; }
            if (BalanceCM != null) { BalanceCM.Cancel(); BalanceCM = null; }
            if (WiimoteCM != null) { WiimoteCM.Cancel(); WiimoteCM = null; }
            if (f != null) { if (f.Visible) f.Close(); f = null; }
        }

        static void ConnectWiimote()
        {
            // bool bbConnected = true; try { bb = new Wiimote(); bb.Connect(); bb.SetLEDs(1); bb.GetStatus(); } catch { bbConnected = false; }
            f.connectingLabel.Text = "PRESS SYNC ON WII MOTE";
            wmConnected = true; try { wm.Connect(); wm.SetLEDs(false, true, true, false); wm.SetReportType(InputReport.IRAccel, true); } catch { wmConnected = false; }

            if (!wmConnected) 
            {
                if (ConnectionManager.ElevateProcessNeedRestart()) { Shutdown(); return; }
                if (WiimoteCM == null) WiimoteCM = new ConnectionManager();
                WiimoteCM.ConnectNextWiiMote();
                return;
            }
            if (WiimoteCM != null) { WiimoteCM.Cancel(); WiimoteCM = null; }

            f.Refresh();
        }

        static void ConnectBalanceBoard()
        {
            f.connectingLabel.Text = "PRESS SYNC on balnce board";
            
            if (bb.WiimoteState.ExtensionType == ExtensionType.BalanceBoard)
            {
                bb = wm;
                bb.SetLEDs(1);
                bb.GetStatus();
                bbConnected = true;
            }

            if(!bbConnected)
            {
                if (ConnectionManager.ElevateProcessNeedRestart()) { Shutdown(); return; }
                if (BalanceCM == null) BalanceCM = new ConnectionManager();
                BalanceCM.ConnectNextWiiMote();
                return;
            }
            if (BalanceCM != null) { BalanceCM.Cancel(); BalanceCM = null; }

            f.Refresh();
        }

        static void getWeight()
        {
            float kg = bb.WiimoteState.BalanceBoardState.WeightKg;
            threshold = (kg * 2.0F) + 5;  // Proportional to user's weight.
        }

        static void BoardTimer_Tick(object sender, System.EventArgs e)
        {
            int jumpCounter = Int32.Parse(f.jumpCounter.Text);

            if (BalanceCM != null)
            {
                if (BalanceCM.IsRunning())
                {
                    f.connectingLabel.Visible = true;

                    return;
                }
                if (BalanceCM.HadError())
                {
                    BoardTimer.Stop();
                    System.Windows.Forms.MessageBox.Show(f, "No compatible bluetooth adapter found - Quitting", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Shutdown();
                    return;
                }
                ConnectBalanceBoard();
                return;
            }

            f.connectingLabel.Visible = false;

            System.Drawing.Point topThreshold = new System.Drawing.Point(350, 200);
            System.Drawing.Point bottomThreshold = new System.Drawing.Point(350, 250);
            System.Drawing.Point loc = f.jumpMan.Location;

            int center = (f.Width / 2) - (f.jumpMan.Size.Width / 2);

            if (wentUp && loc.Y >= topThreshold.Y)
            {
                f.jumpMan.Location = new System.Drawing.Point(center, f.jumpMan.Location.Y - 5);
            }
            else if (!wentUp && loc.Y <= bottomThreshold.Y)
            {
                f.jumpMan.Location = new System.Drawing.Point(center, f.jumpMan.Location.Y + 10);
            }

            getWeight();

            //TopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft,
            
            float topLeft = bb.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
            float topRight = bb.WiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
            float bottomLeft = bb.WiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;
            float bottomRight = bb.WiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;

            // Keep values above 0.
            if (topLeft < 0) topLeft = 0;
            if (topRight < 0) topRight = 0;
            if (bottomLeft < 0) bottomLeft = 0;
            if (bottomRight < 0) bottomRight = 0;


            float topWeight = topLeft + topRight;
            float bottomWeight = bottomLeft + bottomRight;
            float difference = topWeight - bottomWeight;

            if (difference >= threshold)
            {
                wentUp = true;
            }

            if (difference <= threshold && wentUp == true)
            {
                jumpCounter++;
                f.jumpCounter.Text = jumpCounter.ToString();
                wentUp = false;
            }
        }

        static void WiiMoteTimer_Tick(object sender, System.EventArgs e)
        {
            if (WiimoteCM != null)
            {
                if (WiimoteCM.IsRunning())
                {
                    f.connectingLabel.Visible = true;

                    return;
                }
                if (WiimoteCM.HadError())
                {
                    BoardTimer.Stop();
                    System.Windows.Forms.MessageBox.Show(f, "No compatible bluetooth adapter found - Quitting", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Shutdown();
                    return;
                }
                ConnectWiimote();
                return;
            }

            bool buttonA = wm.WiimoteState.ButtonState.B;

            if (buttonA)
            {
                f.jumpMan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(141)))), ((int)(((byte)(48)))));
            }
            else
            {
                f.jumpMan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(82)))), ((int)(((byte)(255)))));
            }
        }
    }
}


