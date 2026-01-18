using System.Windows;
using System.Windows.Controls;

namespace RegMaster
{
    public partial class MainWindow
    {
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            DecimalTextBox.Text = "Decimal";
            BinaryTextBox.Text = "Binary";
            HexTextBox.Text = "Hex";
            AddressTextBox.Text = "0xE2 e.g.";
            BitTextBox.Text = "15 or 0 - 10";
            ValueTextBox.Text = "Hex value";

            AddressTextBox.Foreground = Colors.GrayInactive;
            ValueTextBox.Foreground = Colors.GrayInactive;
            BitTextBox.Foreground = Colors.GrayInactive;

            ClearEaxAndEdxGrids();

            AllCoresCheckBox.IsChecked = false;
            foreach (var checkBox in CoreGrid.Children)
                if (checkBox is CheckBox cb)
                    cb.IsChecked = false;
        }

        private void ResetButtonMMIO_Click(object sender, RoutedEventArgs e)
        {
            byteTextBox.Text = dwordTextBox.Text = wordTextBox.Text = "";
            AddressTextBoxMMIO.Text = "0x53402024 e.g.";
            BitTextBoxMMIO.Text = "15 or 0 - 10";
            ValueTextBoxMMIO.Text = "0x00 (Byte) or 0x0000 (Word) or 0x00000000 (Dword)";
            AddressTextBoxMMIO.Foreground = Colors.GrayInactive;
            ValueTextBoxMMIO.Foreground = Colors.GrayInactive;
            BitTextBoxMMIO.Foreground = Colors.GrayInactive;
            ClearMMIORegistryGrid();
        }

        private void ResetButtonPCI_Click(object sender, RoutedEventArgs e)
        {
            ByteTextBoxPCI.Text = DwordTextBoxPCI.Text = WordTextBoxPCI.Text = "";
            BusTextBoxPCI.Text = "0 - 255";
            DeviceTextBoxPCI.Text = "0 - 31";
            FunctionTextBoxPCI.Text = "0 - 7";
            OffsetTextBoxPCI.Text = "0x10 e.g.";
            OffsetTextBoxPCI.Foreground = BusTextBoxPCI.Foreground = DeviceTextBoxPCI.Foreground = FunctionTextBoxPCI.Foreground = Colors.GrayInactive;
        }
    }
}
