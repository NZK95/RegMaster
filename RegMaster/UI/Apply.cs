using RegMaster.src.MMIO;
using System.Windows;
using System.Windows.Controls;

namespace RegMaster
{
    public partial class MainWindow
    {
        private void ApplyButtonMMIO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var binaryString = string.Empty;

                foreach (var child in RegistryMMIOGrid.Children)
                    if (child is TextBox tb)
                        binaryString += tb.Text;

                var value = Convert.ToUInt64(binaryString, 2);
                var address = Convert.ToUInt64(AddressTextBoxMMIO.Text, 16);
                var size = (uint)32;

                if (!MMIOWriter.WriteMemFull(address, size, value))
                {
                    MessageBox.Show($"Failed to write", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ElaborateNoBitRead(address, false);
                var addressString = AddressTextBoxMMIO.Text.StartsWith("0x") || AddressTextBoxMMIO.Text.StartsWith("0X") ? AddressTextBoxMMIO.Text : $"0x{AddressTextBoxMMIO.Text}";
                MessageBox.Show($"Data at address \"{addressString}\" was changed to \"0x{value:X8}\"", "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BinaryTextBox.Text) || BinaryTextBox.Text == "N/A" || !BinaryTextBox.Text.Contains('|'))
            {
                MessageBox.Show("No eax and edx found", "Please enter a valid memory address.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var eaxBin = string.Empty;
                var edxBin = string.Empty;

                foreach (var child in EaxGrid.Children)
                    if (child is TextBox tb)
                        eaxBin += tb.Text;

                foreach (var child in EdxGrid.Children)
                    if (child is TextBox tb)
                        edxBin += tb.Text;

                var address = Convert.ToUInt32(AddressTextBox.Text, 16);
                var eaxNum = Convert.ToUInt32(eaxBin, 2);
                var edxNum = Convert.ToUInt32(edxBin, 2);
                ulong value = (eaxNum << 32) | edxNum;
                var cores = string.Empty;

                if (GetCountOfSelectedCores() == 0)
                    cores = "0";
                else
                    cores = GetCores();

                if (MSRWriter.WriteFull(address, value, cores))
                {
                    _showMsrMessage = false;
                    ReadButton_Click(sender, e);
                    _showMsrMessage = true;
                    var message = $"Bitmask with address \"{AddressTextBox.Text}\" was changed to \"{value:X8}\" on {cores} {(cores.Split(',').Count() > 1 ? "cores" : "core")}";
                    MessageBox.Show(message, "Operation completed successfully.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show($"Failed to write", "Operation failed.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ApplyPCIButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
