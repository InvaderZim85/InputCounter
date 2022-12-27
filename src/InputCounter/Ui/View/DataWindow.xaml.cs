using System.Windows;
using InputCounter.Ui.ViewModel;
using MahApps.Metro.Controls;

namespace InputCounter.Ui.View;

/// <summary>
/// Interaction logic for DataWindow.xaml
/// </summary>
public partial class DataWindow : MetroWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="DataWindow"/>
    /// </summary>
    public DataWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Occurs when the user hits the close button
    /// </summary>
    /// <param name="sender">The close button</param>
    /// <param name="e">The event arguments</param>
    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Occurs when the window was loaded
    /// </summary>
    /// <param name="sender">The <see cref="DataWindow"/></param>
    /// <param name="e">The event arguments</param>
    private async void DataWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is DataWindowViewModel viewModel)
            await viewModel.LoadDataAsync();
    }
}