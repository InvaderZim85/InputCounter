using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Windows;

namespace InputCounter.Common;

/// <summary>
/// Provides several helper functions
/// </summary>
internal static class Helper
{
    /// <summary>
    /// Init the logger
    /// </summary>
    public static void InitLogger()
    {
        Log.Logger = new LoggerConfiguration().WriteTo.File("logs/log_.log", rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }

    /// <summary>
    /// Executes the loading function
    /// </summary>
    /// <typeparam name="T">The type of the data</typeparam>
    /// <param name="func">The function</param>
    /// <returns>The collection</returns>
    public static async Task<ObservableCollection<T>> ExecuteLoadFuncAsync<T>(Func<Task<List<T>>> func)
    {
        var tmpData = await func();
        return tmpData.ToObservableCollection();
    }

    /// <summary>
    /// Sets the position of the window
    /// </summary>
    /// <param name="window">The window</param>
    public static void SetWindowPosition(Window window)
    {
        /*
         * Note: We will check the saved screen position before setting it,
         *       because we want to avoid setting the position to an invisible area
         *
         * Example (three displays with 1920x1080 resolution):
         *
         *    +---------+---------+---------+ 0
         *    |         |         |         |
         *    |    L    |    M    |    R    |
         *    |         |         |         |
         *    +---------+---------+---------+ 1080
         *  -1920       0        1920      3840
         *
         */
        var settings = Properties.Settings.Default;
        if (settings.WindowPosX == 0 || settings.WindowPosY == 0)
        {
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            // NOTE: The following code is needed for setups with multiple displays
            
            // Before we set the window position, let's check if the position is within the visible area
            // Step 1: Check the left (only when the x-position is negative)
            if (settings.WindowPosX < 0)
            {
                // The window was closed when it was on the left screen
                var left = SystemParameters.VirtualScreenLeft; // the max. left position
                if (left == 0) // There is no "left display"
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    return;
                }

                if (settings.WindowPosX > left) // The saved position is outside the visible area
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    return;
                }
            }
            else if (settings.WindowPosX >= 0)
            {
                // Now we will check the right
                var right = SystemParameters.VirtualScreenWidth - (SystemParameters.VirtualScreenLeft < 0
                    ? SystemParameters.VirtualScreenLeft * -1
                    : 0);
                if (settings.WindowPosX > right) // The saved position is outside the visible area
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    return;
                }
            }

            // Now check the height. The height does not change so quickly,
            // but to be on the safe side we will check it as well.
            var height = SystemParameters.VirtualScreenHeight;
            if (settings.WindowPosY > height)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                return;
            }

            // The checks were successful, so set the saved position
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = settings.WindowPosX;
            window.Top = settings.WindowPosY;
        }
    }

    /// <summary>
    /// Saves the window position
    /// </summary>
    /// <param name="window">The window</param>
    public static void SaveWindowPosition(Window window)
    {
        Properties.Settings.Default.WindowPosX = window.Left;
        Properties.Settings.Default.WindowPosY = window.Top;
        Properties.Settings.Default.Save();
    }
}