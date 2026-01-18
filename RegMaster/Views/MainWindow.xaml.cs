using OpenLibSys;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RegMaster
{
    public partial class MainWindow
    {
        [DllImport(@"WinRing0x64.dll")]
        private static extern bool InitializeOls();

        [DllImport(@"WinRing0x64.dll")]
        private static extern void DeinitializeOls();

        private PciIdsParser _pciIdsParser = new();
        private ObservableCollection<PCIDevice> _pciDevices = new();

        public MainWindow()
        {
            Closed += MainWindow_Closed;
            Loaded += LoadParser;

            CheckStatus();
            InitializeComponent();
            AddCheckboxesBasedOnCores();

            DeviceListBox.ItemsSource = _pciDevices;
        }



        private static void CheckStatus()
        {
            if (!InitializeOls())
            {
                MessageBox.Show("Failed to initialize OLS.");
                Environment.Exit(0);
            }

            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Please run the application as Administrator.");
                Environment.Exit(0);
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            DeinitializeOls();
        }

        public void FillEaxEdxGrids(string eax, string edx)
        {
            EaxGrid.Children.Clear();
            EdxGrid.Children.Clear();

            eax.Reverse();
            edx.Reverse();

            for (int i = 0; i < 32; i++)
            {
                var textBoxEax = new System.Windows.Controls.TextBox
                {
                    Text = eax[i].ToString(),
                    IsReadOnly = true,
                    TextAlignment = System.Windows.TextAlignment.Center,
                    Margin = new System.Windows.Thickness(0, 0, 2, 0)
                };

                textBoxEax.Background = textBoxEax.Text == "1" ? Colors.Orange : Colors.Gray;
                textBoxEax.MouseDoubleClick += TextBox_MouseDoubleClick;

                var textBoxEdx = new System.Windows.Controls.TextBox
                {
                    Text = edx[i].ToString(),
                    IsReadOnly = true,
                    TextAlignment = System.Windows.TextAlignment.Center,
                    Margin = new System.Windows.Thickness(0, 0, 2, 0)
                };

                textBoxEdx.Background = textBoxEdx.Text == "1" ? Colors.Orange : Colors.Gray;
                textBoxEdx.MouseDoubleClick += TextBox_MouseDoubleClick;

                Grid.SetColumn(textBoxEax, i);
                EaxGrid.Children.Add(textBoxEax);

                Grid.SetColumn(textBoxEdx, i);
                EdxGrid.Children.Add(textBoxEdx);
            }
        }

        private void FillMMIORegistryGrid(string value)
        {
            RegistryMMIOGrid.Children.Clear();

            for (int i = 0; i < 32; ++i)
            {
                var textBox = new System.Windows.Controls.TextBox
                {
                    Text = value[i].ToString(),
                    IsReadOnly = true,
                    TextAlignment = System.Windows.TextAlignment.Center,
                    Margin = new System.Windows.Thickness(0, 0, 2, 0)
                };

                textBox.Background = textBox.Text == "1" ? Colors.Orange : Colors.Gray;
                textBox.MouseDoubleClick += TextBox_MouseDoubleClick;

                Grid.SetColumn(textBox, i);
                RegistryMMIOGrid.Children.Add(textBox);
            }
        }

        private void ClearEaxAndEdxGrids()
        {
            foreach (var child in EaxGrid.Children)
            {
                if (child is TextBox tb)
                {
                    tb.Text = "0";
                    tb.Background = Colors.Gray;
                }
            }

            foreach (var child in EdxGrid.Children)
            {
                if (child is TextBox tb)
                {
                    tb.Text = "0";
                    tb.Background = Colors.Gray;
                }
            }
        }

        private void ClearMMIORegistryGrid()
        {
            foreach (var child in RegistryMMIOGrid.Children)
            {
                if (child is TextBox tb)
                {
                    tb.Text = "0";
                    tb.Background = Colors.Gray;
                }
            }
        }

        private void AllCores_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < CoreGrid.Children.Count; i++)
                if (CoreGrid.Children[i] is CheckBox checkBox)
                    checkBox.IsChecked = true;
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;

            if (textBox != null)
            {
                if (textBox.Text == "0")
                {
                    textBox.Text = "1";
                    textBox.Background = Colors.Orange;
                }
                else
                {
                    textBox.Text = "0";
                    textBox.Background = Colors.Gray;
                }
            }
        }

        private void DeviceListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DeviceListBox.SelectedItem is PCIDevice item)
            {
                DetailsTextBlock.Text = item.GetDetailedInfo();
            }
        }

        private void TextEdxBox_0_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_5_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_6_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_7_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_8_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_9_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_10_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_11_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_12_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_13_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_14_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_15_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_16_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_17_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_18_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_19_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_20_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_21_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_22_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_23_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_24_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_25_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_26_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_27_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_28_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_29_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_30_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextEdxBox_31_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_0_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_5_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_6_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_7_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_8_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_9_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_10_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_11_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_12_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_13_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_14_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_15_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_16_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_17_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_18_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_19_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_20_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_21_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_22_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_23_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_24_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_25_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_26_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_27_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_28_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_29_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_30_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextBox_31_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void AddressTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AddressTextBox.Text == "0xE2 e.g.")
            {
                AddressTextBox.Text = "";
                AddressTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void AddressTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                AddressTextBox.Text = "0xE2 e.g.";
                AddressTextBox.Foreground = Colors.GrayInactive;
            }
        }

        private void BitTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BitTextBox.Text == "15 or 0 - 10")
            {
                BitTextBox.Text = "";
                BitTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void BitTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BitTextBox.Text))
            {
                BitTextBox.Text = "15 or 0 - 10";
                BitTextBox.Foreground = Colors.GrayInactive;
            }
        }

        private void ValueTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ValueTextBox.Text == "Hex value")
            {
                ValueTextBox.Text = "";
                ValueTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void ValueTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ValueTextBox.Text))
            {
                ValueTextBox.Text = "Hex value";
                ValueTextBox.Foreground = Colors.GrayInactive;
            }
        }

        private void AddressTextBoxMMIO_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AddressTextBoxMMIO.Text == "0x53402024 e.g.")
            {
                AddressTextBoxMMIO.Text = "";
                AddressTextBoxMMIO.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void AddressTextBoxMMIO_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressTextBoxMMIO.Text))
            {
                AddressTextBoxMMIO.Text = "0x53402024 e.g.";
                AddressTextBoxMMIO.Foreground = Colors.GrayInactive;
            }
        }

        private void BitTextBoxMMIO_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BitTextBoxMMIO.Text == "15 or 0 - 10")
            {
                BitTextBoxMMIO.Text = "";
                BitTextBoxMMIO.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void BitTextBoxMMIO_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BitTextBoxMMIO.Text))
            {
                BitTextBoxMMIO.Text = "15 or 0 - 10";
                BitTextBoxMMIO.Foreground = Colors.GrayInactive;
            }
        }


        private void ValueTextBoxMMIO_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ValueTextBoxMMIO.Text == "0x00 (Byte) or 0x0000 (Word) or 0x00000000 (Dword)")
            {
                ValueTextBoxMMIO.Text = "";
                ValueTextBoxMMIO.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void ValueTextBoxMMIO_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ValueTextBoxMMIO.Text))
            {
                ValueTextBoxMMIO.Text = "0x00 (Byte) or 0x0000 (Word) or 0x00000000 (Dword)";
                ValueTextBoxMMIO.Foreground = Colors.GrayInactive;
            }
        }

        private void BusTextBoxPCI_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BusTextBoxPCI.Text))
            {
                BusTextBoxPCI.Text = "0 - 255"; 
                BusTextBoxPCI.Foreground = Colors.GrayInactive;
            }
        }

        private void DeviceTextBoxPCI_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DeviceTextBoxPCI.Text))
            {
                DeviceTextBoxPCI.Text = "0 - 31"; 
                DeviceTextBoxPCI.Foreground = Colors.GrayInactive;
            }
        }

        private void FunctionTextBoxPCI_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FunctionTextBoxPCI.Text))
            {
                FunctionTextBoxPCI.Text = "0 - 7"; 
                FunctionTextBoxPCI.Foreground = Colors.GrayInactive;
            }
        }

        private void BusTextBoxPCI_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BusTextBoxPCI.Text == "0 - 255")
            {
                BusTextBoxPCI.Text = "";
                BusTextBoxPCI.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void DeviceTextBoxPCI_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DeviceTextBoxPCI.Text == "0 - 31")
            {
                DeviceTextBoxPCI.Text = "";
                DeviceTextBoxPCI.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void FunctionTextBoxPCI_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FunctionTextBoxPCI.Text == "0 - 7")
            {
                FunctionTextBoxPCI.Text = "";
                FunctionTextBoxPCI.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void OffsetTextBoxPCI_GotFocus(object sender, RoutedEventArgs e)
        {
            if (OffsetTextBoxPCI.Text == "0x10 e.g.") 
            {
                OffsetTextBoxPCI.Text = "";
                OffsetTextBoxPCI.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void OffsetTextBoxPCI_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OffsetTextBoxPCI.Text))
            {
                OffsetTextBoxPCI.Text = "0x10 e.g."; 
                OffsetTextBoxPCI.Foreground = Colors.GrayInactive; 
            }
        }


        private void TextMMIOBox_0_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_5_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_6_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_7_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_8_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_9_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_10_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_11_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_12_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_13_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_14_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_15_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_16_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_17_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_18_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_19_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_20_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_21_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_22_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_23_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_24_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_25_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_26_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_27_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_28_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_29_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_30_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }

        private void TextMMIOBox_31_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox_MouseDoubleClick(sender, e);
        }
    }
}