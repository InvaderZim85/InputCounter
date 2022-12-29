using System.Diagnostics;
using CommandLine;
using KeyCountImport.Business;
using KeyCountImport.Common;
using KeyCountImport.Models.Common;
using Serilog;

namespace KeyCountImport;

/// <summary>
/// Provides the main logic of the program
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point
    /// </summary>
    /// <param name="args">The provided arguments</param>
    private static async Task Main(string[] args)
    {
        var arguments = ExtractArguments(args);

        Helper.InitLogger(arguments?.Verbose ?? false);

        if (arguments == null)
        {
            Log.Error("Arguments missing.");
            return;
        }

        if (!File.Exists(arguments.Source) || !File.Exists(arguments.Target))
        {
            Log.Error("Source and / or target file doesn't exist.");
            return;
        }

        // Print the arguments
        Log.Information("Arguments:");
        Log.Information("- Source: {source}", arguments.Source);
        Log.Information("- Target: {target}", arguments.Target);
        Log.Information("- Option: {option}", arguments.Override ? "Override" : "Add");

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        try
        {
            var importManager = new ImportManager(arguments);
            await importManager.ImportAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "A fatal error has occurred.");
        }
        finally
        {
            stopwatch.Stop();
            Log.Information("Import done in {duration}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Extracts the arguments
    /// </summary>
    /// <param name="args">The provided arguments</param>
    /// <returns>The arguments</returns>
    private static Arguments? ExtractArguments(IEnumerable<string> args)
    {
        Arguments? result = null;

        Parser.Default.ParseArguments<Arguments>(args).WithParsed(a => result = a);

        return result;
    }
}