using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InputCounter.Business;
using InputCounter.Common.Enums;
using InputCounter.Model.ViewProperty;
using InputCounter.Ui.View;

namespace InputCounter.Ui.ViewModel;

/// <summary>
/// Provides the logic for <see cref="MainWindow"/>
/// </summary>
internal sealed partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// The instance for the interaction with the data
    /// </summary>
    private readonly DataManager _dataManager = new();

    /// <summary>
    /// Gets or sets the properties which are used by the <see cref="MainWindow"/>
    /// </summary>
    [ObservableProperty]
    private MainWindowProperties _viewProperties = new();

    #region Commands

    /// <summary>
    /// Opens the data window.
    /// </summary>
    [RelayCommand]
    private void ShowData()
    {
        if (ViewProperties.AlwaysOnTop && Application.Current.MainWindow != null)
            Application.Current.MainWindow.Topmost = false;

        var dialog = new DataWindow { Owner = Application.Current.MainWindow };
        dialog.ShowDialog();

        if (ViewProperties.AlwaysOnTop && Application.Current.MainWindow != null)
            Application.Current.MainWindow.Topmost = true;
    }
    #endregion

    /// <summary>
    /// Init the view model
    /// </summary>
    public async void InitViewModel()
    {
        try
        {
            ViewProperties.WindowHeader = $"Input Counter - v{Assembly.GetExecutingAssembly().GetName().Version}";

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
                ViewProperties.KeyboardStats = _dataManager.Statistics.KeyboardStats;
                ViewProperties.MouseStats = _dataManager.Statistics.MouseStats;

                ViewProperties.StatsHeader = $"Statistics - Last update: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
                ViewProperties.StatsHeaderKeyboard = string.IsNullOrEmpty(ViewProperties.KeyboardStats.DateRange)
                    ? "Keyboard"
                    : $"Keyboard {ViewProperties.KeyboardStats.DateRange}";
                ViewProperties.StatsHeaderMouse = string.IsNullOrEmpty(ViewProperties.MouseStats.DateRange)
                    ? "Mouse"
                    : $"Mouse {ViewProperties.MouseStats.DateRange}";
            };

            _dataManager.Start();
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
                ViewProperties.TotalCountKeyboard = _dataManager.ClickCountValues.TodayKeyboardComplete;
                ViewProperties.PreviousCountKeyboard = _dataManager.ClickCountValues.PreviousKeyboard.ToString("N0");

                // Progress bar
                var keyMaxReached = _dataManager.ClickCountValues.TodayKeyboard >
                                    _dataManager.ClickCountValues.PreviousKeyboard;
                ViewProperties.ProgressMaxKeyboard = keyMaxReached ? 1 : _dataManager.ClickCountValues.PreviousKeyboard;
                ViewProperties.ProgressCurrentKeyboard = keyMaxReached ? 0 : _dataManager.ClickCountValues.TodayKeyboard;
                ViewProperties.TotalCountPercentageKeyboard = _dataManager.ClickCountValues.PercentageKeyboard == 0
                    ? string.Empty
                    : $"{_dataManager.ClickCountValues.PercentageKeyboard:N2}%";
                break;
            case StatisticType.Mouse:
                // Display values
                ViewProperties.TotalCountMouse = _dataManager.ClickCountValues.TodayMouseComplete;
                ViewProperties.PreviousCountMouse = _dataManager.ClickCountValues.PreviousMouseComplete;

                // Progress bar
                var mouseMaxReached = _dataManager.ClickCountValues.TodayMouse >
                                      _dataManager.ClickCountValues.PreviousMouse;
                ViewProperties.ProgressMaxMouse = mouseMaxReached ? 1 : _dataManager.ClickCountValues.PreviousMouse;
                ViewProperties.ProgressCurrentMouse = mouseMaxReached ? 0 : _dataManager.ClickCountValues.TodayMouse;
                ViewProperties.TotalCountPercentageMouse = _dataManager.ClickCountValues.PercentageMouse == 0
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
            await DataManager.PrepareDatabaseAsync();
            
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