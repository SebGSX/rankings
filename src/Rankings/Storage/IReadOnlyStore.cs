// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.Storage;

/// <summary>
///     Represents a read only store of text data.
/// </summary>
public interface IReadOnlyStore
{
    /// <summary>
    ///     Reads all lines of text from the store.
    /// </summary>
    /// <returns>An array containing all lines of text from the store.</returns>
    public string[] ReadAllLines();
}