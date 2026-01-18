using System.Diagnostics.Metrics;
using System.Globalization;
using System.Windows;

namespace RegMaster
{
    public partial class MainWindow
    {
        private void ReadPCIButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                ByteTextBoxPCI.Text = PCIReader.ReadByte(BusTextBoxPCI.Text, DeviceTextBoxPCI.Text, FunctionTextBoxPCI.Text, byte.Parse(OffsetTextBoxPCI.Text.Replace("0x", ""), NumberStyles.HexNumber));
                DwordTextBoxPCI.Text = PCIReader.ReadDword(BusTextBoxPCI.Text, DeviceTextBoxPCI.Text, FunctionTextBoxPCI.Text, byte.Parse(OffsetTextBoxPCI.Text.Replace("0x", ""), NumberStyles.HexNumber));
                WordTextBoxPCI.Text = PCIReader.ReadWord(BusTextBoxPCI.Text, DeviceTextBoxPCI.Text, FunctionTextBoxPCI.Text, byte.Parse(OffsetTextBoxPCI.Text.Replace("0x", ""), NumberStyles.HexNumber));

                MessageBox.Show($"PCI {BusTextBoxPCI.Text}:{DeviceTextBoxPCI.Text}:{FunctionTextBoxPCI.Text} read", "Operation completed successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Invalid behavior detected", "Please check the provided values and try again.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool ValidateInput()
        {
            int bus, device, function;

            if (!int.TryParse(BusTextBoxPCI.Text, out bus) || bus < 0 || bus > 255)
            {
                MessageBox.Show("Invalid \"Bus\" value", "It must be between 0 and 255.", MessageBoxButton.OK, MessageBoxImage.Warning);
                BusTextBoxPCI.Focus();
                return false;
            }

            if (!int.TryParse(DeviceTextBoxPCI.Text, out device) || device < 0 || device > 31)
            {
                MessageBox.Show("Invalid \"Device value\"", "It must be between 0 and 31.", MessageBoxButton.OK, MessageBoxImage.Warning);
                DeviceTextBoxPCI.Focus();
                return false;
            }

            if (!int.TryParse(FunctionTextBoxPCI.Text, out function) || function < 0 || function > 7)
            {
                MessageBox.Show("Invalid \"Function\" value", "It must be between 0 and 8.", MessageBoxButton.OK, MessageBoxImage.Warning);
                FunctionTextBoxPCI.Focus();
                return false;
            }

            if (OffsetTextBoxPCI.Text == "0x10 e.g." || !int.TryParse(OffsetTextBoxPCI.Text.Replace("0x", ""), NumberStyles.HexNumber, null, out int offset) || offset < 0)
            {
                MessageBox.Show("Invalid offset", "Enter a hexadecimal value, e.g., 0x10.", MessageBoxButton.OK, MessageBoxImage.Warning);
                OffsetTextBoxPCI.Focus();
                return false;
            }

            return true;
        }
    }
}
