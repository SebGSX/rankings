// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Extensions;
using System.CommandLine;
using Moq;

namespace Rankings.UnitTests.Extensions;

/// <summary>
///     Unit tests for the <see cref="RankingsRootCommandExtensions" /> class.
/// </summary>
public class RankingsRootCommandExtensionsTests
{
    /// <summary>
    ///     Tests that the <see cref="RankingsRootCommandExtensions.AddAppendFileSubcommand" /> method correctly adds
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
    ///     Tests that the <see cref="RankingsRootCommandExtensions.AddAppendResultSubcommand" /> method correctly adds
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
}