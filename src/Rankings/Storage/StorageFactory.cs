// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.Storage;

/// <inheritdoc />
public class StorageFactory : IStorageFactory
{
    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="fullName" /> is <see langword="null" />.
    /// </exception>
    public IReadOnlyStore CreateFileReadOnlyStore(string fullName)
    {
        ArgumentNullException.ThrowIfNull(fullName);

        return new FileReadOnlyStore(fullName);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="fullName" /> is <see langword="null" />.
    /// </exception>
    public IStore CreateFileStore(string fullName)
    {
        ArgumentNullException.ThrowIfNull(fullName);

        return new FileStore(fullName);
    }
}