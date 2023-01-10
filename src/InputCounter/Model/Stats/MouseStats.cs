namespace InputCounter.Model.Stats;

internal sealed class MouseStats : CommonStats
{
    /// <summary>
    /// Backing field for <see cref="LeftCount"/>
    /// </summary>
    private string _leftCount = "/";

    /// <summary>
    /// Gets or sets the count of the left mouse button
    /// </summary>
    public string LeftCount
    {
        get => _leftCount;
        set => SetProperty(ref _leftCount, value);
    }

    /// <summary>
    /// Backing field for <see cref="RightCount"/>
    /// </summary>
    private string _rightCount = "/";

    /// <summary>
    /// Gets or sets the count of the right mouse button
    /// </summary>
    public string RightCount
    {
        get => _rightCount;
        set => SetProperty(ref _rightCount, value);
    }
    
    /// <summary>
    /// Backing field for <see cref="MaxCountLeft"/>
    /// </summary>
    private string _maxCountLeft = "/";

    /// <summary>
    /// Gets or sets the max. count
    /// </summary>
    public string MaxCountLeft
    {
        get => _maxCountLeft;
        set => SetProperty(ref _maxCountLeft, value);
    }
    
    /// <summary>
    /// Backing field for <see cref="MaxCountRight"/>
    /// </summary>
    private string _maxCountRight = "/";

    /// <summary>
    /// Gets or sets the max. count
    /// </summary>
    public string MaxCountRight
    {
        get => _maxCountRight;
        set => SetProperty(ref _maxCountRight, value);
    }

    /// <summary>
    /// Backing field for <see cref="AverageCountLeft"/>
    /// </summary>
    private string _averageCountLeft = "/";

    /// <summary>
    /// Gets or sets the average left count
    /// </summary>
    public string AverageCountLeft
    {
        get => _averageCountLeft;
        set => SetProperty(ref _averageCountLeft, value);
    }

    /// <summary>
    /// Backing field for <see cref="AverageCountRight"/>
    /// </summary>
    private string _averageCountRight = "/";

    /// <summary>
    /// Gets or sets the average right count
    /// </summary>
    public string AverageCountRight
    {
        get => _averageCountRight;
        set => SetProperty(ref _averageCountRight, value);
    }
}