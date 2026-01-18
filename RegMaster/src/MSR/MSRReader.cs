using System.Runtime.InteropServices;

namespace RegMaster
{
    internal static class MSRReader
    {
        [DllImport("WinRing0x64.dll")]
        private static extern bool RdmsrTx(
        ulong index,
        out uint eax,
        out uint edx,
        uint threadAffinityMask);

        public static string GetBinary(ulong address, int core, string bit = "N/A")
        {
            if (!RdmsrTx(address, out uint eax, out uint edx, (uint)(1 << core)))
                return "N/A";

            if (bit == "N/A")
            {
                var edxBin = Convert.ToString(edx, 2).PadLeft(32, '0');
                var eaxBin = Convert.ToString(eax, 2).PadLeft(32, '0');

                return $"{edxBin} | {eaxBin}";
            }

            var full = ((ulong)edx << 32) | eax;

            if (bit.Contains('-'))
            {
                var parts = bit.Split('-');
                var start = int.Parse(parts[0]);
                var end = int.Parse(parts[1]);
                var count = end - start + 1;

                var mask = (1UL << count) - 1;
                var bits = (full >> start) & mask;

                return Convert.ToString((long)bits, 2).PadLeft(count, '0');
            }
            else
            {
                var bitNumeric = int.Parse(bit);
                var bits = (full >> bitNumeric) & 1;

                return bits.ToString();
            }
        }

        public static string GetHex(ulong address, int core, string bit = "N/A")
        {
            if (!RdmsrTx(address, out uint eax, out uint edx, (uint)(1 << core)))
                return "N/A";

            if (bit == "N/A")
                return $"0x{edx:X8} | 0x{eax:X8}";

            var full = ((ulong)edx << 32) | eax;

            if (bit.Contains('-'))
            {
                var parts = bit.Split('-');
                var start = int.Parse(parts[0]);
                var end = int.Parse(parts[1]);
                var count = end - start + 1;

                var mask = (1UL << count) - 1;
                var bits = (full >> start) & mask;

                return $"0x{bits:X}";
            }
            else
            {
                var bitNumeric = int.Parse(bit);
                var bits = (full >> bitNumeric) & 1;

                return $"0x{bits:X}";
            }
        }

        public static string GetDecimal(ulong address, int core, string bit = "N/A")
        {
            if (!RdmsrTx(address, out uint eax, out uint edx, (uint)(1 << core)))
                return "N/A";

            if (bit == "N/A")
                return $"{edx} | {eax}";

            var full = ((ulong)edx << 32) | eax;

            if (bit.Contains('-'))
            {
                var parts = bit.Split('-');
                var start = int.Parse(parts[0]);
                var end = int.Parse(parts[1]);
                var count = end - start + 1;

                var mask = (1UL << count) - 1;
                var bits = (full >> start) & mask;

                return bits.ToString();
            }
            else
            {
                var bitNumeric = int.Parse(bit);
                var bits = (full >> bitNumeric) & 1;

                return bits.ToString();
            }
        }
    }
}
