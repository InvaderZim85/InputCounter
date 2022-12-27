using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using InputCounter.Common.Enums;
using InputCounter.Common.Hook;

namespace InputCounter.Hooks;

/// <summary>
/// Provides the global mouse hook.
/// <para />
/// Source: <a href="https://github.com/rvknth043/Global-Low-Level-Key-Board-And-Mouse-Hook">rvknth043/Global-Low-Level-Key-Board-And-Mouse-Hook</a>
/// </summary>
internal sealed class GlobalMouseHook
{
    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WhMouseLl = 14;
    private static IntPtr _hookId = IntPtr.Zero;

    private readonly LowLevelMouseProc _hook;

    /// <summary>
    /// Occurs when a mouse event raised like a button click
    /// </summary>
    public event EventHandler<CustomMouseEventArgs>? MouseEvent;

    /// <summary>
    /// Creates a new instance of the <see cref="GlobalMouseHook"/>
    /// </summary>
    public GlobalMouseHook()
    {
        _hook = HookCallback;
    }

    /// <summary>
    /// Starts the hook
    /// </summary>
    internal void Start()
    {
        _hookId = SetHook(_hook);
    }

    /// <summary>
    /// Stops the hook
    /// </summary>
    internal void Stop()
    {
        UnhookWindowsHookEx(_hookId);
    }

    /// <summary>
    /// Init the hook
    /// </summary>
    /// <param name="proc">The procedures</param>
    /// <returns>The hook</returns>
    /// <exception cref="Win32Exception">Will be thrown when the hooks is zero</exception>
    private static IntPtr SetHook(LowLevelMouseProc proc)
    {
        var hook = SetWindowsHookEx(WhMouseLl, proc, GetModuleHandle("user32"), 0);
        if (hook == IntPtr.Zero)
        {
            throw new Win32Exception();
        }

        return hook;
    }

    /// <summary>
    /// Will be executes when a mouse action occurs
    /// </summary>
    /// <param name="nCode">The key code</param>
    /// <param name="wParam">The w param</param>
    /// <param name="lParam">The l param</param>
    /// <returns>The pointer</returns>
    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode < 0)
        {
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        MouseEvent?.Invoke(this, new CustomMouseEventArgs
        {
            Action = (MouseActionType)wParam
        });

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    /// <summary>
    /// Finalizer
    /// </summary>
    ~GlobalMouseHook()
    {
        Stop();
    }
}