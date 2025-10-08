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
    ///     Tests that the <see cref="RankingsRootCommandExtensions.AddAppendFileSubCommand" /> method correctly adds
    ///     the append-result subcommand to a root command.
    /// </summary>
    [Fact]
    public void AddAppendFileSubCommand_AddsOptionToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedSubCommandName = "append-file";
        
        // Act
        rootCommand.AddAppendFileSubCommand();
        var subcommand = rootCommand
            .Subcommands.FirstOrDefault(o => o.Name == expectedSubCommandName);
        
        // Assert
        Assert.NotNull(subcommand);
        Assert.Equal(expectedSubCommandName, subcommand.Name);
        Assert.NotNull(subcommand.Description);
        Assert.NotEmpty(subcommand.Description);
    }
    
    /// <summary>
    ///     Tests that the <see cref="RankingsRootCommandExtensions.AddAppendResultSubCommand" /> method correctly adds the
    ///     result option to a root command.
    /// </summary>
    [Fact]
    public void AddAppendResultSubCommand_AddsOptionToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedSubCommandName = "append-result";
        
        // Act
        rootCommand.AddAppendResultSubCommand();
        var subcommand = rootCommand
            .Subcommands.FirstOrDefault(o => o.Name == expectedSubCommandName);
        
        // Assert
        Assert.NotNull(subcommand);
        Assert.Equal(expectedSubCommandName, subcommand.Name);
        Assert.NotNull(subcommand.Description);
        Assert.NotEmpty(subcommand.Description);
    }
}