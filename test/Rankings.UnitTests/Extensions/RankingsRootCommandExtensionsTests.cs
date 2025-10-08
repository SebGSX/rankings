// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Extensions;
using System.CommandLine;
using System.Diagnostics;

namespace Rankings.UnitTests.Extensions;

/// <summary>
///     Unit tests for the <see cref="RankingsRootCommandExtensions" /> class.
/// </summary>
public class RankingsRootCommandExtensionsTests
{
    /// <summary>
    ///     Tests that the <see cref="RankingsRootCommandExtensions.AddResultOption" /> method correctly adds the
    ///     result option to a root command.
    /// </summary>
    [Fact]
    public void AddResultOption_AddsOptionToRootCommand()
    {
        // Arrange
        var rootCommand = new RootCommand();
        const string expectedOptionName = "--result";
        string[] expectedOptionAliases = ["-r"];
        
        // Act
        rootCommand.AddResultOption();
        var option = rootCommand.Options.FirstOrDefault(o => o.Name == expectedOptionName);
        
        // Assert
        Assert.NotNull(option);
        Assert.Equal(expectedOptionName, option.Name);
        Assert.Equal(expectedOptionAliases, option.Aliases);
        Assert.NotNull(option.Description);
        Assert.NotEmpty(option.Description);
    }
}