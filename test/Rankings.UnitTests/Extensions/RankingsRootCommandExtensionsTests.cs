// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Moq;
using Rankings.Extensions;
using Rankings.Services;
using Rankings.Storage;

namespace Rankings.UnitTests.Extensions;

/// <summary>
///     Unit tests for the <see cref="RankingsRootCommandExtensions" /> class.
/// </summary>
public class RankingsRootCommandExtensionsTests
{
    /// <summary>
    ///     Tests that <see cref="RankingsRootCommandExtensions.AddAppendFileSubcommand" /> correctly adds
    ///     the append-file subcommand to a root command.
    /// </summary>
    [Fact]
    public void AddAppendFileSubcommand_AddsSubcommandToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedSubcommandName = "append-file";
        var serviceProviderMockObject = Mock.Of<IServiceProvider>();

        // Act
        rootCommand.AddAppendFileSubcommand(serviceProviderMockObject);
        var subcommand = rootCommand
            .Subcommands.FirstOrDefault(o => o.Name == expectedSubcommandName);

        // Assert
        Assert.NotNull(subcommand);
        Assert.Equal(expectedSubcommandName, subcommand.Name);
        Assert.NotNull(subcommand.Description);
        Assert.NotEmpty(subcommand.Description);
    }

    /// <summary>
    ///     Tests that the handler defined in <see cref="RankingsRootCommandExtensions.AddAppendFileSubcommand" />
    ///     handles invocation correctly.
    /// </summary>
    [Fact]
    public void AddAppendFileSubcommand_Handler_HandlesInvocation()
    {
        // Arrange
        const string fileName = "test.txt";
        var rootCommand = new RootCommand();

        var resultsProcessorMock = new Mock<IContestResultsProcessor>();
        var fileReadOnlyStoreMock = new Mock<IReadOnlyStore>();
        var storageFactoryMock = new Mock<IStorageFactory>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        rootCommand.AddAppendFileSubcommand(serviceProviderMock.Object);

        serviceProviderMock.Setup(m => m.GetService(typeof(IContestResultsProcessor)))
            .Returns(resultsProcessorMock.Object);
        serviceProviderMock.Setup(m => m.GetService(typeof(IStorageFactory)))
            .Returns(storageFactoryMock.Object);
        storageFactoryMock.Setup(m => m.CreateFileReadOnlyStore(It.IsAny<string>()))
            .Returns(fileReadOnlyStoreMock.Object);
        fileReadOnlyStoreMock.Setup(m => m.IsInitialized).Returns(true);

        // Act
        var parseResult = rootCommand.Parse($"append-file --file \"{fileName}\"");
        parseResult.Invoke();

        // Assert
        serviceProviderMock.Verify(m => m.GetService(typeof(IContestResultsProcessor)), Times.Once);
        serviceProviderMock.Verify(m => m.GetService(typeof(IStorageFactory)), Times.Once);
        storageFactoryMock.Verify(m => m.CreateFileReadOnlyStore(fileName), Times.Once);
        fileReadOnlyStoreMock.Verify(m => m.IsInitialized, Times.Once);
        fileReadOnlyStoreMock.Verify(m => m.ReadAllLines(), Times.Once);
        resultsProcessorMock
            .Verify(m => m.Process(It.IsAny<string[]>()), Times.Once);
    }

    /// <summary>
    ///     Tests that <see cref="RankingsRootCommandExtensions.AddAppendResultSubcommand" /> correctly adds
    ///     the result option to a root command.
    /// </summary>
    [Fact]
    public void AddAppendResultSubcommand_AddsSubcommandToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedSubcommandName = "append-result";
        var serviceProviderMockObject = Mock.Of<IServiceProvider>();

        // Act
        rootCommand.AddAppendResultSubcommand(serviceProviderMockObject);
        var subcommand = rootCommand
            .Subcommands.FirstOrDefault(o => o.Name == expectedSubcommandName);

        // Assert
        Assert.NotNull(subcommand);
        Assert.Equal(expectedSubcommandName, subcommand.Name);
        Assert.NotNull(subcommand.Description);
        Assert.NotEmpty(subcommand.Description);
    }

    /// <summary>
    ///     Tests that the handler defined in <see cref="RankingsRootCommandExtensions.AddAppendResultSubcommand" />
    ///     handles invocation correctly.
    /// </summary>
    [Fact]
    public void AddAppendResultSubcommand_Handler_HandlesInvocation()
    {
        // Arrange
        const string contestantResult = "Alice 10, Bob 20";
        var rootCommand = new RootCommand();

        var resultsProcessorMock = new Mock<IContestResultsProcessor>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        rootCommand.AddAppendResultSubcommand(serviceProviderMock.Object);

        serviceProviderMock.Setup(m => m.GetService(typeof(IContestResultsProcessor)))
            .Returns(resultsProcessorMock.Object);

        // Act
        var parseResult = rootCommand.Parse($"append-result --result \"{contestantResult}\"");
        parseResult.Invoke();

        // Assert
        serviceProviderMock.Verify(m => m.GetService(typeof(IContestResultsProcessor)), Times.Once);
        resultsProcessorMock
            .Verify(m => m.Process(It.IsAny<string[]>()), Times.Once);
    }

    /// <summary>
    ///     Tests that <see cref="RankingsRootCommandExtensions.AddClearContestResultsSubcommand" /> correctly adds
    ///     the result option to a root command.
    /// </summary>
    [Fact]
    public void AddClearContestResultsSubcommand_AddsSubcommandToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedSubcommandName = "clear-contest-results";
        var serviceProviderMockObject = Mock.Of<IServiceProvider>();

        // Act
        rootCommand.AddClearContestResultsSubcommand(serviceProviderMockObject);
        var subcommand = rootCommand
            .Subcommands.FirstOrDefault(o => o.Name == expectedSubcommandName);

        // Assert
        Assert.NotNull(subcommand);
        Assert.Equal(expectedSubcommandName, subcommand.Name);
        Assert.NotNull(subcommand.Description);
        Assert.NotEmpty(subcommand.Description);
    }

    /// <summary>
    ///     Tests that the handler defined in <see cref="RankingsRootCommandExtensions.AddClearContestResultsSubcommand" />
    ///     handles invocation correctly.
    /// </summary>
    [Fact]
    public void AddClearContestResultsSubcommand_Handler_HandlesInvocation()
    {
        // Arrange
        var rootCommand = new RootCommand();

        var resultsProcessorMock = new Mock<IContestResultsProcessor>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        rootCommand.AddClearContestResultsSubcommand(serviceProviderMock.Object);

        serviceProviderMock.Setup(m => m.GetService(typeof(IContestResultsProcessor)))
            .Returns(resultsProcessorMock.Object);

        // Act
        var parseResult = rootCommand.Parse("clear-contest-results");
        parseResult.Invoke();

        // Assert
        serviceProviderMock.Verify(m => m.GetService(typeof(IContestResultsProcessor)), Times.Once);
        resultsProcessorMock
            .Verify(m => m.ClearContestResults(), Times.Once);
    }

    /// <summary>
    ///     Tests that the handler defined in <see cref="RankingsRootCommandExtensions.SetRootCommandAction" />
    ///     handles invocation correctly.
    /// </summary>
    [Fact]
    public void SetRootCommandAction_Handler_HandlesInvocation()
    {
        // Arrange
        var rootCommand = new RootCommand();

        var resultsProcessorMock = new Mock<IContestResultsProcessor>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        rootCommand.SetRootCommandAction(serviceProviderMock.Object);

        serviceProviderMock.Setup(m => m.GetService(typeof(IContestResultsProcessor)))
            .Returns(resultsProcessorMock.Object);

        // Act
        var parseResult = rootCommand.Parse(string.Empty);
        parseResult.Invoke();

        // Assert
        serviceProviderMock.Verify(m => m.GetService(typeof(IContestResultsProcessor)), Times.Once);
        resultsProcessorMock
            .Verify(m => m.DisplayRankingTable(), Times.Once);
    }
}