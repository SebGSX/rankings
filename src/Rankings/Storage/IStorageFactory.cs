// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.Storage;

/// <summary>
///     Represents a factory for creating stores.
/// </summary>
public interface IStorageFactory
{
    /// <summary>
    ///     Creates a read only store backed by the file with the specified full name.
    /// </summary>
    /// <param name="fullName">The full name of the file backing the store.</param>
    /// <returns>A read only store backed by the specified file.</returns>
    public IReadOnlyStore CreateFileReadOnlyStore(string fullName);
    
    /// <summary>
    ///     Creates a read/write store backed by the file with the specified full name.
    /// </summary>
    /// <param name="fullName">The full name of the file backing the store.</param>
    /// <returns>A read/write store backed by the specified file.</returns>
    public IStore CreateFileStore(string fullName);
}