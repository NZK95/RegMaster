using System.Windows;

namespace RegMaster
{
    public partial class MainWindow
    {
        private bool _showMsrMessage = true;

        private void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ValueTextBox.Text) || string.IsNullOrEmpty(AddressTextBox.Text)
                || ValueTextBox.Text == "Hex value" || AddressTextBox.Text == "0xE2 e.g.")
            {
                MessageBox.Show("Missing input", "Please provide a valid address and value.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var address = Convert.ToUInt64(AddressTextBox.Text, 16);
                var value = Convert.ToUInt64(ValueTextBox.Text, 16);
                var bit = BitTextBox.Text;
                var cores = string.Empty;

                if (GetCountOfSelectedCores() == 0)
                    cores = "0";
                else
                    cores = GetCores();

                if (string.IsNullOrEmpty(bit) || bit == "15 or 0 - 10")
                    ElaborateNoBit(address, value, cores);

                else if (bit.Contains('-'))
                    ElaborateBitField(address, value, bit, cores);

                else
                    ElaborateSingleBit(address, value, bit, cores);

                _showMsrMessage = false;
                ReadButton_Click(sender, e);
                _showMsrMessage = true;
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ElaborateNoBit(ulong address, ulong value, string cores)
        {
            if (!MSRWriter.WriteFull(address, value, cores))
            {
                MessageBox.Show($"Failed to write", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var message = $"Bitmask with address \"{AddressTextBox.Text}\" was changed to \"" + value + $"\" on {cores} {(cores.Split(',').Count() > 1 ? "cores" : "core")}";
            MessageBox.Show(message, "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private void ElaborateBitField(ulong address, ulong value, string bit, string cores)
        {
            var start = Convert.ToUInt32(bit.Split('-')[0]);
            var end = Convert.ToUInt32(bit.Split('-')[1]);

            if (start > end || end > 63)
            {
                MessageBox.Show("Invalid bit range", "Bits must be between 0 and 63.", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (value != 1 && value != 0)
            {
                MessageBox.Show("Invalid bit value", "The value must be either 0x1 (enabled) or 0x0 (disabled).", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!MSRWriter.WriteBits(address, value, (int)start, (int)end, cores))
            {
                MessageBox.Show($"Failed to write", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var message = $"The bitmask for address \"{AddressTextBox.Text}\" and bit range \"{bit}\" has been updated to \"{value}\" on {cores} {(cores.Split(',').Count() > 1 ? "cores" : "core")}";
            MessageBox.Show(message, "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ElaborateSingleBit(ulong address, ulong value, string bit, string cores)
        {
            if (value != 1 && value != 0)
            {
                MessageBox.Show("Invalid bit value", "The value must be either 0x1 (enabled) or 0x0 (disabled).", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var valueBoolean = value == 1;

            if (!MSRWriter.WriteBit(address, valueBoolean, Convert.ToInt32(bit), cores))
            {
                MessageBox.Show($"Failed to write", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Bit \"{bit}\" at address \"{AddressTextBox.Text}\" has been {(value == 1 ? "enabled" : "disabled")} on {cores} {(cores.Split(',').Count() > 1 ? "cores" : "core")}", "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
    }
}
