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

            ConnectBalanceBoard(false);
            if (form== null) return; //connecting required application restart, end this process here

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

        static void ConnectBalanceBoard(bool WasJustConnected)
        {
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
            float kg = balanceBoard.WiimoteState.BalanceBoardState.WeightKg;
            threshold = (kg * 2.0F) + 5;  // Proportional to user's weight.
        }

        static void BoardTimer_Tick(object sender, System.EventArgs e)
        {
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
                ConnectBalanceBoard(true);
                return;
            }

            System.Drawing.Point topThreshold = new System.Drawing.Point(350, 200);
            System.Drawing.Point bottomThreshold = new System.Drawing.Point(350, 250);
            System.Drawing.Point loc = form.jumpMan.Location;

            int center = (form.Width / 2) - (form.jumpMan.Size.Width / 2);

            if (wentUp && loc.Y >= topThreshold.Y)
            {
                form.jumpMan.Location = new System.Drawing.Point(center, form.jumpMan.Location.Y - 5);
            }
            else if (!wentUp && loc.Y <= bottomThreshold.Y)
            {
                form.jumpMan.Location = new System.Drawing.Point(center, form.jumpMan.Location.Y + 10);
            }

            getWeight();

            form.connectingLabel.Visible = false; // Don't display connecting label.

            //TopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft,
            float topLeft = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
            float topRight = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
            float bottomLeft = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;
            float bottomRight = balanceBoard.WiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;

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
                form.jumpCounter.Text = jumpCounter.ToString();
                wentUp = false;
            }
        }
    }
}


