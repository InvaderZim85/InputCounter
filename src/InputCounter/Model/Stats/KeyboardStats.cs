namespace InputCounter.Model.Stats;

/// <summary>
/// Provides the keyboard stats
/// </summary>
internal sealed class KeyboardStats : CommonStats
{
    /// <summary>
    /// Backing field for <see cref="MostUsedKey"/>
    /// </summary>
    private string _mostUsedKey = "/";

    /// <summary>
    /// Gets or sets the most used key
    /// </summary>
    public string MostUsedKey
    {
        get => _mostUsedKey;
        set => SetProperty(ref _mostUsedKey, value);
    }

    /// <summary>
    /// Backing field for <see cref="LeastUsedKey"/>
    /// </summary>
    private string _leastUsedKey = "/";

    /// <summary>
    /// Gets or sets the least used key
    /// </summary>
    public string LeastUsedKey
    {
        get => _leastUsedKey;
        set => SetProperty(ref _leastUsedKey, value);
    }
}