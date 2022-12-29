using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyCountImport.Models.KeyCount;

/// <summary>
/// Represents a day key count entry
/// </summary>
[Table("DayKeyCount")]
internal class DayKeyCountDbModel
{
    /// <summary>
    /// Gets or sets the id of the entry
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the day of the entry
    /// </summary>
    public DateTime Day { get; set; }

    /// <summary>
    /// Gets or sets the count of the day
    /// </summary>
    public int Count { get; set; }
}