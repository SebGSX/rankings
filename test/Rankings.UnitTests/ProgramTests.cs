// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.UnitTests;

/// <summary>
///     Unit tests for the <see cref="Program"/> class.
/// </summary>
public class ProgramTests
{
    /// <summary>
    ///     Tests that the <see cref="Program.Main"/> method configures the root command with the expected options.
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
    ///     Tests that the <see cref="Program.Main"/> method configures the root command with the expected subcommands.
    /// </summary>
    [Fact]
    public void Main_ConfiguresRootCommandWithExpectedSubCommands()
    {
        // Arrange
        var expectedSubCommands = new[]
        {
            "append-file",
            "append-result"
        };

        // Act
        var subcommandsQuery = Program.ConfiguredSubCommands;
        var actualSubCommands = subcommandsQuery.ToArray();

        // Assert
        Assert.Equal(expectedSubCommands, actualSubCommands);
    }
    
    /// <summary>
    ///     Tests that the <see cref="Program.Main"/> method displays help when called with the "--help" argument.
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
        Console.SetOut(sw);

        // Act
        Program.Main([arg]);
        var actual = sw.ToString();

        // Assert
        Assert.Contains(expected, actual);
    }
    
    /// <summary>
    ///     Tests that the <see cref="Program.Main"/> method displays an error message when called with an invalid
    ///     argument.
    /// </summary>
    [Fact]
    public void Main_WithInvalidArgument_DisplaysError()
    {
        // Arrange
        const string expected = "Unrecognized command or argument '--invalid'.";
        const string invalidArg = "--invalid";
        using var sw = new StringWriter();
        Console.SetError(sw);

        // Act
        Program.Main([invalidArg]);
        var actual = sw.ToString();

        // Assert
        Assert.Contains(expected, actual);
    }
    
    /// <summary>
    ///     Tests that the <see cref="Program.Main"/> method does not throw an exception when called with no arguments.
    /// </summary>
    [Fact]
    public void Main_WithNoArguments_DoesNotThrow()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act & Assert
        var exception = Record.Exception(() => Program.Main(args));
        Assert.Null(exception);
    }
}