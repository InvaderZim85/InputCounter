using System.ComponentModel;
using System.Windows;
using InputCounter.Ui.ViewModel;
using MahApps.Metro.Controls;

namespace InputCounter.Ui.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/>
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Occurs when the window was loaded
    /// </summary>
    /// <param name="sender">The <see cref="MainWindow"/></param>
    /// <param name="e">The event arguments</param>
    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel) 
            return;

        viewModel.InitViewModel();
        viewModel.InitData();
    }

    /// <summary>
    /// Occurs when the window is closing
    /// </summary>
    /// <param name="sender">The <see cref="MainWindow"/></param>
    /// <param name="e">The event arguments</param>
    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.CloseViewModel();
        }
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
}