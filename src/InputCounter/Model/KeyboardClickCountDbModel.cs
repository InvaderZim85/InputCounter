using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InputCounter.Model;

/// <summary>
/// Represents a keyboard click count entry
/// </summary>
[Table("KeyboardClickCount")]
internal sealed class KeyboardClickCountDbModel
{
    /// <summary>
    /// Gets or sets the id of the entry
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the day of the click count
    /// </summary>
    public DateTime Day { get; set; }

    /// <summary>
    /// Gets or sets the count of the day
    /// </summary>
    public int Count { get; set; }
}