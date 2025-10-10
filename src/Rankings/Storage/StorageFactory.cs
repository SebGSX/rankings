// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.Storage;

/// <inheritdoc />
public class StorageFactory : IStorageFactory
{
    /// <inheritdoc />
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="fullName" /> is <see langword="null" />.
    /// </exception>
    public IReadOnlyStore CreateFileReadOnlyStore(string fullName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        return new FileReadOnlyStore(fullName);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="fullName" /> is <see langword="null" />.
    /// </exception>
    public IStore CreateFileStore(string fullName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        return new FileStore(fullName);
    }
}