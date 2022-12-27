using InputCounter.Common.Enums;
using System;

namespace InputCounter.Common.Hook;

/// <summary>
/// Provides the mouse events
/// </summary>
internal class CustomMouseEventArgs : EventArgs
{
    /// <summary>
    /// Gets the mouse action (like left button down / left button up, ...)
    /// </summary>
    public MouseActionType Action { get; init; }
}