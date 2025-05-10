using System;
using System.Windows.Input;

namespace InputCounter.Common.Hook;

/// <summary>
/// Provides the keyboard events.
/// </summary>
/// <param name="key">The key which was pressed.</param>
public class KeyPressedArgs(Key key) : EventArgs
{
    /// <summary>
    /// Gets the key which was pressed.
    /// </summary>
    public Key Key { get; } = key;
}