// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Microsoft.Extensions.Options;
using Moq;
using Rankings.Parsers;
using Rankings.Services;
using Rankings.Storage;

namespace Rankings.UnitTests.Services;

/// <summary>
///     Unit tests for the <see cref="ContestResultsProcessor"/> class.
/// </summary>
public class ContestResultsProcessorTests
{
    /// <summary>
    ///     Tests that the constructor instantiates the object when provided with valid parameters.
    /// </summary>
    [Fact]
    public void Ctor_WithValidParameters_Instantiates()
    {
        // Arrange
        var options = Options.Create(new ContestResultsProcessorOptions { FilePath = "test.json" });
        var storageFactoryMock = new Mock<IStorageFactory>();

        // Act
        var actual = new ContestResultsProcessor(options, storageFactoryMock.Object);
        
        // Assert
        Assert.NotNull(actual);
    }

    [Fact]
    public void ClearContestResults_ResetsStore()
    {
        // Arrange
        var options = Options.Create(new ContestResultsProcessorOptions { FilePath = "test.json" });
        var storageFactoryMock = new Mock<IStorageFactory>();
        var fileStoreMock = new Mock<IStore>();
        var processor = new ContestResultsProcessor(options, storageFactoryMock.Object);
        
        storageFactoryMock
            .Setup(m => m.CreateFileStore(It.IsAny<string>()))
            .Returns(fileStoreMock.Object);
        fileStoreMock.Setup(m => m.IsInitialized).Returns(true);
        
        // Act
        processor.ClearContestResults();
        
        // Assert
        storageFactoryMock.Verify(m => m.CreateFileStore(options.Value.FilePath), Times.Once);
        fileStoreMock.Verify(m => m.IsInitialized, Times.Once);
        fileStoreMock.Verify(m => m.Reset(), Times.Once);
    }
    
    /// <summary>
    ///     Tests that <see cref="ContestResultsProcessor.Process"/> throws an <see cref="InvalidOperationException"/>
    ///     when provided with invalid contest results, and that the exception message matches the expected message
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <param name="expected">The expected error message.</param>
    [Theory]
    [InlineData(
        $"1{ContestResultParser.ContestantResultSeparator}2{ContestResultParser.ContestantResultSeparator}3",
        $"A result can only contain one {ContestResultParser.ContestantResultSeparator} symbol.")]
    [InlineData(
        "12345",
        $"A result must be separated into two parts by the {ContestResultParser.ContestantResultSeparator} symbol; one part for each contestant's name and score.")]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator} Bob 20",
        "A result must include the results for both contestants. Cannot find a result for contestant 1.")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator}",
        "A result must include the results for both contestants. Cannot find a result for contestant 2.")]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator} Bob 20",
        "A result must include names for both contestants. Cannot find a name for contestant 1.")]
    [InlineData(
        $"Alice{ContestResultParser.ContestantResultSeparator} Bob 20",
        "A result must include scores for both contestants. Cannot find a score for contestant 1.")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator}20",
        "A result must include names for both contestants. Cannot find a name for contestant 2.")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator} Bob",
        "A result must include scores for both contestants. Cannot find a score for contestant 2.")]
    public void Process_WithInvalidContestResults_ThrowsInvalidOperationException(string input, string expected)
    {
        // Arrange
        var options = Options.Create(new ContestResultsProcessorOptions { FilePath = "test.json" });
        var storageFactoryMock = new Mock<IStorageFactory>();
        var processor = new ContestResultsProcessor(options, storageFactoryMock.Object);
        using var sw = new StringWriter();
        var originalError = Console.Error;
        Console.SetError(sw);
        
        // Act
        var exception = Record.Exception(() => processor.Process([input]));
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Contains("Error in contestant result 1: ", sw.ToString());
        Assert.Equal(expected, exception.Message);
        storageFactoryMock.Verify(m => m.CreateFileStore(It.IsAny<string>()), Times.Never);
        
        // Cleanup
        Console.SetError(originalError);
    }

    /// <summary>
    ///     Tests that <see cref="ContestResultsProcessor.Process"/> stores results when provided with valid contest
    ///     results.
    /// </summary>
    [Fact]
    public void Process_WithValidContestResults_StoresResults()
    {
        // Arrange
        var options = Options.Create(new ContestResultsProcessorOptions { FilePath = "test.json" });
        var storageFactoryMock = new Mock<IStorageFactory>();
        var fileStoreMock = new Mock<IStore>();
        var processor = new ContestResultsProcessor(options, storageFactoryMock.Object);

        storageFactoryMock
            .Setup(m => m.CreateFileStore(It.IsAny<string>()))
            .Returns(fileStoreMock.Object);
        
        fileStoreMock.Setup(m => m.IsInitialized).Returns(false);
        
        var contestResults = new[]
        {
            $"Alice 10{ContestResultParser.ContestantResultSeparator} Bob 20",
            $"Charlie 15{ContestResultParser.ContestantResultSeparator} Dana 15"
        };
        
        // Act
        processor.Process(contestResults);
        
        // Assert
        storageFactoryMock.Verify(m => m.CreateFileStore(options.Value.FilePath), Times.Once);
        fileStoreMock.Verify(m => m.IsInitialized, Times.Once);
        fileStoreMock.Verify(m => m.Initialize(), Times.Once);
        fileStoreMock.Verify(m => m.AppendAllLines(It.IsAny<string[]>()), Times.Once);
    }
}