namespace InputCounter.Common.Enums;

/// <summary>
/// Provides the different mouse actions
/// </summary>
public enum MouseActionType
{
    /// <summary>
    /// Left button down
    /// </summary>
    LeftButtonDown = 0x0201,

    /// <summary>
    /// Left button up
    /// </summary>
    LeftButtonUp = 0x0202,

    /// <summary>
    /// Movement
    /// </summary>
    Move = 0x0200,

    /// <summary>
    /// Wheel
    /// </summary>
    Wheel = 0x020A,

    /// <summary>
    /// Right button down
    /// </summary>
    RightButtonDown = 0x0204,

    /// <summary>
    /// Right button up
    /// </summary>
    RightButtonUp = 0x0205,

    /// <summary>
    /// Wheel button down
    /// </summary>
    WheelButtonDown = 0x207,

    /// <summary>
    /// Wheel button up
    /// </summary>
    WheelButtonUp = 0x208,

    /// <summary>
    /// X-Button down
    /// </summary>
    XButtonDown = 0x020B,

    /// <summary>
    /// X-Button up
    /// </summary>
    XButtonUp = 0x020C
}