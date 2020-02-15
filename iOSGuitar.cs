using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using Windows.Storage.Streams;

namespace GHLtarUtility
{
    class iOSGuitar
    {
        public BluetoothLEDevice device;
        public IXbox360Controller controller;
        public bool isDisconnected;
        public GattCharacteristic optionCharacteristic;

        public iOSGuitar(BluetoothLEDevice guitar, IXbox360Controller newController)
        {
            device = guitar;
            controller = newController;
            prepareGuitar();
        }

        async void prepareGuitar()
        {
            device.ConnectionStatusChanged += Device_ConnectionStatusChanged;
            GattDeviceServicesResult services = await device.GetGattServicesAsync();
            foreach (GattDeviceService service in services.Services)
            {
                GattCharacteristicsResult characteristics = await service.GetCharacteristicsAsync();
                foreach (GattCharacteristic characteristic in characteristics.Characteristics)
                {
                    if (characteristic.Uuid.ToString().Equals("533e1524-3abe-f33f-cd00-594e8b0a8ea3"))
                    {
                        controller.Connect();
                        optionCharacteristic = characteristic;
                        GattCommunicationStatus status = await optionCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                        optionCharacteristic.ValueChanged += Characteristic_ValueChanged;
                    }
                }
            }
        }

        void Device_ConnectionStatusChanged(BluetoothLEDevice device, object b)
        {
            if (device.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
            {
                Console.WriteLine("destroy time");
                destroy();
                isDisconnected = true;
            }
        }

        void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            // Read 20 bytes from the characteristic
            IBuffer buffer = args.CharacteristicValue;
            var dataReader = DataReader.FromBuffer(buffer);
            byte[] readBuffer = new byte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                readBuffer[i] = dataReader.ReadByte();
            }

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
                controller.SetButtonState(Xbox360Button.Up, false);
            }
            else if (strum == 0x00)
            {
                // Strum Up
                controller.SetButtonState(Xbox360Button.Down, false);
                controller.SetButtonState(Xbox360Button.Up, true);
            }
            else
            {
                // No Strum
                controller.SetButtonState(Xbox360Button.Down, false);
                controller.SetButtonState(Xbox360Button.Up, false);
            }

            // Set the buttons (pause/HP only for now)
            byte buttons = readBuffer[1];
            controller.SetButtonState(Xbox360Button.Start, (buttons & 0x02) != 0x00); // Pause
            controller.SetButtonState(Xbox360Button.Back, (buttons & 0x08) != 0x00); // Hero Power

            // TODO: Proper D-Pad and Whammy/Tilt emulation
        }

        public void destroy()
        {
            // Destroy EVERYTHING.
            device.ConnectionStatusChanged -= Device_ConnectionStatusChanged;
            try { controller.Disconnect(); } catch (Exception) { }
            device.Dispose();
            isDisconnected = true;
        }
    }
}
