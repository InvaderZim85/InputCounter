using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InputCounter.Model;

/// <summary>
/// Represents a mouse click entry
/// </summary>
[Table("MouseClickCount")]
internal sealed class MouseClickCountDbModel
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
    /// Gets or sets the count of the day for a left click
    /// </summary>
    public int LeftCount { get; set; }

    /// <summary>
    /// Gets or sets the count of the day for a right click
    /// </summary>
    public int RightCount { get; set; }
}