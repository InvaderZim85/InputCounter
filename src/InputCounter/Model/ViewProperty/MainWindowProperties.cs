using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using InputCounter.Model.Stats;

namespace InputCounter.Model.ViewProperty;

/// <summary>
/// Provides the properties for the main window
/// </summary>
internal class MainWindowProperties : ObservableObject
{
    #region Keyboard

    /// <summary>
    /// Backing field for <see cref="TotalCountKeyboard"/>
    /// </summary>
    private string _totalCountKeyboard = string.Empty;

    /// <summary>
    /// Gets or sets the total count of the keyboard
    /// </summary>
    public string TotalCountKeyboard
    {
        get => _totalCountKeyboard;
        set => SetProperty(ref _totalCountKeyboard, value);
    }

    /// <summary>
    /// Backing field for <see cref="TotalCountPercentageKeyboard"/>
    /// </summary>
    private string _totalCountPercentageKeyboard = string.Empty;

    /// <summary>
    /// Gets or sets the percentage of the total count
    /// </summary>
    public string TotalCountPercentageKeyboard
    {
        get => _totalCountPercentageKeyboard;
        set => SetProperty(ref _totalCountPercentageKeyboard, value);
    }

    /// <summary>
    /// Backing field for <see cref="PreviousCountKeyboard"/>
    /// </summary>
    private string _previousCountKeyboard = string.Empty;

    /// <summary>
    /// Gets or sets the previous count of the keyboard
    /// </summary>
    public string PreviousCountKeyboard
    {
        get => _previousCountKeyboard;
        set => SetProperty(ref _previousCountKeyboard, value);
    }

    /// <summary>
    /// Backing field for <see cref="ProgressMaxKeyboard"/>
    /// </summary>
    private int _progressMaxKeyboard = 100;

    /// <summary>
    /// Gets or sets the max progress value
    /// </summary>
    public int ProgressMaxKeyboard
    {
        get => _progressMaxKeyboard;
        set => SetProperty(ref _progressMaxKeyboard, value);
    }

    /// <summary>
    /// Backing field for <see cref="ProgressCurrentKeyboard"/>
    /// </summary>
    private int _progressCurrentKeyboard = 0;

    /// <summary>
    /// Gets or sets the current progress of the keyboard
    /// </summary>
    public int ProgressCurrentKeyboard
    {
        get => _progressCurrentKeyboard;
        set => SetProperty(ref _progressCurrentKeyboard, value);
    }

    /// <summary>
    /// Backing field for <see cref="KeyboardStats"/>
    /// </summary>
    private KeyboardStats _keyboardStats = new();

    /// <summary>
    /// Gets or sets the keyboard stats
    /// </summary>
    public KeyboardStats KeyboardStats
    {
        get => _keyboardStats;
        set => SetProperty(ref _keyboardStats, value);
    }
    #endregion

    #region Mouse
    /// <summary>
    /// Backing field for <see cref="TotalCountMouse"/>
    /// </summary>
    private string _totalCountMouse = string.Empty;

    /// <summary>
    /// Gets or sets the total count of the keyboard
    /// </summary>
    public string TotalCountMouse
    {
        get => _totalCountMouse;
        set => SetProperty(ref _totalCountMouse, value);
    }

    /// <summary>
    /// Backing field for <see cref="TotalCountPercentageMouse"/>
    /// </summary>
    private string _totalCountPercentageMouse = string.Empty;

    /// <summary>
    /// Gets or sets the total count in percentage for the mouse
    /// </summary>
    public string TotalCountPercentageMouse
    {
        get => _totalCountPercentageMouse;
        set => SetProperty(ref _totalCountPercentageMouse, value);
    }

    /// <summary>
    /// Backing field for <see cref="PreviousCountMouse"/>
    /// </summary>
    private string _previousCountMouse = string.Empty;

    /// <summary>
    /// Gets or sets the previous count of the keyboard
    /// </summary>
    public string PreviousCountMouse
    {
        get => _previousCountMouse;
        set => SetProperty(ref _previousCountMouse, value);
    }

    /// <summary>
    /// Backing field for <see cref="ProgressMaxMouse"/>
    /// </summary>
    private int _progressMaxMouse = 100;

    /// <summary>
    /// Gets or sets the max progress of the mouse
    /// </summary>
    public int ProgressMaxMouse
    {
        get => _progressMaxMouse;
        set => SetProperty(ref _progressMaxMouse, value);
    }

    /// <summary>
    /// Backing field for <see cref="ProgressCurrentMouse"/>
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
    /// Backing field for <see cref="MouseStats"/>
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
    /// Backing field for <see cref="WindowHeader"/>
    /// </summary>
    private string _windowHeader = "Input Counter";

    /// <summary>
    /// Gets or sets the window header
    /// </summary>
    public string WindowHeader
    {
        get => _windowHeader;
        set => SetProperty(ref _windowHeader, value);
    }

    /// <summary>
    /// Backing field for <see cref="StatsHeader"/>
    /// </summary>
    private string _statsHeader = "Statistics";

    /// <summary>
    /// Gets or sets the stats header
    /// </summary>
    public string StatsHeader
    {
        get => _statsHeader;
        set => SetProperty(ref _statsHeader, value);
    }

    /// <summary>
    /// Backing field for <see cref="StatsHeaderKeyboard"/>
    /// </summary>
    private string _statsHeaderKeyboard = "Keyboard";

    /// <summary>
    /// Gets or sets the header of the keyboard stats
    /// </summary>
    public string StatsHeaderKeyboard
    {
        get => _statsHeaderKeyboard;
        set => SetProperty(ref _statsHeaderKeyboard, value);
    }

    /// <summary>
    /// Backing field for <see cref="StatsHeaderMouse"/>
    /// </summary>
    private string _statsHeaderMouse = "Mouse";

    /// <summary>
    /// Gets or sets the header of hte mouse stats
    /// </summary>
    public string StatsHeaderMouse
    {
        get => _statsHeaderMouse;
        set => SetProperty(ref _statsHeaderMouse, value);
    }

    /// <summary>
    /// Backing field for <see cref="AlwaysOnTop"/>
    /// </summary>
    private bool _alwaysOnTop;

    /// <summary>
    /// Gets or sets the value which indicates if the application should be always on top
    /// </summary>
    public bool AlwaysOnTop
    {
        get => _alwaysOnTop;
        set
        {
            if (!SetProperty(ref _alwaysOnTop, value) || Application.Current.MainWindow == null)
                return;

            Application.Current.MainWindow.Topmost = value;
            Properties.Settings.Default.AlwaysOnTop = value;
            Properties.Settings.Default.Save();
        }
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