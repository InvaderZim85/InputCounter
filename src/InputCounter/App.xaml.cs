using System.Windows;
using InputCounter.Common;
using InputCounter.Ui.View;

namespace InputCounter;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// The main window
    /// </summary>
    private MainWindow? _mainWindow;

    /// <summary>
    /// Occurs when the window will be started
    /// </summary>
    /// <param name="sender">The <see cref="App"/></param>
    /// <param name="e">The event arguments</param>
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        Helper.InitLogger();

        _mainWindow = new MainWindow();

        Helper.SetWindowPosition(_mainWindow);

        _mainWindow.Show();
    }

    /// <summary>
    /// Occurs when the window will be closed
    /// </summary>
    /// <param name="sender">The <see cref="App"/></param>
    /// <param name="e">The event arguments</param>
    private void App_OnExit(object sender, ExitEventArgs e)
    {
        if (_mainWindow != null)
            Helper.SaveWindowPosition(_mainWindow);
    }
}