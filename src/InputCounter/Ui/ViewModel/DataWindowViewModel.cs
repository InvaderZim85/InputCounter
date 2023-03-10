using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using InputCounter.Business;
using InputCounter.Common;
using InputCounter.Model;

namespace InputCounter.Ui.ViewModel;

/// <summary>
/// Provides the logic for the <see cref="View.DataWindow"/>
/// </summary>
internal class DataWindowViewModel : ViewModelBase
{
    /// <summary>
    /// The instance for the interaction with the data
    /// </summary>
    private readonly InputDataManager _manager = new();

    /// <summary>
    /// Backing field for <see cref="KeyboardClickCount"/>
    /// </summary>
    private ObservableCollection<KeyboardClickCountDbModel> _keyboardClickCount = new();

    /// <summary>
    /// Gets or sets the keyboard click count
    /// </summary>
    public ObservableCollection<KeyboardClickCountDbModel> KeyboardClickCount
    {
        get => _keyboardClickCount;
        private set => SetProperty(ref _keyboardClickCount, value);
    }

    /// <summary>
    /// Backing field for <see cref="AverageKeyboardClickCount"/>
    /// </summary>
    private int _averageKeyboardClickCount;

    /// <summary>
    /// Gets or sets the average keyboard click count
    /// </summary>
    public int AverageKeyboardClickCount
    {
        get => _averageKeyboardClickCount;
        set => SetProperty(ref _averageKeyboardClickCount, value);
    }

    /// <summary>
    /// Backing field for <see cref="KeyList"/>
    /// </summary>
    private ObservableCollection<KeyboardKeyCountDbModel> _keyList = new();

    /// <summary>
    /// Gets or sets the keyboard list
    /// </summary>
    public ObservableCollection<KeyboardKeyCountDbModel> KeyList
    {
        get => _keyList;
        private set => SetProperty(ref _keyList, value);
    }

    /// <summary>
    /// Backing field for <see cref="MouseClickCount"/>
    /// </summary>
    private ObservableCollection<MouseClickCountDbModel> _mouseClickCount = new();

    /// <summary>
    /// Gets or sets the mouse click count
    /// </summary>
    public ObservableCollection<MouseClickCountDbModel> MouseClickCount
    {
        get => _mouseClickCount;
        private set => SetProperty(ref _mouseClickCount, value);
    }

    /// <summary>
    /// Backing field for <see cref="AverageMouseClickCount"/>
    /// </summary>
    private int _averageMouseClickCount;

    /// <summary>
    /// Gets or sets the average mouse click count
    /// </summary>
    public int AverageMouseClickCount
    {
        get => _averageMouseClickCount;
        set => SetProperty(ref _averageMouseClickCount, value);
    }

    /// <summary>
    /// The command to reload the data
    /// </summary>
    public ICommand ReloadCommand => new AsyncRelayCommand(LoadDataAsync);

    /// <summary>
    /// Loads the data
    /// </summary>
    public async Task LoadDataAsync()
    {
        var controller = await ShowProgress("Loading", "Please wait while loading the data...");

        try
        {
            KeyboardClickCount = await Helper.ExecuteLoadFuncAsync(_manager.LoadClickCountKeyboardAsync);
            AverageKeyboardClickCount = (int)KeyboardClickCount.Average(a => a.Count);

            KeyList = await Helper.ExecuteLoadFuncAsync(_manager.LoadKeyCountAsync);

            MouseClickCount = await Helper.ExecuteLoadFuncAsync(_manager.LoadClickCountMouseAsync);
            AverageMouseClickCount = (int)MouseClickCount.Average(a => a.TotalCount);
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
}