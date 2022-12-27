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
}