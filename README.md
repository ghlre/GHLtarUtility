# GHLtar Utility

Use a Guitar Hero Live PS3/Wii U dongle or iOS Bluetooth Guitar on your Windows PC by emulating an Xbox 360 controller.

Compiled releases are available [from the releases page](https://github.com/ghlre/GHLtarUtility/releases).

## GHLtar Utility Lite

The Lite version removes the ability to use iOS guitars as the Bluetooth LE APIs are only available in Windows 10.

## Requirements

- GHLtar Utility requires Windows 10 (build 10240) or higher
- GHLtar Utility Lite requires Windows 7 or higher
- .NET Framework: 4.6 for non-lite, 4.5.2 for lite.
- Bluetooth capabilities for iOS guitars

---

## Installation

### Program Installation

1. Download GHLtarUtility from [the releases page](https://github.com/ghlre/GHLtarUtility/releases) and extract the contents of the downloaded .zip into a new folder.
2. Download and install the ViGEmBus driver:
- Windows 10: [download the latest version](https://github.com/ViGEm/ViGEmBus/releases).
- Windows 8.1 or earlier: [download v1.16.116](https://github.com/ViGEm/ViGEmBus/releases/tag/setup-v1.16.116).

### PS3/Wii U Dongle Setup

1. [Download Zadig](https://zadig.akeo.ie/) and run it as administrator.
2. Click the Options tab, then click `List All Devices`.
3. For PS3/Wii U dongles, select the `Guitar Hero` device in the dropdown menu. For PS3 turntables, select `Guitar Hero5 for PlayStation (R) 3`.
    - If you need to verify you selected the right device, the PS3/Wii U GHL dongle has a USB ID of `12BA 074B`, and the PS3 turntable dongle has a USB ID of `12BA 0140`.
4. In the box the green or orange arrow is pointing to, make sure the driver listed is the `WinUSB` driver. If it is not, click the arrow buttons until it says `WinUSB`.
5. Click the `Install WCID Driver`/`Replace Driver` button, then close Zadig.
6. Launch GHLtarUtility (or GHLtarUtility Lite for non-Windows 10) and sync your guitar to the dongle.

### iOS Guitar Setup
1. Ensure that your Bluetooth is enabled on your PC.
2. Pair your iOS guitar in your Bluetooth settings.
3. Open GHLtarUtility and make sure the `Searching Mode` box is checked, then wait for it to connect your guitar.

---

## License
This project is licensed under the GNU General Public License v3.0. See [LICENSE](https://github.com/ghlre/GHLtarUtility/blob/master/LICENSE) for details.
