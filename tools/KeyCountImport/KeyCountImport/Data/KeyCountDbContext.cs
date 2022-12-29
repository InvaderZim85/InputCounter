using KeyCountImport.Models.KeyCount;
using Microsoft.EntityFrameworkCore;

namespace KeyCountImport.Data;

/// <summary>
/// Provides the functions for the interaction with the key count database
/// </summary>
internal class KeyCountDbContext : DbContext
{
    /// <summary>
    /// Contains the connection string
    /// </summary>
    private readonly string _conString;

    /// <summary>
    /// Gets or sets the click count list
    /// </summary>
    public DbSet<DayKeyCountDbModel> ClickCount => Set<DayKeyCountDbModel>();

    /// <summary>
    /// Gets or sets the key count list
    /// </summary>
    public DbSet<KeyListDbModel> KeyCount => Set<KeyListDbModel>();

    /// <summary>
    /// Creates a new instance of the <see cref="KeyCountDbContext"/>
    /// </summary>
    /// <param name="filePath">The path of the db file</param>
    public KeyCountDbContext(string filePath)
    {
        _conString = $"Data Source = {filePath}";
    }

    /// <summary>
    /// Occurs when the context is configuring
    /// </summary>
    /// <param name="optionsBuilder">The options builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_conString);
    }
}