using RegMaster.src.MMIO;
using System.Windows;

namespace RegMaster
{
    public partial class MainWindow
    {
        private void WriteButtonMMIO_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddressTextBoxMMIO.Text) || string.IsNullOrEmpty(ValueTextBoxMMIO.Text) ||
                AddressTextBoxMMIO.Text == "0x53402024 e.g." || ValueTextBoxMMIO.Text == "0x00 (Byte) or 0x0000 (Word) or 0x00000000 (Dword)")
            {
                MessageBox.Show("Missing input", "Please provide a valid address and value.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var address = Convert.ToUInt64(AddressTextBoxMMIO.Text, 16);

                if (string.IsNullOrEmpty(BitTextBoxMMIO.Text) || BitTextBoxMMIO.Text == "15 or 0 - 10")
                    ElaborateNoBitWrite(address);

                else if (BitTextBoxMMIO.Text.Contains('-'))
                    ElaborateBitFieldWrite(address);

                else
                    ElaborateSingleBitWrite(address);
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool ElaborateBitFieldWrite(ulong address)
        {
            var start = Convert.ToUInt32(BitTextBoxMMIO.Text.Split('-')[0]);
            var end = Convert.ToUInt32(BitTextBoxMMIO.Text.Split('-')[1]);

            if (start > end || end > 31)
            {
                MessageBox.Show("Invalid bit range", "Bit range must be between 0 and 31 and start ≤ end.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (ValueTextBoxMMIO.Text != "0x1" && ValueTextBoxMMIO.Text != "0X1" && ValueTextBoxMMIO.Text != "1" &&
                ValueTextBoxMMIO.Text != "0x0" && ValueTextBoxMMIO.Text != "0X0" && ValueTextBoxMMIO.Text != "0")
            {
                MessageBox.Show("Invalid bit value", "The value must be either 0x1 (enabled) or 0x0 (disabled).", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            var valueBoolean = ValueTextBoxMMIO.Text == "1" || ValueTextBoxMMIO.Text == "0x1" || ValueTextBoxMMIO.Text == "0X1";

            if (!MMIOWriter.WriteBits(address, start, end, valueBoolean))
            {
                MessageBox.Show("Failed to write data to memory", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            ElaborateNoBitRead(address, showMessage: false);
            var addressString = AddressTextBoxMMIO.Text.StartsWith("0x") || AddressTextBoxMMIO.Text.StartsWith("0X") ? AddressTextBoxMMIO.Text : $"0x{AddressTextBoxMMIO.Text}";
            MessageBox.Show($"Bitfield \"{BitTextBoxMMIO.Text}\" at address \"{addressString}\" has been {(valueBoolean ? "enabled" : "disabled")}", "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private bool ElaborateSingleBitWrite(ulong address)
        {
            if (Convert.ToUInt32(BitTextBoxMMIO.Text) > 31)
            {
                MessageBox.Show("Invalid bit position", "The value must be either 0 and 31.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (ValueTextBoxMMIO.Text != "0x1" && ValueTextBoxMMIO.Text != "0X1" && ValueTextBoxMMIO.Text != "1" &&
                ValueTextBoxMMIO.Text != "0x0" && ValueTextBoxMMIO.Text != "0X0" && ValueTextBoxMMIO.Text != "0")
            {
                MessageBox.Show("Invalid bit value", "The value must be either 0x1 (enabled) or 0x0 (disabled).", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            var valueBoolean = ValueTextBoxMMIO.Text == "1" || ValueTextBoxMMIO.Text == "0x1" || ValueTextBoxMMIO.Text == "0X1";

            if (!MMIOWriter.WriteMembit(address, Convert.ToUInt32(BitTextBoxMMIO.Text), valueBoolean))
            {
                MessageBox.Show("Failed to write data to memory", "Operation faileds.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            ElaborateNoBitRead(address, showMessage: false);
            var addressString = AddressTextBoxMMIO.Text.StartsWith("0x") || AddressTextBoxMMIO.Text.StartsWith("0X") ? AddressTextBoxMMIO.Text : $"0x{AddressTextBoxMMIO.Text}";
            MessageBox.Show($"Bit \"{BitTextBoxMMIO.Text}\" at address \"{addressString}\" has been {(valueBoolean ? "enabled" : "disabled")}", "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private bool ElaborateNoBitWrite(ulong address)
        {
            var value = ValueTextBoxMMIO.Text
                   .Replace("0x", "")
                   .Replace("0X", "")
                   .Replace(" ", "")
                   .ToUpper();

            var size = (uint)value.Length * 4;

            if (value.Length != 2 && value.Length != 4 && value.Length != 8)
            {
                MessageBox.Show("Invalid value length", "The value should have 2 (byte), 4 (word) or 8 (dword) characters.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!MMIOWriter.WriteMemFull(address, size, Convert.ToUInt64(value, 16)))
            {
                MessageBox.Show("Failed to write data to memory", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var addressString = (AddressTextBoxMMIO.Text.StartsWith("0x") ? AddressTextBoxMMIO.Text : $"0x{AddressTextBoxMMIO.Text}");
            var typeOfData = "Data";

            switch (value.Length)
            {
                case 2: typeOfData = "Byte (8 bit)"; break;
                case 4: typeOfData = "Word (16 bit)"; break;
                case 8: typeOfData = "Dword (32 bit)"; break;
                default: typeOfData = "Data"; break;
            }

            ElaborateNoBitRead(address, showMessage: false);
            MessageBox.Show($"{typeOfData} written to memory address \"{addressString}\" successfully", "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
