using LibUsbDotNet;
using LibUsbDotNet.Main;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GHLtarUtility
{
    class PS3Guitar
    {
        public UsbDevice device;
        public IXbox360Controller controller;
        private Timer runTimer;
        System.Threading.Thread t;
        private bool shouldStop;

        public PS3Guitar(UsbDevice dongle, IXbox360Controller newController)
        {
            device = dongle;
            controller = newController;

            // Timer to send control packets
            runTimer = new Timer(10000);
            runTimer.Elapsed += sendControlPacket;
            runTimer.Start();

            // Thread to constantly read inputs
            t = new System.Threading.Thread(new System.Threading.ThreadStart(updateRoutine));
            t.Start();

            controller.Connect();
        }

        public bool isReadable()
        {
            // If device isn't open (closes itself), assume disconnected.
            if (!device.IsOpen) return false;
            if (!device.UsbRegistryInfo.IsAlive) return false;
            return true;
        }

        public void updateRoutine()
        {
            while (!shouldStop)
            {
                // Read 27 bytes from the guitar
                int bytesRead;
                byte[] readBuffer = new byte[27];
                var reader = device.OpenEndpointReader(ReadEndpointID.Ep01);
                reader.Read(readBuffer, 100, out bytesRead);

                // Set the fret inputs on the virtual 360 controller
                byte frets = readBuffer[0];
                controller.SetButtonState(Xbox360Button.A, (frets & 0x02) != 0x00); // B1
                controller.SetButtonState(Xbox360Button.B, (frets & 0x04) != 0x00); // B2
                controller.SetButtonState(Xbox360Button.Y, (frets & 0x08) != 0x00); // B3
                controller.SetButtonState(Xbox360Button.X, (frets & 0x01) != 0x00); // W1
                controller.SetButtonState(Xbox360Button.LeftShoulder, (frets & 0x10) != 0x00); // W2
                controller.SetButtonState(Xbox360Button.RightShoulder, (frets & 0x20) != 0x00); // W3

                // Set the strum bar values - can probably be more efficient but eh
                byte strum = readBuffer[4];
                if (strum == 0xFF)
                {
                    // Strum Down
                    controller.SetButtonState(Xbox360Button.Down, true);
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, -32768);
                    controller.SetButtonState(Xbox360Button.Up, false);
                }
                else if (strum == 0x00)
                {
                    // Strum Up
                    controller.SetButtonState(Xbox360Button.Down, false);
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, 32767);
                    controller.SetButtonState(Xbox360Button.Up, true);
                }
                else
                {
                    // No Strum
                    controller.SetButtonState(Xbox360Button.Down, false);
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                    controller.SetButtonState(Xbox360Button.Up, false);
                }

                // Set the buttons (pause/HP only for now)
                byte buttons = readBuffer[1];
                controller.SetButtonState(Xbox360Button.Start, (buttons & 0x02) != 0x00); // Pause
                controller.SetButtonState(Xbox360Button.Back, (buttons & 0x01) != 0x00); // Hero Power
                controller.SetButtonState(Xbox360Button.LeftThumb, (buttons & 0x04) != 0x00); // GHTV Button
                controller.SetButtonState(Xbox360Button.Guide, (buttons & 0x10) != 0x00); // Sync Button

                // Set the tilt and whammy
                controller.SetAxisValue(Xbox360Axis.RightThumbY, (short)((readBuffer[6] - 0x80) * 0x102));
                controller.SetAxisValue(Xbox360Axis.RightThumbX, (short)((readBuffer[19] - 0x80) * 0x102));

                // TODO: Proper D-Pad emulation
            }
        }

        public void sendControlPacket(Object source, ElapsedEventArgs e)
        {
            // Send the control packet (this is what keeps strumming alive)
            byte[] buffer = new byte[9] { 0x02, 0x08, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int bytesWrote;
            UsbSetupPacket setupPacket = new UsbSetupPacket(0x21, 0x09, 0x0201, 0x0000, 0x0008);
            device.ControlTransfer(ref setupPacket, buffer, 0x0008, out bytesWrote);
        }

        public void destroy()
        {
            // Destroy EVERYTHING.
            shouldStop = true;
            try { controller.Disconnect(); } catch (Exception) { }
            runTimer.Stop();
            runTimer.Dispose();
            t.Abort();
            device.Close();
        }
    }
}
