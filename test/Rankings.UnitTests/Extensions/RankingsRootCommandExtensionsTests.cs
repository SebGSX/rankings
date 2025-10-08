// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Extensions;
using System.CommandLine;

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
    public void AddAppendFileSubcommand_AddsOptionToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedSubcommandName = "append-file";
        
        // Act
        rootCommand.AddAppendFileSubcommand();
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
    public void AddAppendResultSubcommand_AddsOptionToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedSubcommandName = "append-result";
        
        // Act
        rootCommand.AddAppendResultSubcommand();
        var subcommand = rootCommand
            .Subcommands.FirstOrDefault(o => o.Name == expectedSubcommandName);
        
        // Assert
        Assert.NotNull(subcommand);
        Assert.Equal(expectedSubcommandName, subcommand.Name);
        Assert.NotNull(subcommand.Description);
        Assert.NotEmpty(subcommand.Description);
    }
}