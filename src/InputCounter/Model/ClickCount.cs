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
    public string TodayKeyboardComplete => CalculateTodayValueKeyboard();

    /// <summary>
    /// Gets the keyboard percentage (today vs. previous)
    /// </summary>
    public double PercentageKeyboard => TodayKeyboard > PreviousKeyboard ? 0 : CalculatePercentage(TodayKeyboard, PreviousKeyboard);

    /// <summary>
    /// Gets or sets the left mouse click count of today
    /// </summary>
    public int TodayMouseLeft { get; set; }

    /// <summary>
    /// Gets or sets the right mouse click count of today
    /// </summary>
    public int TodayMouseRight { get; set; }

    /// <summary>
    /// Gets the mouse click count of today (<see cref="TodayMouseLeft"/> + <see cref="TodayMouseRight"/>)
    /// </summary>
    public int TodayMouse => TodayMouseLeft + TodayMouseRight;

    /// <summary>
    /// Gets or sets the previous (yesterday) left mouse click count
    /// </summary>
    public int PreviousMouseLeft { get; set; }

    /// <summary>
    /// Gets or sets the previous (yesterday) right mouse click count
    /// </summary>
    public int PreviousMouseRight { get; set; }

    /// <summary>
    /// Gets or sets the previous (yesterday) mouse click count (<see cref="PreviousMouseLeft"/> + <see cref="PreviousMouseRight"/>)
    /// </summary>
    public int PreviousMouse => PreviousMouseLeft + PreviousMouseRight;

    /// <summary>
    /// Gets the previous mouse click value
    /// </summary>
    public string PreviousMouseComplete =>
        $"{PreviousMouse:N0} (L: {PreviousMouseLeft:N0}, R: {PreviousMouseRight:N0})";

    /// <summary>
    /// Gets the total mouse click value of today
    /// </summary>
    public string TodayMouseComplete => CalculateTodayValueMouse();

    /// <summary>
    /// Gets the mouse percentage (today vs. previous)
    /// </summary>
    public double PercentageMouse => TodayMouse > PreviousMouse ? 0 : CalculatePercentage(TodayMouse, PreviousMouse);

    /// <summary>
    /// Calculates the today value
    /// </summary>
    /// <returns>The result</returns>
    private string CalculateTodayValueKeyboard()
    {
        if (PreviousKeyboard == 0)
            return TodayKeyboard.ToString("N0");

        var percentage = CalculatePercentage(TodayKeyboard, PreviousKeyboard);
        return $"{TodayKeyboard:N0} - {percentage:N2}%";
    }

    /// <summary>
    /// Calculates the today mouse value
    /// </summary>
    /// <returns>The today value of the mouse</returns>
    private string CalculateTodayValueMouse()
    {
        if (PreviousMouse == 0)
            return $"{TodayMouse:N0} (L: {TodayMouseLeft:N0}, R: {TodayMouseRight:N0})";

        var percentage = CalculatePercentage(TodayMouse, PreviousMouse);
        return $"{TodayMouse:N0} (L: {TodayMouseLeft:N0}, R: {TodayMouseRight:N0}) - {percentage:N2}%";
    }

    /// <summary>
    /// Calculates the percentage between to current and the previous value
    /// </summary>
    /// <param name="today">The value of today</param>
    /// <param name="previous">The previous value</param>
    /// <returns>The percentage</returns>
    private static double CalculatePercentage(int today, int previous)
    {
        if (today == 0 && previous == 0)
            return 0;

        return 100d / previous * today;
    }
}