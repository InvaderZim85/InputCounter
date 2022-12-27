using CommunityToolkit.Mvvm.ComponentModel;

namespace InputCounter.Model.Stats;

/// <summary>
/// Provides the statistics
/// </summary>
internal sealed class Statistics : ObservableObject
{
    /// <summary>
    /// Backing field for <see cref="KeyboardStats"/>
    /// </summary>
    private KeyboardStats _keyboardStats = new();

    /// <summary>
    /// Gets or sets the key board statistics
    /// </summary>
    public KeyboardStats KeyboardStats
    {
        get => _keyboardStats;
        set => SetProperty(ref _keyboardStats, value);
    }

    /// <summary>
    /// Backing field for <see cref="MouseStats"/>
    /// </summary>
    private MouseStats _mouseStats = new();

    /// <summary>
    /// Gets or sets the mouse statistics
    /// </summary>
    public MouseStats MouseStats
    {
        get => _mouseStats;
        set => SetProperty(ref _mouseStats, value);
    }
}