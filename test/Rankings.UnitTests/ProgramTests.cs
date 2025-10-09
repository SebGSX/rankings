// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.UnitTests;

/// <summary>
///     Unit tests for the <see cref="Program"/> class.
/// </summary>
public class ProgramTests
{
    /// <summary>
    ///     Tests that <see cref="Program.Main"/> configures the root command with the expected options.
    /// </summary>
    [Fact]
    public void Main_ConfiguresRootCommandWithExpectedOptions()
    {
        // Arrange
        var expectedOptions = new[]
        {
            "--help",
            "--version"
        };

        // Act
        var optionsQuery = Program.ConfiguredOptions;
        var actualOptions = optionsQuery.ToArray();

        // Assert
        Assert.Equal(expectedOptions, actualOptions);
    }
    
    /// <summary>
    ///     Tests that <see cref="Program.Main"/> configures the root command with the expected subcommands.
    /// </summary>
    [Fact]
    public void Main_ConfiguresRootCommandWithExpectedSubcommands()
    {
        // Arrange
        var expectedSubcommands = new[]
        {
            "append-file",
            "append-result",
            "clear-contest-results"
        };

        // Act
        var subcommandsQuery = Program.ConfiguredSubcommands;
        var actualSubcommands = subcommandsQuery.ToArray();

        // Assert
        Assert.Equal(expectedSubcommands, actualSubcommands);
    }
    
    /// <summary>
    ///     Tests that <see cref="Program.Main"/> displays help when called with the "--help" argument.
    /// </summary>
    /// <param name="arg">The help argument to test.</param>
    [Theory]
    [InlineData("-?")]
    [InlineData("-h")]
    [InlineData("--help")]
    public void Main_WithHelpArgument_DisplaysHelp(string arg)
    {
        // Arrange
        const string expected = "Show help and usage information";
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        // Act
        Program.Main([arg]);
        var actual = sw.ToString();

        // Assert
        Assert.Contains(expected, actual);
        
        // Cleanup
        Console.SetOut(originalOut);
    }
    
    /// <summary>
    ///     Tests that <see cref="Program.Main"/> displays an error message when called with an invalid
    ///     argument.
    /// </summary>
    [Fact]
    public void Main_WithInvalidArgument_DisplaysError()
    {
        // Arrange
        const string expected = "Unrecognized command or argument '--invalid'.";
        const string invalidArg = "--invalid";
        using var sw = new StringWriter();
        var originalError = Console.Error;
        Console.SetError(sw);

        // Act
        Program.Main([invalidArg]);
        var actual = sw.ToString();

        // Assert
        Assert.Contains(expected, actual);
        
        // Cleanup
        Console.SetError(originalError);
    }
    
    /// <summary>
    ///     Tests that <see cref="Program.Main"/> succeeds when called with no arguments.
    /// </summary>
    [Fact]
    public void Main_WithNoArguments_Succeeds()
    {
        // Arrange
        var args = Array.Empty<string>();
        using var sw = new StringWriter();
        var originalError = Console.Error;
        Console.SetError(sw);

        // Act
        Program.Main(args);
        var actual = sw.ToString();
        
        // Assert
        Assert.Empty(actual);
        
        // Cleanup
        Console.SetError(originalError);
    }
}