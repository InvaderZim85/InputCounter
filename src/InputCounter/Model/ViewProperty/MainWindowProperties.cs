using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using InputCounter.Model.Stats;

namespace InputCounter.Model.ViewProperty;

/// <summary>
/// Provides the properties for the main window
/// </summary>
internal sealed partial class MainWindowProperties : ObservableObject
{
    #region Keyboard

    /// <summary>
    /// Gets or sets the total count of the keyboard
    /// </summary>
    [ObservableProperty]
    private string _totalCountKeyboard = string.Empty;

    /// <summary>
    /// Gets or sets the percentage of the total count
    /// </summary>
    [ObservableProperty]
    private string _totalCountPercentageKeyboard = string.Empty;

    /// <summary>
    /// Gets or sets the previous count of the keyboard
    /// </summary>
    [ObservableProperty] 
    private string _previousCountKeyboard = string.Empty;

    /// <summary>
    /// Gets or sets the max progress value
    /// </summary>
    [ObservableProperty]
    private int _progressMaxKeyboard = 100;

    /// <summary>
    /// Gets or sets the current progress of the keyboard
    /// </summary>
    [ObservableProperty]
    private int _progressCurrentKeyboard;

    /// <summary>
    /// Gets or sets the keyboard stats
    /// </summary>
    [ObservableProperty]
    private KeyboardStats _keyboardStats = new();
    #endregion

    #region Mouse
    /// <summary>
    /// Gets or sets the total count of the keyboard
    /// </summary>
    [ObservableProperty] 
    private string _totalCountMouse = string.Empty;

    /// <summary>
    /// Gets or sets the total count in percentage for the mouse
    /// </summary>
    [ObservableProperty]
    private string _totalCountPercentageMouse = string.Empty;

    /// <summary>
    /// Gets or sets the previous count of the keyboard
    /// </summary>
    [ObservableProperty]
    private string _previousCountMouse = string.Empty;

    /// <summary>
    /// Gets or sets the max progress of the mouse
    /// </summary>
    [ObservableProperty]
    private int _progressMaxMouse = 100;

    /// <summary>
    /// Gets or sets the current progress of the mouse
    /// </summary>
    private int _progressCurrentMouse;

    /// <summary>
    /// Gets or sets the current progress of the mouse
    /// </summary>
    public int ProgressCurrentMouse
    {
        get => _progressCurrentMouse;
        set => SetProperty(ref _progressCurrentMouse, value);
    }

    /// <summary>
    /// Gets or sets the mouse statistics
    /// </summary>
    private MouseStats _mouseStats = new();

    /// <summary>
    /// Gets or sets the mouse statistics
    /// </summary>
    public MouseStats MouseStats
    {
        get => _mouseStats;
        set => SetProperty(ref _mouseStats, value);
    }
    #endregion

    #region General
    /// <summary>
    /// Gets or sets the window header
    /// </summary>
    [ObservableProperty]
    private string _windowHeader = "Input Counter";

    /// <summary>
    /// Gets or sets the stats header
    /// </summary>
    [ObservableProperty] 
    private string _statsHeader = "Statistics";

    /// <summary>
    /// Gets or sets the header of the keyboard stats
    /// </summary>
    [ObservableProperty] 
    private string _statsHeaderKeyboard = "Keyboard";

    /// <summary>
    /// Gets or sets the header of hte mouse stats
    /// </summary>
    [ObservableProperty] 
    private string _statsHeaderMouse = "Mouse";

    /// <summary>
    /// Gets or sets the value which indicates if the application should be always on top
    /// </summary>
    [ObservableProperty]
    private bool _alwaysOnTop;

    /// <summary>
    /// Occurs when the value of <see cref="AlwaysOnTop"/> was changed.
    /// </summary>
    /// <param name="value">The new value.</param>
    partial void OnAlwaysOnTopChanged(bool value)
    {
        if (Application.Current.MainWindow == null)
            return;

        Application.Current.MainWindow.Topmost = value;
        Properties.Settings.Default.AlwaysOnTop = value;
        Properties.Settings.Default.Save();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of the <see cref="MainWindowProperties"/>
    /// </summary>
    public MainWindowProperties()
    {
        AlwaysOnTop = Properties.Settings.Default.AlwaysOnTop;
    }
}