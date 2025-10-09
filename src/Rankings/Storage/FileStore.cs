// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Rankings.Storage;

/// <summary>
///     Represents a read/write store of text data backed by an operating system file.
/// </summary>
public class FileStore : FileReadOnlyStore, IStore
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FileStore" /> class.
    /// </summary>
    /// <param name="fullName">The full name of the file backing the store.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="fullName" /> is <see langword="null" />.
    /// </exception>
    public FileStore(string fullName) : base(fullName)
    {
        // All initialization is handled by the base class.
    }

    /// <inheritdoc />
    /// <remarks>
    ///     Exceptions are not caught and bubble up to be handled by the caller, which facilitates testing.
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "File IO is an OS concern.")]
    public void AppendAllLines(string[] lines)
    {
        // Any exception thrown bubbles up to be handled by the caller.
        File.AppendAllLines(FileInfo.FullName, lines);
    }

    /// <inheritdoc />
    /// <remarks>
    ///     Exceptions are not caught and bubble up to be handled by the caller, which facilitates testing.
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "File IO is an OS concern.")]
    public void Initialize()
    {
        // Any exception thrown bubbles up to be handled by the caller.
        if (FileInfo.Exists) return;

        using (FileInfo.Create())
        {
            // Just create, then dispose of the file handle.
        }
    }
}