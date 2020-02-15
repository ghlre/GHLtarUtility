using LibUsbDotNet;
using LibUsbDotNet.Main;
using Nefarius.ViGEm.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GHLtarUtilityLite
{
    public partial class MainWindow : Form
    {
        ViGEmClient client;

        List<PS3Guitar> PS3Guitars = new List<PS3Guitar>();

        public MainWindow()
        {
            InitializeComponent();
            this.FormClosing += this_FormClosing;
            try
            {
                client = new ViGEmClient();
            } catch (Exception)
            {
                MessageBox.Show("You need to install the ViGEm Bus Driver to use this application.", "GHLtar Utility", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void this_FormClosing(object sender, EventArgs e)
        {
            foreach (PS3Guitar guitar in PS3Guitars) guitar.destroy();
        }

        private void UpdatePS3Display()
        {
            if (PS3Guitars.Count >= 1)
            {
                ps3P1Panel.BackColor = Color.LimeGreen;
                ps3P1Label.Text = "Connected!";

                // Update indicators
                try
                {
                    switch (PS3Guitars[0].controller.UserIndex)
                    {
                        case 0: ps3P1Indicator.Image = Properties.Resources.player1; break;
                        case 1: ps3P1Indicator.Image = Properties.Resources.player2; break;
                        case 2: ps3P1Indicator.Image = Properties.Resources.player3; break;
                        case 3: ps3P1Indicator.Image = Properties.Resources.player4; break;
                        default: ps3P1Indicator.Image = Properties.Resources.player0; break;
                    }
                } catch (Exception)
                {
                    ps3P1Indicator.Image = Properties.Resources.player0;
                }
            } else
            {
                ps3P1Panel.BackColor = Color.LightGray;
                ps3P1Label.Text = "Not Connected";
                ps3P1Indicator.Image = Properties.Resources.player0;
            }

            if (PS3Guitars.Count >= 2)
            {
                ps3P2Panel.BackColor = Color.LimeGreen;
                ps3P2Label.Text = "Connected!";

                // Update indicators
                try
                {
                    switch (PS3Guitars[1].controller.UserIndex)
                    {
                        case 0: ps3P2Indicator.Image = Properties.Resources.player1; break;
                        case 1: ps3P2Indicator.Image = Properties.Resources.player2; break;
                        case 2: ps3P2Indicator.Image = Properties.Resources.player3; break;
                        case 3: ps3P2Indicator.Image = Properties.Resources.player4; break;
                        default: ps3P2Indicator.Image = Properties.Resources.player0; break;
                    }
                }
                catch (Exception)
                {
                    ps3P2Indicator.Image = Properties.Resources.player0;
                }
            }
            else
            {
                ps3P2Panel.BackColor = Color.LightGray;
                ps3P2Label.Text = "Not Connected";
                ps3P2Indicator.Image = Properties.Resources.player0;
            }

            if (PS3Guitars.Count >= 3)
            {
                ps3P3Panel.BackColor = Color.LimeGreen;
                ps3P3Label.Text = "Connected!";

                // Update indicators
                try
                {
                    switch (PS3Guitars[2].controller.UserIndex)
                    {
                        case 0: ps3P3Indicator.Image = Properties.Resources.player1; break;
                        case 1: ps3P3Indicator.Image = Properties.Resources.player2; break;
                        case 2: ps3P3Indicator.Image = Properties.Resources.player3; break;
                        case 3: ps3P3Indicator.Image = Properties.Resources.player4; break;
                        default: ps3P3Indicator.Image = Properties.Resources.player0; break;
                    }
                }
                catch (Exception)
                {
                    ps3P3Indicator.Image = Properties.Resources.player0;
                }
            }
            else
            {
                ps3P3Panel.BackColor = Color.LightGray;
                ps3P3Label.Text = "Not Connected";
                ps3P3Indicator.Image = Properties.Resources.player0;
            }

            if (PS3Guitars.Count >= 4)
            {
                ps3P4Panel.BackColor = Color.LimeGreen;
                ps3P4Label.Text = "Connected!";

                // Update indicators
                try
                {
                    switch (PS3Guitars[3].controller.UserIndex)
                    {
                        case 0: ps3P4Indicator.Image = Properties.Resources.player1; break;
                        case 1: ps3P4Indicator.Image = Properties.Resources.player2; break;
                        case 2: ps3P4Indicator.Image = Properties.Resources.player3; break;
                        case 3: ps3P4Indicator.Image = Properties.Resources.player4; break;
                        default: ps3P4Indicator.Image = Properties.Resources.player0; break;
                    }
                }
                catch (Exception)
                {
                    ps3P4Indicator.Image = Properties.Resources.player0;
                }
            }
            else
            {
                ps3P4Panel.BackColor = Color.LightGray;
                ps3P4Label.Text = "Not Connected";
                ps3P4Indicator.Image = Properties.Resources.player0;
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            DisplayTimer_Tick(sender, e);
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            // Create list of devices to prevent re-attaching existing dongles.
            List<string> devices = new List<string>();

            foreach (PS3Guitar guitar in PS3Guitars.ToList())
            {
                // Remove any guitars that can't be found anymore
                if (!guitar.isReadable())
                {
                    guitar.destroy();
                    PS3Guitars.Remove(guitar);
                }
                else
                {
                    // Add guitars that are still found to the list of existing devices
                    devices.Add(guitar.device.DevicePath);
                }
            }

            // Enumerate through WinUSB devices and set those up if they are valid dongles.
            foreach (UsbRegistry device in LibUsbDotNet.UsbDevice.AllDevices)
            {
                // USB\VID_12BA&PID_074B is the ID of the PS3/Wii U dongle.
                if (device.Vid == 0x12BA && device.Pid == 0x074B)
                {
                    UsbDevice trueDevice;
                    device.Open(out trueDevice);
                    if (PS3Guitars.Count < 4 && trueDevice != null && !devices.Contains(trueDevice.DevicePath))
                    {
                        PS3Guitar newGuitar = new PS3Guitar(trueDevice, client.CreateXbox360Controller());
                        PS3Guitars.Add(newGuitar);
                    }
                }
            }
            UpdatePS3Display();
        }
    }
}
