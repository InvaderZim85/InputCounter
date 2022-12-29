using CommandLine;

namespace KeyCountImport.Models.Common;

/// <summary>
/// Represents the given arguments
/// </summary>
internal sealed class Arguments
{
    /// <summary>
    /// Gets or sets the path of the source file (key count)
    /// </summary>
    [Option('s', "source", Required = true, HelpText = "The path of the KeyCount database")]
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path of the target file (input counter)
    /// </summary>
    [Option('t', "target", Required = true, HelpText = "The path of the InputCounter database")]
    public string Target { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value which indicates if the values should be overwritten or not.
    /// <para />
    /// If <see langword="true"/>, the existing values will be overwritten. If <see langword="false"/> the values will be added (old value + new value)
    /// </summary>
    [Option('o', "override", Required = false, Default = false, HelpText = "Add this switch to override the data in the target database")]
    public bool Override { get; set; }

    /// <summary>
    /// Gets or sets the value wich indicates if a verbose log should be created
    /// </summary>
    [Option('v', "verbose", Required = false, HelpText = "Add this switch to activate a verbose log")]
    public bool Verbose { get; set; }
}