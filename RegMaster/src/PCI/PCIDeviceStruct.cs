namespace RegMaster
{
    public class PCIDevice
    {
        public byte Bus { get; set; }
        public byte Slot { get; set; }
        public byte Function { get; set; }

        public uint VendorId { get; set; }
        public uint DeviceId { get; set; }

        public string VendorName { get; set; }
        public string DeviceName { get; set; }

        public ushort Command { get; set; }
        public ushort Status { get; set; }
        public byte RevisionId { get; set; }
        public uint ClassCode { get; set; }

        public byte CacheLineSize { get; set; }
        public byte LatencyTimer { get; set; }
        public byte HeaderType { get; set; }
        public byte BIST { get; set; }

        public uint SubsystemVendorId { get; set; }
        public uint SubsystemDeviceId { get; set; }
        public string SubsystemVendorName { get; set; }
        public string SubsystemDeviceName { get; set; }

        public byte InterruptLine { get; set; }
        public byte InterruptPin { get; set; }

        public bool IsMultiFunction { get; set; }
        public string DisplayName => $"Bus {Bus:X2}, Slot {Slot:X2}, Function {Function:X2} - {VendorName} {DeviceName}";
        public string BdfAddress => $"{Bus:X2}:{Slot:X2}:{Function}";

        public PCIBaseAddressRegister[] BaseAddressRegisters { get; set; } = new PCIBaseAddressRegister[6];

        public string GetDetailedInfo()
        {
            var stringBars = string.Empty;

            foreach (var bar in BaseAddressRegisters)
            {
                if(bar is not null)
                    stringBars += bar.ToString() + "\n";
            }

            return $"""
                Bus: 0x{Bus:X2}
                Slot: 0x{Slot:X2}
                Function: 0x{Function:X2}
                BDF: {BdfAddress}
                
                Vendor ID: 0x{VendorId:X4} - {VendorName}
                Device ID: 0x{DeviceId:X4} - {DeviceName}
                
                Revision: 0x{RevisionId:X2}
                Class Code: 0x{ClassCode:X6}
                Command: 0x{Command:X4}
                Status: 0x{Status:X4}
                
                Subsystem Vendor: 0x{SubsystemVendorId:X4} - {SubsystemVendorName}
                Subsystem Device: 0x{SubsystemDeviceId:X4} - {SubsystemDeviceName}
                
                Interrupt Line: {InterruptLine}
                Interrupt Pin: {InterruptPin}
                
                Cache Line Size: {CacheLineSize}
                Latency Timer: {LatencyTimer}
                Header Type: 0x{HeaderType:X2}
                BIST: 0x{BIST:X2}

                {stringBars}
                """;
        }
    }

    public class PCIBaseAddressRegister
    {
        public uint Index { get; set; }
        public uint RawValue { get; set; }
        public bool IsIOSpace { get; set; }
        public bool IsPrefetchable { get; set; }
        public ulong BaseAddress { get; set; }
        public uint Size { get; set; }
        public string TypeName => IsIOSpace ? "I/O Port" : "Memory";
        public string PrefetchableName => IsPrefetchable ? "Prefetchable" : "Non-prefetchable";

        public override string ToString() =>
            $"BAR{Index}: 0x{BaseAddress:X8} ({TypeName}, {PrefetchableName}) Size: 0x{Size:X}";
    }
}
