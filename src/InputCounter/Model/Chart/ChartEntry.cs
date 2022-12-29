namespace InputCounter.Model.Chart;

/// <summary>
/// Represents a chart entry
/// </summary>
internal class ChartEntry
{
    /// <summary>
    /// Gets or sets the value of the entry
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Gets or sets the date ticks
    /// </summary>
    public long DateTicks { get; set; }
}