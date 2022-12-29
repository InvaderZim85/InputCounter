using InputCounter.Model;
using System.Collections.ObjectModel;
using InputCounter.Business;
using InputCounter.Common;
using System.Threading.Tasks;
using System;
using System.Linq;
using InputCounter.Model.Chart;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace InputCounter.Ui.ViewModel;

/// <summary>
/// Provides the logic for the <see cref="View.KeyboardCountControl"/>
/// </summary>
internal class KeyboardCountControlViewModel : ViewModelBase
{
    /// <summary>
    /// The instance for the interaction with the input data
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
    /// Backing field for <see cref="ChartSeries"/>
    /// </summary>
    private ISeries[] _chartSeries = new ISeries[] {};

    /// <summary>
    /// Gets or sets the chart series
    /// </summary>
    public ISeries[] ChartSeries
    {
        get => _chartSeries;
        set => SetProperty(ref _chartSeries, value);
    }

    /// <summary>
    /// Backing field for <see cref="AxesX"/>
    /// </summary>
    private Axis[] _axesX = new Axis[]{};

    /// <summary>
    /// Gets or sets the 
    /// </summary>
    public Axis[] AxesX
    {
        get => _axesX;
        set => SetProperty(ref _axesX, value);
    }

    /// <summary>
    /// Loads the data
    /// </summary>
    public async Task LoadDataAsync()
    {
        var controller = await ShowProgress("Loading", "Please wait while loading the data...");

        try
        {
            KeyboardClickCount = await Helper.ExecuteLoadFuncAsync(_manager.LoadClickCountKeyboardAsync);

            // Create the series
            var tmpSeries = new LineSeries<ChartEntry>
            {
                Name = "Date",
                TooltipLabelFormatter = point => $"{point.Model!.DateTicks.ToChartDate()} - {point.PrimaryValue:N0}",
                Values = KeyboardClickCount.OrderBy(o => o.Day).Select(s => new ChartEntry
                {
                    Value = s.Count,
                    DateTicks = s.Day.Ticks
                }),
                Mapping = (entry, point) =>
                {
                    point.PrimaryValue = entry.Value;
                    point.SecondaryValue = entry.DateTicks;
                },
                LineSmoothness = 0,
                
            };

            ChartSeries = new ISeries[] { tmpSeries };

            // Create the axis
            var axis = new Axis
            {
                Name = "Date",
                Labeler = value => value.ToChartDate(),
                MinStep = 1
            };

            AxesX = new Axis[] { axis };
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