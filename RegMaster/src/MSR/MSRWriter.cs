using System.Runtime.InteropServices;

namespace RegMaster
{
    internal class MSRWriter
    {
        [DllImport("WinRing0x64.dll")]
        private static extern bool RdmsrTx(ulong index, out uint eax, out uint edx, uint threadAffinityMask);

        [DllImport("WinRing0x64.dll")]
        private static extern bool WrmsrTx(ulong index, uint eax, uint edx, uint threadAffinityMask);

        public static bool WriteBit(ulong address, bool value, int bit, string cores)
        {
            int core = cores == "all" ? 0 : Convert.ToInt32(cores.Split(',')[0]);

            if (!RdmsrTx(address, out uint eax, out uint edx, (uint)(1 << core)))
                return false;

            ulong full = ((ulong)edx << 32) | eax;

            if (value)
                full |= (1UL << bit);
            else
                full &= ~(1UL << bit);

            eax = (uint)(full & 0xFFFFFFFF);
            edx = (uint)(full >> 32);

            if (cores == "all")
                return WriteForAllCores(address, eax, edx);
            else
                return WriteForSpecificCores(address, eax, edx, cores);
        }

        public static bool WriteFull(ulong address, ulong value, string cores)
        {
            uint eax = (uint)(value & 0xFFFFFFFF);
            uint edx = (uint)(value >> 32);

            if (cores == "all")
                return WriteForAllCores(address, eax, edx);
            else
                return WriteForSpecificCores(address, eax, edx, cores);
        }

        public static bool WriteBits(ulong address, ulong value, int start, int end, string cores)
        {
            bool valueBoolean = value == 1;

            for (int i = start; i <= end; ++i)
                if (!WriteBit(address, valueBoolean, i, cores))
                    return false;

            return true;
        }

        private static bool WriteForAllCores(ulong address, uint eax, uint edx)
        {
            int coresCount = Environment.ProcessorCount;

            for (int i = 0; i < coresCount; i++)
            {
                uint mask = (uint)(1 << i);
                if (!WrmsrTx(address, eax, edx, mask))
                    return false;
            }

            return true;
        }

        private static bool WriteForSpecificCores(ulong address, uint eax, uint edx, string cores)
        {
            var coresArray = cores.Split(',').Select(core => int.Parse(core.Trim())).ToArray();

            foreach (var core in coresArray)
            {
                uint mask = (uint)(1 << core);
                if (!WrmsrTx(address, eax, edx, mask))
                    return false;
            }

            return true;
        }
    }
}
