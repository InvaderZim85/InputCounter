using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputCounter.Model;

/// <summary>
/// Represents a keyboard key count entry
/// </summary>
[Table("KeyboardKeyCount")]
internal sealed class KeyboardKeyCountDbModel
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
    /// Gets or sets the name of the key (readable format)
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the key count
    /// </summary>
    public int Count { get; set; }
}