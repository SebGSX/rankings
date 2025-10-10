// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Storage;

namespace Rankings.UnitTests.Storage;

/// <summary>
///     Unit tests for the <see cref="StorageFactory" /> class.
/// </summary>
public class StorageFactoryTests
{
    /// <summary>
    ///     Tests that <see cref="StorageFactory.CreateFileReadOnlyStore(string)" /> throws an
    ///     <see cref="ArgumentException" /> when the input is <c>null</c>.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("\n")]
    [InlineData("\r")]
    [InlineData("\t")]
    [InlineData("\r\n")]
    [InlineData("\r\n\t")]
    public void CreateFileReadOnlyStore_WithNullFullName_ThrowsArgumentException(string input)
    {
        // Arrange
        var storageFactory = new StorageFactory();

        // Act
        var exception = Record.Exception(() => storageFactory.CreateFileReadOnlyStore(input));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("fullName", ((ArgumentException)exception).ParamName);
    }
    
    /// <summary>
    ///     Tests that <see cref="StorageFactory.CreateFileReadOnlyStore(string)" /> throws an
    ///     <see cref="ArgumentNullException" /> when the input is <c>null</c>.
    /// </summary>
    [Fact]
    public void CreateFileReadOnlyStore_WithNullFullName_ThrowsArgumentNullException()
    {
        // Arrange
        var storageFactory = new StorageFactory();
        string fullName = null!;

        // Act
        var exception = Record.Exception(() => storageFactory.CreateFileReadOnlyStore(fullName));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("fullName", ((ArgumentNullException)exception).ParamName);
    }

    /// <summary>
    ///     Tests that <see cref="StorageFactory.CreateFileReadOnlyStore(string)" /> returns a
    ///     <see cref="FileReadOnlyStore" /> when provided with a valid full name.
    /// </summary>
    [Fact]
    public void CreateFileReadOnlyStore_WithValidFullName_ReturnsFileReadOnlyStore()
    {
        // Arrange
        var storageFactory = new StorageFactory();
        const string fullName = "test-file.txt";

        // Act
        var store = storageFactory.CreateFileReadOnlyStore(fullName);

        // Assert
        Assert.NotNull(store);
        Assert.IsType<FileReadOnlyStore>(store);
    }

    /// <summary>
    ///     Tests that <see cref="StorageFactory.CreateFileStore(string)" /> throws an
    ///     <see cref="ArgumentException" /> when the input is <c>null</c>.
    /// </summary>
    /// <param name="input">The input to test.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("\n")]
    [InlineData("\r")]
    [InlineData("\t")]
    [InlineData("\r\n")]
    [InlineData("\r\n\t")]
    public void CreateFileStore_WithNullFullName_ThrowsArgumentException(string input)
    {
        // Arrange
        var storageFactory = new StorageFactory();

        // Act
        var exception = Record.Exception(() => storageFactory.CreateFileStore(input));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("fullName", ((ArgumentException)exception).ParamName);
    }
    
    /// <summary>
    ///     Tests that <see cref="StorageFactory.CreateFileStore(string)" /> throws an
    ///     <see cref="ArgumentNullException" /> when the input is <c>null</c>.
    /// </summary>
    [Fact]
    public void CreateFileStore_WithNullFullName_ThrowsArgumentNullException()
    {
        // Arrange
        var storageFactory = new StorageFactory();
        string fullName = null!;

        // Act
        var exception = Record.Exception(() => storageFactory.CreateFileStore(fullName));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("fullName", ((ArgumentNullException)exception).ParamName);
    }

    /// <summary>
    ///     Tests that <see cref="StorageFactory.CreateFileStore(string)" /> returns a
    ///     <see cref="FileStore" /> when provided with a valid full name.
    /// </summary>
    [Fact]
    public void CreateFileStore_WithValidFullName_ReturnsFileStore()
    {
        // Arrange
        var storageFactory = new StorageFactory();
        const string fullName = "test-file.txt";

        // Act
        var store = storageFactory.CreateFileStore(fullName);

        // Assert
        Assert.NotNull(store);
        Assert.IsType<FileStore>(store);
    }
}