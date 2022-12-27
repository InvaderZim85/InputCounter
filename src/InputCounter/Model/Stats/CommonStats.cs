using CommunityToolkit.Mvvm.ComponentModel;

namespace InputCounter.Model.Stats;

/// <summary>
/// Represents the common statistics
/// </summary>
internal class CommonStats : ObservableObject
{
    /// <summary>
    /// Backing field for <see cref="TotalCount"/>
    /// </summary>
    private string _totalCount = "/";

    /// <summary>
    /// Gets or sets the total count
    /// </summary>
    public string TotalCount
    {
        get => _totalCount;
        set => SetProperty(ref _totalCount, value);
    }

    /// <summary>
    /// Backing field for <see cref="MaxCount"/>
    /// </summary>
    private string _maxCount = "/";

    /// <summary>
    /// Gets or sets the max. count
    /// </summary>
    public string MaxCount
    {
        get => _maxCount;
        set => SetProperty(ref _maxCount, value);
    }

    /// <summary>
    /// Backing field for <see cref="AverageCount"/>
    /// </summary>
    private string _averageCount = "/";

    /// <summary>
    /// Gets or sets the average count
    /// </summary>
    public string AverageCount
    {
        get => _averageCount;
        set => SetProperty(ref _averageCount, value);
    }
}