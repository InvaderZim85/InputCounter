using Serilog;

namespace KeyCountImport.Common;

/// <summary>
/// Provides several helper functions
/// </summary>
internal static class Helper
{
    /// <summary>
    /// Init the logger
    /// </summary>
    /// <param name="verbose">true to create verbose log, otherwise false</param>
    public static void InitLogger(bool verbose)
    {
        const string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
        if (verbose)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: template)
                .WriteTo.File("log/log_.log", rollingInterval: RollingInterval.Day, outputTemplate: template)
                .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: template)
                .WriteTo.File("log/log_.log", rollingInterval: RollingInterval.Day, outputTemplate: template)
                .CreateLogger();
        }
        
    }
}