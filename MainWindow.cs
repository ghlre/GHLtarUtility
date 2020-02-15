using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.WinUsb;
using Nefarius.ViGEm.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace GHLtarUtility
{
    public partial class MainWindow : Form
    {
        ViGEmClient client;
        BluetoothLEAdvertisementWatcher watcher = new BluetoothLEAdvertisementWatcher();

        List<PS3Guitar> PS3Guitars = new List<PS3Guitar>();
        List<iOSGuitar> iOSGuitars = new List<iOSGuitar>();

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
            watcher.Stop();
            foreach (iOSGuitar guitar in iOSGuitars) guitar.destroy();
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

        private void UpdateiOSDisplay()
        {
            if (iOSGuitars.Count >= 1)
            {
                iOSP1Panel.BackColor = Color.LimeGreen;
                iOSP1Label.Text = "Connected!";
                iOSP1Disconnect.Enabled = true;

                // Update indicators
                try
                {
                    switch (iOSGuitars[0].controller.UserIndex)
                    {
                        case 0: iOSP1Indicator.Image = Properties.Resources.player1; break;
                        case 1: iOSP1Indicator.Image = Properties.Resources.player2; break;
                        case 2: iOSP1Indicator.Image = Properties.Resources.player3; break;
                        case 3: iOSP1Indicator.Image = Properties.Resources.player4; break;
                        default: iOSP1Indicator.Image = Properties.Resources.player0; break;
                    }
                }
                catch (Exception)
                {
                    iOSP1Indicator.Image = Properties.Resources.player0;
                }
            }
            else
            {
                iOSP1Panel.BackColor = Color.LightGray;
                iOSP1Label.Text = "Not Connected";
                iOSP1Disconnect.Enabled = false;
                iOSP1Indicator.Image = Properties.Resources.player0;
            }

            if (iOSGuitars.Count >= 2)
            {
                iOSP2Panel.BackColor = Color.LimeGreen;
                iOSP2Label.Text = "Connected!";
                iOSP2Disconnect.Enabled = true;

                // Update indicators
                try
                {
                    switch (iOSGuitars[1].controller.UserIndex)
                    {
                        case 0: iOSP2Indicator.Image = Properties.Resources.player1; break;
                        case 1: iOSP2Indicator.Image = Properties.Resources.player2; break;
                        case 2: iOSP2Indicator.Image = Properties.Resources.player3; break;
                        case 3: iOSP2Indicator.Image = Properties.Resources.player4; break;
                        default: iOSP2Indicator.Image = Properties.Resources.player0; break;
                    }
                }
                catch (Exception)
                {
                    iOSP2Indicator.Image = Properties.Resources.player0;
                }
            }
            else
            {
                iOSP2Panel.BackColor = Color.LightGray;
                iOSP2Label.Text = "Not Connected";
                iOSP2Disconnect.Enabled = false;
                iOSP2Indicator.Image = Properties.Resources.player0;
            }

            if (iOSGuitars.Count >= 3)
            {
                iOSP3Panel.BackColor = Color.LimeGreen;
                iOSP3Label.Text = "Connected!";
                iOSP3Disconnect.Enabled = true;

                // Update indicators
                try
                {
                    switch (iOSGuitars[2].controller.UserIndex)
                    {
                        case 0: iOSP3Indicator.Image = Properties.Resources.player1; break;
                        case 1: iOSP3Indicator.Image = Properties.Resources.player2; break;
                        case 2: iOSP3Indicator.Image = Properties.Resources.player3; break;
                        case 3: iOSP3Indicator.Image = Properties.Resources.player4; break;
                        default: iOSP3Indicator.Image = Properties.Resources.player0; break;
                    }
                }
                catch (Exception)
                {
                    iOSP3Indicator.Image = Properties.Resources.player0;
                }
            }
            else
            {
                iOSP3Panel.BackColor = Color.LightGray;
                iOSP3Label.Text = "Not Connected";
                iOSP3Disconnect.Enabled = false;
                iOSP3Indicator.Image = Properties.Resources.player0;
            }

            if (iOSGuitars.Count >= 4)
            {
                iOSP4Panel.BackColor = Color.LimeGreen;
                iOSP4Label.Text = "Connected!";
                iOSP4Disconnect.Enabled = true;

                // Update indicators
                try
                {
                    switch (iOSGuitars[3].controller.UserIndex)
                    {
                        case 0: iOSP4Indicator.Image = Properties.Resources.player1; break;
                        case 1: iOSP4Indicator.Image = Properties.Resources.player2; break;
                        case 2: iOSP4Indicator.Image = Properties.Resources.player3; break;
                        case 3: iOSP4Indicator.Image = Properties.Resources.player4; break;
                        default: iOSP4Indicator.Image = Properties.Resources.player0; break;
                    }
                }
                catch (Exception)
                {
                    iOSP4Indicator.Image = Properties.Resources.player0;
                }
            }
            else
            {
                iOSP4Panel.BackColor = Color.LightGray;
                iOSP4Label.Text = "Not Connected";
                iOSP4Disconnect.Enabled = false;
                iOSP4Indicator.Image = Properties.Resources.player0;
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            watcher.Received += OnBLEAdvertisement;
            iOSSearching.Checked = true;
            DisplayTimer_Tick(sender, e);
            
        }

        async private void OnBLEAdvertisement(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            if (eventArgs.Advertisement.LocalName.Contains("Ble Guitar"))
            {
                BluetoothLEDevice guitar = await BluetoothLEDevice.FromBluetoothAddressAsync(eventArgs.BluetoothAddress);
                iOSGuitar newGuitar = new iOSGuitar(guitar, client.CreateXbox360Controller());
                iOSGuitars.Add(newGuitar);
            }
        }

        private void iOSSearching_CheckedChanged(object sender, EventArgs e)
        {
            if (iOSSearching.Checked) watcher.Start();
            if (!iOSSearching.Checked) watcher.Stop();
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            foreach (iOSGuitar guitar in iOSGuitars.ToList())
            {
                if (guitar.isDisconnected)
                {
                    iOSGuitars.Remove(guitar);
                }
            }


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

            UpdateiOSDisplay();
            UpdatePS3Display();
        }

        private void iOSDisconnect_Click(object sender, EventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "iOSP1Disconnect":
                    if (iOSGuitars.Count >= 1) iOSGuitars[0].destroy(); break;
                case "iOSP2Disconnect":
                    if (iOSGuitars.Count >= 2) iOSGuitars[1].destroy(); break;
                case "iOSP3Disconnect":
                    if (iOSGuitars.Count >= 3) iOSGuitars[2].destroy(); break;
                case "iOSP4Disconnect":
                    if (iOSGuitars.Count >= 4) iOSGuitars[3].destroy(); break;
            }
            MessageBox.Show("iOS guitars don't power off yet. Please disconnect the batteries to fully turn off your guitar, or wait for the guitar to time out.", "iOS Guitars", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
