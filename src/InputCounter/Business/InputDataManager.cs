using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InputCounter.Data;
using InputCounter.Model;
using Microsoft.EntityFrameworkCore;

namespace InputCounter.Business;

/// <summary>
/// Provides the functions for the interaction with the input data
/// </summary>
internal class InputDataManager
{
    /// <summary>
    /// Loads the click count data of the keyboard
    /// </summary>
    /// <returns>The click count data</returns>
    public async Task<List<KeyboardClickCountDbModel>> LoadClickCountKeyboardAsync()
    {
        await using var context = new AppDbContext();
        var average = await context.KeyboardClickCount.AsNoTracking().AverageAsync(a => a.Count);

        var data = await context.KeyboardClickCount.AsNoTracking().OrderByDescending(o => o.Day).ToListAsync();

        // Calculate the diff to the average
        foreach (var entry in data)
        {
            entry.DiffToAverage = CalculateAverage(average, entry.Count);
        }

        return data;
    }

    /// <summary>
    /// Loads the list with the used keys
    /// </summary>
    /// <returns>The list with the used keys</returns>
    public static Task<List<KeyboardKeyCountDbModel>> LoadKeyCountAsync()
    {
        using var context = new AppDbContext();
        return context.KeyboardKeyCount.AsNoTracking().OrderByDescending(o => o.KeyCode).ToListAsync();
    }

    /// <summary>
    /// Loads the click count data of the mouse
    /// </summary>
    /// <returns>The click count data</returns>
    public static async Task<List<MouseClickCountDbModel>> LoadClickCountMouseAsync()
    {
        await using var context = new AppDbContext();
        var data = await context.MouseClickCount.AsNoTracking().OrderByDescending(o => o.Day).ToListAsync();

        var average = data.Average(a => a.TotalCount);

        // Calculate the diff to the average
        foreach (var entry in data)
        {
            entry.DiffToAverage = CalculateAverage(average, entry.TotalCount);
        }

        return data;
    }

    /// <summary>
    /// Calculates the difference of the total to the average
    /// </summary>
    /// <param name="average">The average value</param>
    /// <param name="total">The total</param>
    /// <returns>The difference to the average</returns>
    private static double CalculateAverage(double average, int total)
    {
        if (total == 0)
            return 0;

        var valueA = total - average;
        var valueB = (total + average) / 2;
        return valueA / valueB * 100;
    }
}