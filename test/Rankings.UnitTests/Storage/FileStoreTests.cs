// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Storage;

namespace Rankings.UnitTests.Storage;

/// <summary>
///     Unit tests for the <see cref="FileStore" /> class.
/// </summary>
public class FileStoreTests
{
    /// <summary>
    ///     Tests that the constructor throws an <see cref="ArgumentNullException" /> when the input is <c>null</c>.
    /// </summary>
    [Fact]
    public void Ctor_WithNullFileInfo_ThrowsArgumentNullException()
    {
        // Arrange
        string fullName = null!;

        // Act
        var exception = Record.Exception(() => new FileStore(fullName));
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("fullName", ((ArgumentNullException)exception).ParamName);
    }
    
    /// <summary>
    ///     Tests that the constructor throws an <see cref="ArgumentException" /> when the input is empty.
    /// </summary>
    [Fact]
    public void Ctor_WithEmptyFileInfo_ThrowsArgumentException()
    {
        // Arrange
        const string fullName = "";
        
        // Act
        var exception = Record.Exception(() => new FileStore(fullName));
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("fullName", ((ArgumentException)exception).ParamName);
    }
    
    /// <summary>
    ///     Tests that the constructor instantiates the object when the input is a valid file name.
    /// </summary>
    [Fact]
    public void Ctor_WithValidFullName_Instantiates()
    {
        // Arrange
        const string fullName = "validFileName.txt";
        
        // Act
        var actual = new FileStore(fullName);
        
        // Assert
        Assert.NotNull(actual);
    }
}