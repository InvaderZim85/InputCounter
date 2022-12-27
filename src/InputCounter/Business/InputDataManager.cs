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
    public Task<List<KeyboardClickCountDbModel>> LoadClickCountKeyboardAsync()
    {
        using var context = new AppDbContext();
        return context.KeyboardClickCount.AsNoTracking().OrderByDescending(o => o.Day).ToListAsync();
    }

    /// <summary>
    /// Loads the list with the used keys
    /// </summary>
    /// <returns>The list with the used keys</returns>
    public Task<List<KeyboardKeyCountDbModel>> LoadKeyCountAsync()
    {
        using var context = new AppDbContext();
        return context.KeyboardKeyCount.AsNoTracking().OrderByDescending(o => o.KeyCode).ToListAsync();
    }

    /// <summary>
    /// Loads the click count data of the mouse
    /// </summary>
    /// <returns>The click count data</returns>
    public Task<List<MouseClickCountDbModel>> LoadClickCountMouseAsync()
    {
        using var context = new AppDbContext();
        return context.MouseClickCount.AsNoTracking().OrderByDescending(o => o.Day).ToListAsync();
    }
}