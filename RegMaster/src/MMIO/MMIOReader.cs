using System.Runtime.InteropServices;
using System.Windows;

namespace RegMaster
{
    internal static class MMIOReader
    {
        [DllImport(@"inpoutx64.dll")]
        private static extern IntPtr MapPhysToLin(ulong addr, uint size, out IntPtr handle);

        [DllImport(@"inpoutx64.dll")]
        private static extern bool UnmapPhysicalMemory(IntPtr handle, IntPtr addr);

        public static string ReadMem(ulong address, uint size)
        {
            IntPtr handle = IntPtr.Zero;
            IntPtr mappedAddress = MapPhysToLin(address, (size / 8), out handle);

            try
            {
                if (mappedAddress == IntPtr.Zero)
                    return "N/A";

                byte[] buffer = new byte[size / 8];
                Marshal.Copy(mappedAddress, buffer, 0, buffer.Length);

                string result;
                switch (size)
                {
                    case 8:
                        result = $"{buffer[0]:X2}";
                        break;
                    case 16:
                        ushort val16 = BitConverter.ToUInt16(buffer, 0);
                        result = $"{val16:X4}";
                        break;
                    case 32:
                        uint val32 = BitConverter.ToUInt32(buffer, 0);
                        result = $"{val32:X8}";
                        break;
                    case 64:
                        ulong val64 = BitConverter.ToUInt64(buffer, 0);
                        result = $"{val64:X16}";
                        break;
                    default:
                        result = "N/A";
                        break;
                }

                UnmapPhysicalMemory(handle, mappedAddress);
                return result;
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                UnmapPhysicalMemory(handle, mappedAddress);
                return "N/A";
            }
        }

        public static string ReadBit(ulong address, uint size, uint bitPosition)
        {
            if (bitPosition >= size )
                return "N/A";

            ulong value;

            switch (size)
            {
                case 8:
                    value = Convert.ToByte(ReadMem(address, size), 16);
                    break;

                case 16:
                    value = Convert.ToUInt16(ReadMem(address, size), 16);
                    break;

                case 32:
                    value = Convert.ToUInt32(ReadMem(address, size), 16);
                    break;

                case 64:
                    value = Convert.ToUInt64(ReadMem(address, size), 16);
                    break;

                default:
                    return "N/A";
            }

            return (((value >> (int)bitPosition) & 1UL) == 1) ? "1" : "0";
        }

        public static string ReadBits(ulong address, uint size, uint start, uint end)
        {
            if (start > end || end >= size)
                return "N/A";

            ulong value;
            switch (size)
            {
                case 8:
                    value = Convert.ToByte(ReadMem(address, size), 16);
                    break;

                case 16:
                    value = Convert.ToUInt16(ReadMem(address, size), 16);
                    break;

                case 32:
                    value = Convert.ToUInt32(ReadMem(address, size), 16);
                    break;

                case 64:
                    value = Convert.ToUInt64(ReadMem(address, size), 16);
                    break;

                default:
                    return "N/A";
            }

            ulong mask = ((1UL << (int)(end - start + 1)) - 1);
            ulong field = (value >> (int)start) & mask;
            return Convert.ToString((long)field, 2).PadLeft((int)(end - start + 1), '0');
        }
    }
}
