using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using InputCounter.Business;
using InputCounter.Common.Enums;
using InputCounter.Model.Stats;
using InputCounter.Ui.View;

namespace InputCounter.Ui.ViewModel;

/// <summary>
/// Provides the logic for <see cref="View.MainWindow"/>
/// </summary>
internal sealed class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// The instance for the interaction with the data
    /// </summary>
    private readonly DataManager _dataManager = new();

    #region ViewProperties

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
        private set => SetProperty(ref _progressMaxKeyboard, value);
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
        private set => SetProperty(ref _keyboardStats, value);
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
        private set => SetProperty(ref _progressMaxMouse, value);
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
        private set => SetProperty(ref _mouseStats, value);
    }
    #endregion

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
        private set => SetProperty(ref _statsHeader, value);
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

    #region Commands

    /// <summary>
    /// The command to open the data window
    /// </summary>
    public ICommand ShowDataCommand => new RelayCommand(() =>
    {
        if (AlwaysOnTop && Application.Current.MainWindow != null)
            Application.Current.MainWindow.Topmost = false;

        var dialog = new DataWindow(){Owner = Application.Current.MainWindow};
        dialog.ShowDialog();

        if (AlwaysOnTop && Application.Current.MainWindow != null)
            Application.Current.MainWindow.Topmost = true;
    });
    #endregion

    /// <summary>
    /// Init the view model
    /// </summary>
    public async void InitViewModel()
    {
        try
        {
            // Add the events
            _dataManager.KeyboardClicked += (_, _) =>
            {
                SetValues(StatisticType.Keyboard);
            };

            _dataManager.MouseClicked += (_, _) =>
            {
                SetValues(StatisticType.Mouse);
            };

            _dataManager.StatsUpdated += (_, _) =>
            {
                KeyboardStats = _dataManager.Statistics.KeyboardStats;
                MouseStats = _dataManager.Statistics.MouseStats;

                StatsHeader = $"Statistics - Last update: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
            };

            _dataManager.Start();

            AlwaysOnTop = Properties.Settings.Default.AlwaysOnTop;
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
        }
    }

    /// <summary>
    /// Sets the values
    /// </summary>
    /// <param name="type">The stats type</param>
    private void SetValues(StatisticType type)
    {
        switch (type)
        {
            case StatisticType.Keyboard:
                // Display values
                TotalCountKeyboard = _dataManager.ClickCountValues.TodayKeyboardComplete;
                PreviousCountKeyboard = _dataManager.ClickCountValues.PreviousKeyboard.ToString("N0");

                // Progress bar
                var keyMaxReached = _dataManager.ClickCountValues.TodayKeyboard >
                                    _dataManager.ClickCountValues.PreviousKeyboard;
                ProgressMaxKeyboard = keyMaxReached ? 1 : _dataManager.ClickCountValues.PreviousKeyboard;
                ProgressCurrentKeyboard = keyMaxReached ? 0 : _dataManager.ClickCountValues.TodayKeyboard;
                TotalCountPercentageKeyboard = _dataManager.ClickCountValues.PercentageKeyboard == 0
                    ? string.Empty
                    : $"{_dataManager.ClickCountValues.PercentageKeyboard:N2}%";
                break;
            case StatisticType.Mouse:
                // Display values
                TotalCountMouse = _dataManager.ClickCountValues.TodayMouseComplete;
                PreviousCountMouse = _dataManager.ClickCountValues.PreviousMouse.ToString("N0");

                // Progress bar
                var mouseMaxReached = _dataManager.ClickCountValues.TodayMouse >
                                      _dataManager.ClickCountValues.PreviousMouse;
                ProgressMaxMouse = mouseMaxReached ? 1 : _dataManager.ClickCountValues.PreviousMouse;
                ProgressCurrentMouse = mouseMaxReached ? 0 : _dataManager.ClickCountValues.TodayMouse;
                TotalCountPercentageMouse = _dataManager.ClickCountValues.PercentageMouse == 0
                    ? string.Empty
                    : $"{_dataManager.ClickCountValues.PercentageMouse:N2}%";
                break;
        }
    }

    /// <summary>
    /// Init the database
    /// </summary>
    public async void InitData()
    {
        var controller = await ShowProgress("Init", "Initializing database...");

        try
        {
            await _dataManager.PrepareDatabaseAsync();
            
            // Load / set the counts
            await _dataManager.LoadCountsAsync();
            SetValues(StatisticType.Keyboard);
            SetValues(StatisticType.Mouse);

            // Load the statistics
            await _dataManager.LoadStatisticsAsync();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
        }
        finally
        {
            await controller.CloseAsync();
        }
    }

    /// <summary>
    /// Occurs when the view model will be closed
    /// </summary>
    public void CloseViewModel()
    {
        _dataManager.Stop();
    }
}