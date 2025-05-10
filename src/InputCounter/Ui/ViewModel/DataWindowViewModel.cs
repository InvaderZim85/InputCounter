using CommunityToolkit.Mvvm.Input;
using InputCounter.Business;
using InputCounter.Common;
using InputCounter.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace InputCounter.Ui.ViewModel;

/// <summary>
/// Provides the logic for the <see cref="View.DataWindow"/>
/// </summary>
internal sealed partial class DataWindowViewModel : ViewModelBase
{
    /// <summary>
    /// The instance for the interaction with the data
    /// </summary>
    private readonly InputDataManager _manager = new();

    /// <summary>
    /// Gets or sets the click count.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<KeyboardClickCountDbModel> _keyboardClickCount = [];

    /// <summary>
    /// Gets or sets the average keyboard click count
    /// </summary>
    [ObservableProperty]
    private int _averageKeyboardClickCount;

    /// <summary>
    /// Gets or sets the keyboard list
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<KeyboardKeyCountDbModel> _keyList = [];

    /// <summary>
    /// Gets or sets the mouse click count
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MouseClickCountDbModel> _mouseClickCount = [];

    /// <summary>
    /// Gets or sets the average mouse click count
    /// </summary>
    [ObservableProperty] 
    private int _averageMouseClickCount;

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

            KeyList = await Helper.ExecuteLoadFuncAsync(InputDataManager.LoadKeyCountAsync);

            MouseClickCount = await Helper.ExecuteLoadFuncAsync(InputDataManager.LoadClickCountMouseAsync);
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