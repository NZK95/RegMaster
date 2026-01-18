using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows;

namespace RegMaster.src.MMIO
{
    internal static class MMIOWriter
    {
        [DllImport(@"inpoutx64.dll")]
        private static extern IntPtr MapPhysToLin(ulong addr, uint size, out IntPtr handle);

        [DllImport(@"inpoutx64.dll")]
        private static extern bool UnmapPhysicalMemory(IntPtr handle, IntPtr addr);

        public static bool WriteMemFull(ulong address, uint size, ulong value)
        {
            IntPtr handle = IntPtr.Zero;
            IntPtr mappedAddress = MapPhysToLin(address, (size / 8), out handle);

            try
            {
                if (mappedAddress == IntPtr.Zero)
                    return false;

                byte[] data = BitConverter.GetBytes(value);
                Marshal.Copy(data, 0, mappedAddress, (int)(size / 8));

                UnmapPhysicalMemory(handle, mappedAddress);
                return true;
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                UnmapPhysicalMemory(handle, mappedAddress);
                return false;
            }
        }

        public static bool WriteMembit(ulong address, uint bitPosition, bool value)
        {
            try
            {
                uint size = 0;

                if (bitPosition < 8)
                    size = 8;
                else if (bitPosition < 16)
                    size = 16;
                else if (bitPosition < 32)
                    size = 32;
                else if (bitPosition < 64)
                    size = 64;

                var currentValue = Convert.ToUInt64(MMIOReader.ReadMem(address, size), 16);

                if (value)
                    currentValue |= (1UL << (int)bitPosition);
                else
                    currentValue &= ~(1UL << (int)bitPosition);

                return WriteMemFull(address, size, currentValue);
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }

        public static bool WriteBits(ulong address, uint start, uint end, bool value)
        {
            try
            {
                if (start > end || end >= 64)
                    return false;

                uint size;

                if (end < 8)
                    size = 8;
                else if (end < 16)
                    size = 16;
                else if (end < 32)
                    size = 32;
                else
                    size = 64;

                ulong currentValue = Convert.ToUInt64(MMIOReader.ReadMem(address, size), 16);
                ulong mask = ((1UL << (int)(end - start + 1)) - 1) << (int)start;

                if (value)
                    currentValue |= mask;
                else
                    currentValue &= ~mask;

                return WriteMemFull(address, size, currentValue);
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }
    }
}
