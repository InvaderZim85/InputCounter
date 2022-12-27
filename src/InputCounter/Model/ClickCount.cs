namespace InputCounter.Model;

/// <summary>
/// Provides the count values
/// </summary>
internal sealed class ClickCount
{
    /// <summary>
    /// Gets or sets the keyboard count of today
    /// </summary>
    public int TodayKeyboard { get; set; }

    /// <summary>
    /// Gets or sets the previous (yesterday) keyboard count
    /// </summary>
    public int PreviousKeyboard { get; set; }

    /// <summary>
    /// Gets the keyboard value of today
    /// </summary>
    public string TodayKeyboardComplete => CalculateTodayValue(TodayKeyboard, PreviousKeyboard);

    /// <summary>
    /// Gets the keyboard percentage (today vs. previous)
    /// </summary>
    public double PercentageKeyboard => TodayKeyboard > PreviousKeyboard ? 0 : CalculatePercentage(TodayKeyboard, PreviousKeyboard);

    /// <summary>
    /// Gets or sets the mouse count of today
    /// </summary>
    public int TodayMouse { get; set; }

    /// <summary>
    /// Gets or sets the previous (yesterday) mouse count
    /// </summary>
    public int PreviousMouse { get; set; }

    /// <summary>
    /// Gets the mouse value of today
    /// </summary>
    public string TodayMouseComplete => CalculateTodayValue(TodayMouse, PreviousMouse);

    /// <summary>
    /// Gets the mouse percentage (today vs. previous)
    /// </summary>
    public double PercentageMouse => TodayMouse > PreviousMouse ? 0 : CalculatePercentage(TodayMouse, PreviousMouse);

    /// <summary>
    /// Calculates the today value
    /// </summary>
    /// <param name="today">The value of today</param>
    /// <param name="previous">The previous value</param>
    /// <returns>The result</returns>
    private static string CalculateTodayValue(int today, int previous)
    {
        if (previous == 0)
            return today.ToString("N0");

        var percentage = CalculatePercentage(today, previous);
        return $"{today:N0} ({percentage:N2}%)";
    }

    /// <summary>
    /// Calculates the percentage between to current and the previous value
    /// </summary>
    /// <param name="today">The value of today</param>
    /// <param name="previous">The previous value</param>
    /// <returns>The percentage</returns>
    private static double CalculatePercentage(int today, int previous)
    {
        return 100d / previous * today;
    }
}