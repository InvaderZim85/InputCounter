using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InputCounter.Common.Enums;
using InputCounter.Model;
using InputCounter.Model.Stats;
using Microsoft.EntityFrameworkCore;
using ZimLabs.CoreLib;

namespace InputCounter.Data;

/// <summary>
/// Provides the functions for the interaction with the "database"
/// </summary>
internal sealed class AppDbContext : DbContext
{
    #region DbSets
    /// <summary>
    /// Gets or sets the keyboard click count table
    /// </summary>
    public DbSet<KeyboardClickCountDbModel> KeyboardClickCount => Set<KeyboardClickCountDbModel>();

    /// <summary>
    /// Gets or sets the keyboard key count table
    /// </summary>
    public DbSet<KeyboardKeyCountDbModel> KeyboardKeyCount => Set<KeyboardKeyCountDbModel>();

    /// <summary>
    /// Gets or sets the mouse click count table
    /// </summary>
    public DbSet<MouseClickCountDbModel> MouseClickCount => Set<MouseClickCountDbModel>();
    #endregion

    #region Keyboard
    /// <summary>
    /// Prepares the key count table an adds an empty entry for all missing days
    /// </summary>
    /// <returns>The awaitable task</returns>
    public async Task PrepareKeyCountTableAsync()
    {
        // Check if the table contains any data
        if (!await KeyboardClickCount.AsNoTracking().AnyAsync())
            return;

        var minDate = await KeyboardClickCount.AsNoTracking().MinAsync(m => m.Day);
        var maxDate = await KeyboardClickCount.AsNoTracking().MaxAsync(m => m.Day);

        // Skip the stuff if the dates are equal
        if (minDate.Date == maxDate.Date) 
            return;

        var tmpDate = minDate.Date;
        var tmpList = await KeyboardClickCount.AsNoTracking().Select(s => s.Day.Date).ToListAsync();

        var hasChanges = false;
        while (tmpDate < maxDate)
        {
            // Check if an entry exists with the day
            if (tmpList.Any(a => a == tmpDate.Date))
            {
                // An entry exists, skip the entry
                tmpDate = tmpDate.AddDays(1);
                continue;
            }

            if (!hasChanges)
                hasChanges = true;

            // Add a new "empty" entry
            await KeyboardClickCount.AddAsync(new KeyboardClickCountDbModel
            {
                Day = tmpDate.Date,
                Count = 0
            });

            // Add a new day
            tmpDate = tmpDate.AddDays(1);
        }

        if (hasChanges)
            await SaveChangesAsync();
    }

    /// <summary>
    /// Adds a new key
    /// </summary>
    /// <param name="key">The key which should be added</param>
    /// <returns>The awaitable task</returns>
    public async Task InsertKeyAsync(Key key)
    {
        // Update the key count
        var countEntry = await KeyboardClickCount.FirstOrDefaultAsync(f => f.Day.Date == DateTime.Now.Date);
        if (countEntry == null)
        {
            await KeyboardClickCount.AddAsync(new KeyboardClickCountDbModel
            {
                Day = DateTime.Now.Date,
                Count = 1
            });
        }
        else
        {
            countEntry.Count++;
        }

        // Update the key list
        var keyEntry = await KeyboardKeyCount.FirstOrDefaultAsync(f => f.KeyCode == (int)key);
        if (keyEntry == null)
        {
            await KeyboardKeyCount.AddAsync(new KeyboardKeyCountDbModel
            {
                KeyCode = (int)key,
                Key = key.ToString(),
                Count = 1
            });
        }
        else
        {
            keyEntry.Count++;
        }

        await SaveChangesAsync();
    }
    #endregion

    #region Mouse
    /// <summary>
    /// Prepares the mouse count table an adds an empty entry for all missing days
    /// </summary>
    /// <returns>The awaitable task</returns>
    public async Task PrepareMouseCountTableAsync()
    {
        // Check if the table contains any data
        if (!await MouseClickCount.AsNoTracking().AnyAsync())
            return;

        var minDate = await MouseClickCount.AsNoTracking().MinAsync(m => m.Day);
        var maxDate = await MouseClickCount.AsNoTracking().MaxAsync(m => m.Day);

        // Skip the stuff if the dates are equal
        if (minDate.Date == maxDate.Date)
            return;

        var tmpDate = minDate.Date;
        var tmpList = await MouseClickCount.AsNoTracking().Select(s => s.Day.Date).ToListAsync();

        var hasChanges = false;
        while (tmpDate < maxDate)
        {
            // Check if an entry exists with the day
            if (tmpList.Any(a => a == tmpDate.Date))
            {
                // An entry exists, skip the entry
                tmpDate = tmpDate.AddDays(1);
                continue;
            }

            if (!hasChanges)
                hasChanges = true;

            // Add a new "empty" entry
            await MouseClickCount.AddAsync(new MouseClickCountDbModel
            {
                Day = tmpDate.Date,
                LeftCount = 0,
                RightCount = 0
            });

            // Add a new day
            tmpDate = tmpDate.AddDays(1);
        }

        if (hasChanges)
            await SaveChangesAsync();
    }

    /// <summary>
    /// Adds a mouse message
    /// </summary>
    /// <param name="message">The message</param>
    /// <returns>The awaitable task</returns>
    public async Task InsertMouseActionAsync(MouseActionType message)
    {
        var entry = await MouseClickCount.FirstOrDefaultAsync(f => f.Day.Date == DateTime.Now.Date);
        if (entry == null)
        {
            await MouseClickCount.AddAsync(new MouseClickCountDbModel
            {
                Day = DateTime.Now.Date,
                LeftCount = message == MouseActionType.LeftButtonDown ? 1 : 0,
                RightCount = message == MouseActionType.RightButtonUp ? 1 : 0
            });
        }
        else
        {
            if (message == MouseActionType.LeftButtonDown)
                entry.LeftCount++;
            else if (message == MouseActionType.RightButtonUp)
                entry.RightCount++;
        }

        await SaveChangesAsync();
    }
    #endregion

    #region Statistics
    /// <summary>
    /// Loads the count for the desired type and date
    /// </summary>
    /// <param name="type">The type</param>
    /// <param name="dateType">The date</param>
    /// <returns>The count</returns>
    public Task<int> LoadCountAsync(StatisticType type, DateType dateType)
    {
        var date = dateType == DateType.Today ? DateTime.Now.Date : DateTime.Now.AddDays(-1).Date;

        var count = type == StatisticType.Keyboard
            ? KeyboardClickCount.AsNoTracking().Where(w => w.Day.Date == date.Date).SumAsync(s => s.Count)
            : MouseClickCount.AsNoTracking().Where(w => w.Day.Date == date.Date)
                .SumAsync(s => s.LeftCount + s.RightCount);

        return count;
    }

    /// <summary>
    /// Loads the statistics
    /// </summary>
    /// <returns>The statistics</returns>
    public async Task<Statistics> LoadStatisticsAsync()
    {
        var result = new Statistics();

        if (await KeyboardClickCount.AnyAsync() && await KeyboardKeyCount.AnyAsync())
        {
            // Day range
            var minDate = await KeyboardClickCount.AsNoTracking().MinAsync(m => m.Day);
            var maxDate = await KeyboardClickCount.AsNoTracking().MaxAsync(m => m.Day);
            var dayRange = (maxDate - minDate).TotalDays + 1;

            var totalCount = await KeyboardClickCount.AsNoTracking().SumAsync(s => s.Count);

            var maxEntry =
                await KeyboardClickCount.AsNoTracking().OrderByDescending(o => o.Count).FirstOrDefaultAsync() ??
                new KeyboardClickCountDbModel();
            var average = await KeyboardClickCount.AsNoTracking().AverageAsync(a => a.Count);

            result.KeyboardStats.MaxCount = $"{maxEntry.Count:N0} - {maxEntry.Day:dd.MM.yyyy}";
            result.KeyboardStats.AverageCount = average.ToString("N0");
            result.KeyboardStats.TotalCount = $"{totalCount:N0} // {minDate:dd.MM.yyyy} - {maxDate:dd.MM.yyyy} ({dayRange:N0})";
        }

        if (await KeyboardKeyCount.AnyAsync())
        {
            var mostUsedKey = await KeyboardKeyCount.AsNoTracking().OrderByDescending(o => o.Count).FirstOrDefaultAsync() ??
                              new KeyboardKeyCountDbModel();
            var leastUsedKey = await KeyboardKeyCount.AsNoTracking().OrderBy(o => o.Count).FirstOrDefaultAsync() ??
                               new KeyboardKeyCountDbModel();

            result.KeyboardStats.MostUsedKey = $"{mostUsedKey.Key} - {mostUsedKey.Count:N0}";
            result.KeyboardStats.LeastUsedKey = $"{leastUsedKey.Key} - {leastUsedKey.Count:N0}";
        }

        if (await MouseClickCount.AnyAsync())
        {
            // Day range
            var minDate = await MouseClickCount.AsNoTracking().MinAsync(m => m.Day);
            var maxDate = await MouseClickCount.AsNoTracking().MaxAsync(m => m.Day);
            var dayRange = (maxDate - minDate).TotalDays + 1;

            var maxEntry = await MouseClickCount.AsNoTracking().OrderByDescending(o => o.LeftCount)
                .FirstOrDefaultAsync() ?? new MouseClickCountDbModel();

            var average = await (from entry in MouseClickCount.AsNoTracking()
                let avgTotal = entry.RightCount + entry.LeftCount
                select avgTotal).AverageAsync();

            var total = await MouseClickCount.AsNoTracking().SumAsync(s => s.LeftCount + s.RightCount);
            var totalRight = await MouseClickCount.AsNoTracking().SumAsync(s => s.RightCount);
            var totalLeft = await MouseClickCount.AsNoTracking().SumAsync(s => s.LeftCount);

            result.MouseStats.MaxCount = $"{maxEntry.LeftCount:N0} - {maxEntry.Day:dd.MM.yyyy}";
            result.MouseStats.AverageCount = average.ToString("N0");
            result.MouseStats.TotalCount = $"{total:N0} // {minDate:dd.MM.yyyy} - {maxDate:dd.MM.yyyy} ({dayRange:N0})";
            result.MouseStats.RightCount = totalRight.ToString("N0");
            result.MouseStats.LeftCount = totalLeft.ToString("N0");
        }

        return result;
    }
    #endregion

    /// <summary>
    /// Occurs when the context is configuring
    /// </summary>
    /// <param name="optionsBuilder">The options builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source = {Path.Combine(Core.GetBaseDirPath(), "InputCountDatabase.db")}");
    }

    /// <summary>
    /// Occurs when the models will be created
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KeyboardClickCountDbModel>().HasIndex(h => h.Day).IsUnique();
        modelBuilder.Entity<KeyboardKeyCountDbModel>().HasIndex(h => h.KeyCode).IsUnique();
        modelBuilder.Entity<MouseClickCountDbModel>().HasIndex(h => h.Day).IsUnique();
    }
}