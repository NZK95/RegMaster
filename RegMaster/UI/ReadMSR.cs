using System.Windows;

namespace RegMaster
{
    public partial class MainWindow
    {
        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddressTextBox.Text) || AddressTextBox.Text == "0xE2 e.g.")
            {
                MessageBox.Show("No input detected", "Please enter a valid memory address.", MessageBoxButton.OK, MessageBoxImage.Warning);
                AddressTextBox.Focus();
                return;
            }

            try
            {
                ClearEaxAndEdxGrids();

                var address = Convert.ToUInt64(AddressTextBox.Text, 16);
                var bit = BitTextBox.Text;
                var isBitDefault = string.IsNullOrEmpty(bit) || bit == "15 or 0 - 10";
                var selectedCore = 0;

                if (AllCoresCheckBox.IsChecked == true || GetCountOfSelectedCores() > 1)
                    selectedCore = 0;
                else
                    selectedCore = GetNumberOfSelectedCore();

                BinaryTextBox.Text = isBitDefault ? MSRReader.GetBinary(address, selectedCore) : MSRReader.GetBinary(address, selectedCore, BitTextBox.Text);
                HexTextBox.Text = isBitDefault ? MSRReader.GetHex(address, selectedCore) : MSRReader.GetHex(address, selectedCore, BitTextBox.Text);
                DecimalTextBox.Text = isBitDefault ? MSRReader.GetDecimal(address, selectedCore) : MSRReader.GetDecimal(address, selectedCore, BitTextBox.Text); ;

                if (BinaryTextBox.Text.Contains('|'))
                {
                    var eax = BinaryTextBox.Text.Split('|')[1].Trim();
                    var edx = BinaryTextBox.Text.Split('|')[0].Trim();

                    FillEaxEdxGrids(eax, edx);
                }

                if (_showMsrMessage)
                {
                    if (BinaryTextBox.Text != "N/A")
                        MessageBox.Show($"Address \"{(AddressTextBox.Text.StartsWith("0x") ? AddressTextBox.Text : $"0x{AddressTextBox.Text}")}\" read on core {selectedCore}", "Operation completed successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show($"Address \"{(AddressTextBox.Text.StartsWith("0x") ? AddressTextBox.Text : $"0x{AddressTextBox.Text}")}\" was not found", "Operation failed", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
