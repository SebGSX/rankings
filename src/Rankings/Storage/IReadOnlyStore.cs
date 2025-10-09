// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.Storage;

/// <summary>
///     Represents a read only store of text data.
/// </summary>
public interface IReadOnlyStore
{
    /// <summary>
    ///     Indicates whether the store is empty.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the store is empty; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEmpty { get; }

    /// <summary>
    ///     Indicates whether the store has been initialized.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the store has been initialized; otherwise, <c>false</c>.
    /// </returns>
    public bool IsInitialized { get; }

    /// <summary>
    ///     Reads all lines of text from the store.
    /// </summary>
    /// <returns>An array containing all lines of text from the store.</returns>
    public string[] ReadAllLines();
}