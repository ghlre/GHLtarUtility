using LibUsbDotNet;
using LibUsbDotNet.Main;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;
using System.Timers;

namespace GHLtarUtilityLite
{
    abstract class PS3Peripheral
    {
        public UsbDevice device;
        public IXbox360Controller controller;

        public abstract bool isReadable();

        public abstract void destroy();
    }
}
