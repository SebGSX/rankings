// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.Storage;

/// <summary>
///     Represents a read/write store of text data.
/// </summary>
public interface IStore : IReadOnlyStore
{
    /// <summary>
    ///     Appends all lines of text to the store.
    /// </summary>
    public void AppendAllLines(string[] lines);
    
    /// <summary>
    ///     Initializes the store, creating any backing resources if they do not already exist.
    /// </summary>
    public void Initialize();

    /// <summary>
    ///     Resets the store, clearing all data and returning it to an uninitialized state.
    /// </summary>
    public void Reset();
}