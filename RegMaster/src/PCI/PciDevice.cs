namespace RegMaster
{
    public class PciDevice
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public Dictionary<(uint, uint), string> SubDevices { get; set; } = new();
    }
}
