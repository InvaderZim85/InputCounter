using KeyCountImport.Models.InputCount;
using Microsoft.EntityFrameworkCore;

namespace KeyCountImport.Data;

/// <summary>
/// Provides the functions for the interaction with the input counter database
/// </summary>
internal class InputCounterDbContext : DbContext
{
    /// <summary>
    /// Contains the connection string
    /// </summary>
    private readonly string _conString;

    /// <summary>
    /// Gets or sets the click count list
    /// </summary>
    public DbSet<KeyboardClickCountDbModel> ClickCount => Set<KeyboardClickCountDbModel>();

    /// <summary>
    /// Gets or sets the key count list
    /// </summary>
    public DbSet<KeyboardKeyCountDbModel> KeyCount => Set<KeyboardKeyCountDbModel>();

    /// <summary>
    /// Creates a new instance of the <see cref="InputCounterDbContext"/>
    /// </summary>
    /// <param name="filePath">The path of the db file</param>
    public InputCounterDbContext(string filePath)
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