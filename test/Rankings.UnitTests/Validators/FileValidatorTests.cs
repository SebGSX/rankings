// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Rankings.Extensions;
using Rankings.Validators;

namespace Rankings.UnitTests.Validators;

/// <summary>
///     Unit tests for the <see cref="FileValidator" /> class.
/// </summary>
public class FileValidatorTests
{
    /// <summary>
    ///     Tests that the validator adds an error when the input is invalid.
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <param name="expected">The expected error message.</param>
    [Theory]
    [InlineData("append-file --file", "Required argument missing for option: '--file'.")]
    [InlineData("append-file --file \"none-existent.txt\"", "The file specified does not exist.")]
    public void Validate_WithError_AddsError(string input, string expected)
    {
        // Arrange
        var rootCommand = new RootCommand();
        rootCommand.AddAppendFileSubcommand();

        // Act
        var parseResult = rootCommand.Parse(input);
        
        // Assert
        Assert.Single(parseResult.Errors);
        Assert.Equal(expected, parseResult.Errors[0].Message);
    }
}