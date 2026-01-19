using System.Windows;
using System.Windows.Controls;

namespace RegMaster
{
    public partial class MainWindow
    {
        private void AddCheckboxesBasedOnCores()
        {
            var coreCount = Environment.ProcessorCount;

            for (int i = 0; i < coreCount; i++)
            {
                CoreGrid.ColumnDefinitions.Add(new ColumnDefinition() { });

                var checkBox = new CheckBox
                {
                    Content = $"Core {i}",
                    Margin = new Thickness(50, 0, 0, 0),
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                checkBox.Unchecked += (sender, e) =>
                {
                    AllCoresCheckBox.IsChecked = false;
                    checkBox.IsChecked = false;
                };

                CoreGrid.Children.Add(checkBox);
                Grid.SetColumn(checkBox, i + 1);
            }
        }

        private string GetCores()
        {
            if (AllCoresCheckBox.IsChecked == true)
                return "all";

            var result = string.Empty;

            for (int i = 1; i < CoreGrid.Children.Count; i++)
                if (CoreGrid.Children[i] is CheckBox checkBox && checkBox.IsChecked == true)
                    result += $"{i - 1},";

            return result.Remove(result.Length - 1);
        }

        private int GetNumberOfSelectedCore()
        {
            for (int i = 1; i < CoreGrid.Children.Count; i++)
                if (CoreGrid.Children[i] is CheckBox checkBox && checkBox.IsChecked == true)
                    return Convert.ToInt32(checkBox.Content.ToString().Replace("Core ", "").Trim());

            return 0;
        }

        private int GetCountOfSelectedCores()
        {
            var count = 0;

            for (int i = 1; i < CoreGrid.Children.Count; i++)
                if (CoreGrid.Children[i] is CheckBox checkBox && checkBox.IsChecked == true)
                    count++;

            return count;
        }
        
        private bool NoOneCoreIsSelected()
        {
            for (int i = 1; i < CoreGrid.Children.Count; i++)
                if (CoreGrid.Children[i] is CheckBox checkBox && checkBox.IsChecked == true)
                    return false;

            return true;
        }
    }
}
