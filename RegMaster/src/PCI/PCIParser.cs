using System.Globalization;
using System.Net.Http;

namespace RegMaster
{
    public class PciIdsParser
    {
        private const string PCI_IDS_URL = "https://pci-ids.ucw.cz/v2.2/pci.ids";
        private Dictionary<uint, PciVendor> vendors = new();
        private PciVendor currentVendor = null;
        private uint currentVendorId = 0;

        public async Task<bool> LoadData()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("RegMaster/1.0");
                var content = await client.GetStringAsync(PCI_IDS_URL);
                var list = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in list)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    if (!line.StartsWith("\t"))
                        ParseVendor(line);

                    else if (line.StartsWith("\t") && !line.StartsWith("\t\t"))
                        ParseDevice(line);

                    else if (line.StartsWith("\t\t"))
                        ParseSubDevice(line);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ParseVendor(string line)
        {
            try
            {
                var parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 1 || !uint.TryParse(parts[0], NumberStyles.HexNumber, null, out uint vendorId))
                    return;

                string vendorName = parts.Length > 1 ? parts[1].Trim() : "Unknown";

                currentVendorId = vendorId;
                currentVendor = new PciVendor
                {
                    Id = vendorId,
                    Name = vendorName,
                    Devices = new Dictionary<uint, PciDevice>()
                };

                vendors[vendorId] = currentVendor;
            }
            catch
            {
                return;
            }
        }

        private void ParseDevice(string line)
        {
            try
            {
                if (currentVendor == null)
                    return;

                var trimmed = line.TrimStart('\t');
                var parts = trimmed.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 1 || !uint.TryParse(parts[0], NumberStyles.HexNumber, null, out uint deviceId))
                    return;

                string deviceName = parts.Length > 1 ? parts[1].Trim() : "Unknown";

                var device = new PciDevice
                {
                    Id = deviceId,
                    Name = deviceName,
                    SubDevices = new Dictionary<(uint, uint), string>()
                };

                currentVendor.Devices[deviceId] = device;
            }
            catch
            {
                return;
            }
        }

        private void ParseSubDevice(string line)
        {
            try
            {
                if (currentVendor == null || currentVendor.Devices.Count == 0)
                    return;

                var trimmed = line.TrimStart('\t');
                var parts = trimmed.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 2)
                    return;

                if (!uint.TryParse(parts[0], NumberStyles.HexNumber, null, out uint subsysVendorId) ||
                    !uint.TryParse(parts[1], NumberStyles.HexNumber, null, out uint subsysDeviceId))
                    return;

                string subDeviceName = parts.Length > 2 ? parts[2].Trim() : "Unknown";

                var lastDevice = currentVendor.Devices.Values.Last();
                lastDevice.SubDevices[(subsysVendorId, subsysDeviceId)] = subDeviceName;
            }
            catch
            {
                return;
            }
        }

        public string GetVendorName(uint vendorId)
        {
            return vendors.ContainsKey(vendorId) ? vendors[vendorId].Name : "Unknown Vendor";
        }

        public string GetFullDeviceName(uint vendorId, uint deviceId)
        {
            string vendorName = GetVendorName(vendorId);
            string deviceName = GetDeviceName(vendorId, deviceId);
            return $"{vendorName} - {deviceName}";
        }

        public string GetDeviceName(uint vendorId, uint deviceId)
        {
            if (!vendors.ContainsKey(vendorId))
                return "Unknown Device";

            var vendor = vendors[vendorId];
            return vendor.Devices.ContainsKey(deviceId) ? vendor.Devices[deviceId].Name : "Unknown Device";
        }

        public string GetSubDeviceName(uint vendorId, uint deviceId, uint subsysVendorId, uint subsysDeviceId)
        {
            if (!vendors.ContainsKey(vendorId))
                return null;

            var vendor = vendors[vendorId];
            if (!vendor.Devices.ContainsKey(deviceId))
                return null;

            var device = vendor.Devices[deviceId];
            return device.SubDevices.ContainsKey((subsysVendorId, subsysDeviceId))
                ? device.SubDevices[(subsysVendorId, subsysDeviceId)]
                : null!;
        }

    }
}
