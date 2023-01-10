using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InputCounter.Common;

/// <summary>
/// Provides several extensions
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Converts the given list into an observable collection
    /// </summary>
    /// <typeparam name="T">The type of the data</typeparam>
    /// <param name="source">The source data</param>
    /// <returns>The collection</returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        return new ObservableCollection<T>(source);
    }
}