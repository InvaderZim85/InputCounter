using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyCountImport.Models.KeyCount;

/// <summary>
/// Represents a key list entry
/// </summary>
[Table("KeyList")]
internal class KeyListDbModel
{
    /// <summary>
    /// Gets or sets the id of the entry
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the code of the key
    /// </summary>
    public int KeyCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the key
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the count of the key
    /// </summary>
    public int Count { get; set; }
}