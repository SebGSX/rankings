// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rankings.Storage;

/// <summary>
///     Represents a read only store of text data backed by an operating system file.
/// </summary>
[DebuggerDisplay("{" + nameof(FileInfo) + "}", Name = nameof(FileInfo))]
public class FileReadOnlyStore : IReadOnlyStore
{
    /// <summary>
    ///     Represents the file information for the backing file.
    /// </summary>
    protected readonly FileInfo FileInfo;

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileReadOnlyStore" /> class.
    /// </summary>
    /// <param name="fullName">The full name of the file backing the store.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="fullName" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="fullName" /> is empty or consists only of white-space characters.
    /// </exception>
    public FileReadOnlyStore(string fullName)
    {
        ArgumentNullException.ThrowIfNull(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        FileInfo = new FileInfo(fullName);
    }

    /// <inheritdoc />
    /// <remarks>
    ///     Exceptions are swallowed and <c>false</c> is returned if any are thrown,
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "File IO is an OS concern.")]
    public bool IsEmpty
    {
        get
        {
            try
            {
                return FileInfo is { Exists: true, Length: 0 };
            }
            catch (Exception)
            {
                return true;
            }
        }
    }

    /// <inheritdoc />
    /// <remarks>
    ///     Exceptions are swallowed and <c>false</c> is returned if any are thrown,
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "File IO is an OS concern.")]
    public bool IsInitialized
    {
        get
        {
            try
            {
                return FileInfo.Exists;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <inheritdoc />
    /// <remarks>
    ///     Exceptions are not caught and bubble up to be handled by the caller, which facilitates testing.
    /// </remarks>
    /// <exception cref="FileNotFoundException">Thrown if the backing file does not exist.</exception>
    [ExcludeFromCodeCoverage(Justification = "File IO is an OS concern.")]
    public string[] ReadAllLines()
    {
        // Any exception thrown bubbles up to be handled by the caller.
        return !FileInfo.Exists
            ? throw new FileNotFoundException(null, FileInfo.FullName)
            : File.ReadAllLines(FileInfo.FullName);
    }
}