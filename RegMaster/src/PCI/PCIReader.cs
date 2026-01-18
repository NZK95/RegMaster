using System.Globalization;
using System.Runtime.InteropServices;

namespace RegMaster
{
    internal static class PCIReader
    {
        [DllImport("WinRing0x64.dll")]
        private static extern uint ReadPciConfigDword(uint pciAddress, byte offset);

        [DllImport("WinRing0x64.dll")]
        private static extern uint ReadPciConfigByte(uint pciAddress, uint offset);

        [DllImport("WinRing0x64.dll")]
        private static extern uint ReadPciConfigWord(uint pciAddress, uint offset);

        public static string ReadDword(string bus, string device, string function, byte offset)
        {
            try
            {
                var busHex = uint.Parse(bus, NumberStyles.HexNumber);
                var deviceHex = uint.Parse(device, NumberStyles.HexNumber);
                var functionHex = uint.Parse(function, NumberStyles.HexNumber);

                var pciAddress = BuildPciAddress(busHex, deviceHex, functionHex);
                var value = ReadPciConfigDword(pciAddress, offset);

                return $"0x{value.ToString("X")}";
            }
            catch
            {
                return "N/A";
            }
        }

        public static string ReadWord(string bus, string device, string function, byte offset)
        {
            var busHex = uint.Parse(bus, NumberStyles.HexNumber);
            var deviceHex = uint.Parse(device, NumberStyles.HexNumber);
            var functionHex = uint.Parse(function, NumberStyles.HexNumber);

            var pciAddress = BuildPciAddress(busHex, deviceHex, functionHex);
            var value = ReadPciConfigWord(pciAddress, offset);

            return $"0x{value.ToString("X4")}";
        }

        public static string ReadByte(string bus, string device, string function, byte offset)
        {
            var busHex = uint.Parse(bus, NumberStyles.HexNumber);
            var deviceHex = uint.Parse(device, NumberStyles.HexNumber);
            var functionHex = uint.Parse(function, NumberStyles.HexNumber);

            var pciAddress = BuildPciAddress(busHex, deviceHex, functionHex);
            var value = ReadPciConfigByte(pciAddress, offset);

            return $"0x{value.ToString("X2")}";
        }

        private static uint BuildPciAddress(uint bus, uint device, uint function)
        {
            return (bus << 8) | (device << 3) | function;
        }

        public static PCIDevice ReadCompleteDeviceInfo(byte bus, byte device, byte function, uint vendorId, uint deviceId, PciIdsParser parser)
        {
            var pciDevice = new PCIDevice
            {
                Bus = bus,
                Slot = device,
                Function = function,
                VendorId = vendorId,
                DeviceId = deviceId,
                VendorName = parser.GetVendorName(vendorId),
                DeviceName = parser.GetDeviceName(vendorId, deviceId)
            };

            string data1Hex = ReadDword(bus.ToString("X2"), device.ToString("X2"), function.ToString(), 0x04).Replace("0x", "").Replace("0X", "");
            if (data1Hex != "N/A")
            {
                uint data1 = uint.Parse(data1Hex, NumberStyles.HexNumber);
                pciDevice.Command = (ushort)(data1 & 0xFFFF);
                pciDevice.Status = (ushort)((data1 >> 16) & 0xFFFF);
            }

            string data2Hex = ReadDword(bus.ToString("X2"), device.ToString("X2"), function.ToString(), 0x08).Replace("0x", "").Replace("0X", "");
            if (data2Hex != "N/A")
            {
                uint data2 = uint.Parse(data2Hex, NumberStyles.HexNumber);
                pciDevice.RevisionId = (byte)(data2 & 0xFF);
                pciDevice.ClassCode = data2 >> 8;
            }

            string data3Hex = ReadDword(bus.ToString("X2"), device.ToString("X2"), function.ToString(), 0x0C).Replace("0x", "").Replace("0X", "");
            if (data3Hex != "N/A")
            {
                uint data3 = uint.Parse(data3Hex, NumberStyles.HexNumber);
                pciDevice.CacheLineSize = (byte)(data3 & 0xFF);
                pciDevice.LatencyTimer = (byte)((data3 >> 8) & 0xFF);
                pciDevice.HeaderType = (byte)((data3 >> 16) & 0xFF);
                pciDevice.BIST = (byte)((data3 >> 24) & 0xFF);
                pciDevice.IsMultiFunction = (pciDevice.HeaderType & 0x80) != 0;
            }

            string data4Hex = ReadDword(bus.ToString("X2"), device.ToString("X2"), function.ToString(), 0x2C).Replace("0x", "").Replace("0X", "");
            if (data4Hex != "N/A")
            {
                uint data4 = uint.Parse(data4Hex, NumberStyles.HexNumber);
                pciDevice.SubsystemVendorId = (uint)(data4 & 0xFFFF);
                pciDevice.SubsystemDeviceId = (uint)((data4 >> 16) & 0xFFFF);
                pciDevice.SubsystemVendorName = parser.GetVendorName(pciDevice.SubsystemVendorId);
                pciDevice.SubsystemDeviceName = parser.GetDeviceName(pciDevice.SubsystemVendorId, pciDevice.SubsystemDeviceId);
            }

            string data5Hex = ReadDword(bus.ToString("X2"), device.ToString("X2"), function.ToString(), 0x3C).Replace("0x", "").Replace("0X", "");
            if (data5Hex != "N/A")
            {
                uint data5 = uint.Parse(data5Hex, NumberStyles.HexNumber);
                pciDevice.InterruptLine = (byte)(data5 & 0xFF);
                pciDevice.InterruptPin = (byte)((data5 >> 8) & 0xFF);
            }

            for (int i = 0; i < 6; i++)
            {
                byte barOffset = (byte)(0x10 + i * 4);
                string dataBarHex = ReadDword(bus.ToString("X2"), device.ToString("X2"), function.ToString(), barOffset).Replace("0x", "").Replace("0X", "");

                if (dataBarHex != "N/A")
                {
                    uint dataBar = uint.Parse(dataBarHex, NumberStyles.HexNumber);

                        bool isIOSpace = (dataBar & 1) == 1;
                        uint baseAddress = isIOSpace ? (dataBar & 0xFFFFFFFC) : (dataBar & 0xFFFFFFF0);

                        pciDevice.BaseAddressRegisters[i] = new PCIBaseAddressRegister
                        {
                            Index = (uint)i,
                            RawValue = dataBar,
                            IsIOSpace = isIOSpace,
                            BaseAddress = baseAddress,
                        };
                }
            }

            return pciDevice;
        }
    }
}