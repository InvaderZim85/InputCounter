using KeyCountImport.Data;
using KeyCountImport.Models.Common;
using KeyCountImport.Models.InputCount;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KeyCountImport.Business;

/// <summary>
/// Provides the functions for the interaction with the two databases
/// </summary>
internal class ImportManager
{
    /// <summary>
    /// Contains the arguments
    /// </summary>
    private readonly Arguments _arguments;

    /// <summary>
    /// Creates a new instance of the <see cref="ImportManager"/>
    /// </summary>
    /// <param name="arguments">The arguments</param>
    public ImportManager(Arguments arguments)
    {
        _arguments = arguments;
    }

    /// <summary>
    /// Imports the data of the key count database into the input counter database
    /// </summary>
    /// <returns>The awaitable task</returns>
    public async Task ImportAsync()
    {
        // Step 1: Prepare the contexts
        await using var sourceContext = new KeyCountDbContext(_arguments.Source);
        await using var targetContext = new InputCounterDbContext(_arguments.Target);

        // Step 2: Import the click count
        await ImportClickCountAsync(sourceContext, targetContext);

        // Step 3: Import the key list
        await ImportKeyCountAsync(sourceContext, targetContext);
    }

    /// <summary>
    /// Imports key click count
    /// </summary>
    /// <param name="sourceContext">The source context</param>
    /// <param name="targetContext">The target context</param>
    /// <returns>The awaitable task</returns>
    private async Task ImportClickCountAsync(KeyCountDbContext sourceContext, InputCounterDbContext targetContext)
    {
        Log.Information("Import click count");
        var sourceData = await sourceContext.ClickCount.AsNoTracking().ToListAsync();

        var count = 1;
        foreach (var entry in sourceData)
        {
            Log.Verbose("Import {count} of {total}. Day: {day}", count++, sourceData.Count,
                entry.Day.ToString("dd.MM.yyyy"));

            var existingEntry = await targetContext.ClickCount.FirstOrDefaultAsync(f => f.Day.Date == entry.Day.Date);
            if (existingEntry != null)
            {
                if (_arguments.Override)
                {
                    existingEntry.Count = entry.Count;
                }
                else
                {
                    existingEntry.Count += entry.Count;
                }
            }
            else
            {
                await targetContext.ClickCount.AddAsync(new KeyboardClickCountDbModel
                {
                    Day = entry.Day,
                    Count = entry.Count
                });
            }
        }

        await targetContext.SaveChangesAsync();
    }

    /// <summary>
    /// Imports key count
    /// </summary>
    /// <param name="sourceContext">The source context</param>
    /// <param name="targetContext">The target context</param>
    /// <returns>The awaitable task</returns>
    private async Task ImportKeyCountAsync(KeyCountDbContext sourceContext, InputCounterDbContext targetContext)
    {
        Log.Information("Import key list");
        var sourceData = await sourceContext.KeyCount.AsNoTracking().ToListAsync();

        var count = 1;
        foreach (var entry in sourceData)
        {
            Log.Verbose("Import {count} of {total}. Key: {key}", count++, sourceData.Count,
                entry.Key);

            var existingEntry = await targetContext.KeyCount.FirstOrDefaultAsync(f => f.KeyCode == entry.KeyCode);
            if (existingEntry != null)
            {
                if (_arguments.Override)
                {
                    existingEntry.Count = entry.Count;
                }
                else
                {
                    existingEntry.Count += entry.Count;
                }
            }
            else
            {
                await targetContext.KeyCount.AddAsync(new KeyboardKeyCountDbModel
                {
                    KeyCode = entry.KeyCode,
                    Key = entry.Key,
                    Count = entry.Count
                });
            }
        }

        await targetContext.SaveChangesAsync();
    }
}