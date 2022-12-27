using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

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
}