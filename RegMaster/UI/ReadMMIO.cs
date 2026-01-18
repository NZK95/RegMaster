using System.Windows;

namespace RegMaster
{
    public partial class MainWindow
    {
        private void ReadButtonMMIO_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddressTextBoxMMIO.Text) || AddressTextBoxMMIO.Text == "0x53402024 e.g.")
            {
                MessageBox.Show("No input detected", "Please provide a valid memory address.", MessageBoxButton.OK, MessageBoxImage.Warning);
                AddressTextBoxMMIO.Focus();
                return;
            }

            try
            {
                ClearMMIORegistryGrid();

                ulong address = Convert.ToUInt64(AddressTextBoxMMIO.Text, 16);

                if (string.IsNullOrEmpty(BitTextBoxMMIO.Text) || BitTextBoxMMIO.Text == "15 or 0 - 10")
                    ElaborateNoBitRead(address);

                else if (BitTextBoxMMIO.Text.Contains('-'))
                    ElaborateBitFieldRead(address);

                else
                    ElaborateSingleBitRead(address);
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ElaborateBitFieldRead(ulong address)
        {
            ElaborateNoBitRead(address, showMessage: false);

            var start = Convert.ToUInt32(BitTextBoxMMIO.Text.Split('-')[0]);
            var end = Convert.ToUInt32(BitTextBoxMMIO.Text.Split('-')[1]);

            if (start > end || end >= 32)
            {
                MessageBox.Show("Invalid bit range", "Bit range must be between 0 and 31 and start ≤ end.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var message = $"Bits ({start} - {end}): \"{MMIOReader.ReadBits(address, 32, start, end)}\"";
            MessageBox.Show(message, "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ElaborateSingleBitRead(ulong address)
        {
            ElaborateNoBitRead(address, showMessage: false);

            var bitPosition = Convert.ToUInt32(BitTextBoxMMIO.Text);

            if (bitPosition >= 32)
            {
                MessageBox.Show("Invalid bit position", "Bit position must be between 0 and 31.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var bit = MMIOReader.ReadBit(address, 32, bitPosition);
            MessageBox.Show($"Bit \"{bitPosition}\" at address \"{(AddressTextBoxMMIO.Text.StartsWith("0x") ? AddressTextBoxMMIO.Text : $"0x{AddressTextBoxMMIO.Text}")}\" is {(bit == "1" ? "enabled" : "disabled")}", "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ElaborateNoBitRead(ulong address, bool showMessage = true)
        {
            byteTextBox.Text = MMIOReader.ReadMem(address, 8);
            wordTextBox.Text = MMIOReader.ReadMem(address, 16);
            dwordTextBox.Text = MMIOReader.ReadMem(address, 32);
            FillGridWithMessages(address, showMessage);
        }

        private void FillGridWithMessages(ulong address, bool showMessage = true)
        {
            var dword = MMIOReader.ReadMem(address, 32);

            if (string.IsNullOrEmpty(dword) || dword == "N/A")
            {
                MessageBox.Show($"Memory address \"{(AddressTextBoxMMIO.Text.StartsWith("0x") ? AddressTextBoxMMIO.Text : $"0x{AddressTextBoxMMIO.Text}")}\" was not found", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var decimalValue = Convert.ToUInt32(dword, 16);
                var binaryValue = Convert.ToString(decimalValue, 2).PadLeft(32, '0');
                FillMMIORegistryGrid(binaryValue);

                if (showMessage)
                    MessageBox.Show($"Memory address \"{(AddressTextBoxMMIO.Text.StartsWith("0x") ? AddressTextBoxMMIO.Text : $"0x{AddressTextBoxMMIO.Text}")}\" read", "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
