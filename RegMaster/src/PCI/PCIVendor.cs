namespace RegMaster
{
    public class PciVendor
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public Dictionary<uint, PciDevice> Devices { get; set; } = new();
    }
}
