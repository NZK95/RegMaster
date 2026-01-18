using System.Globalization;
using System.Windows;

namespace RegMaster
{
    public partial class MainWindow
    {
        private async void LoadParser(object sender, RoutedEventArgs e)
        {
            if (!await _pciIdsParser.LoadData())
                MessageBox.Show("Unable to load PCI Ids", "PCI functionality may not work.", MessageBoxButton.OK, MessageBoxImage.Warning);

            foreach (var pci in EnumeratePCIDevices())
            {
                _pciDevices.Add(pci);
            }
        }

        public IEnumerable<PCIDevice> EnumeratePCIDevices()
        {
            var foundDevices = new HashSet<string>();
            var foundDevicesNames = new HashSet<string>();

            for (int bus = 0; bus < 256; bus++)
            {
                for (int device = 0; device < 32; device++)
                {
                    (var flowControl, var vendorCheck) = SkipInvalidDevices((byte)bus, (byte)device);
                    if (!flowControl)
                        continue;

                    for (int function = 0; function < GetMaxFunctions(vendorCheck); function++)
                    {
                        var data = PCIReader.ReadDword(bus.ToString(), device.ToString(), function.ToString(), 0x00).Replace("0x", "").Replace("0X", "");
                        if (data == "N/A")
                            continue;

                        var dataHex = uint.Parse(data, NumberStyles.HexNumber);

                        var vid = dataHex & 0xFFFF;
                        var did = (dataHex >> 16) & 0xFFFF;

                        if (vid == 0xFFFF || vid == 0x0000)
                            continue;

                        var deviceKey = $"{bus:X2}:{device:X2}:{function:X2}";
                        if (foundDevices.Contains(deviceKey))
                            continue;

                        foundDevices.Add(deviceKey);

                        var pciDevice = PCIReader.ReadCompleteDeviceInfo((byte)bus, (byte)device, (byte)function, vid, did, _pciIdsParser);

                        yield return pciDevice;
                    }
                }
            }
        }

        private byte GetMaxFunctions(uint vendorCheck)
        {
            byte headerType = (byte)((vendorCheck >> 16) & 0xFF);
            bool isMultiFunction = (headerType & 0x80) != 0;

            return isMultiFunction ? (byte)8 : (byte)1;
        }

        private (bool flowControl, uint value) SkipInvalidDevices(byte bus, byte device)
        {
            var data = PCIReader.ReadDword(bus.ToString(), device.ToString(), "0", 0x00).Replace("0x", "").Replace("0X", "");
            if (data == "N/A")
                return (flowControl: false, value: default);

            var dataHex = uint.Parse(data, NumberStyles.HexNumber);
            var vendorId = dataHex & 0xFFFF;

            if (vendorId == 0xFFFF || vendorId == 0x0000)
                return (flowControl: false, value: default);

            return (flowControl: true, value: default);
        }
    }
}
