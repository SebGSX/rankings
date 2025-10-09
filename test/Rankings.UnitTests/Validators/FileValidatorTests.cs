// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Moq;
using Rankings.Extensions;
using Rankings.Validators;

namespace Rankings.UnitTests.Validators;

/// <summary>
///     Unit tests for the <see cref="FileValidator" /> class.
/// </summary>
public class FileValidatorTests
{
    /// <summary>
    ///     Tests that <see cref="FileValidator.Validate" /> adds an error to the parse result when the input is
    ///     invalid.
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <param name="expected">The expected error message.</param>
    [Theory]
    [InlineData("append-file --file", "Required argument missing for option: '--file'.")]
    [InlineData("append-file --file \"invalid-filename-*.txt\"",
        "The file name contains invalid characters and is invalid.")]
    [InlineData($"append-file --file \"/\n/filename.txt\"",
        "The directory (folder) name contains invalid characters and is invalid.")]
    [InlineData("append-file --file \"\"", "The file name is missing or empty and is invalid.")]
    [InlineData("append-file --file \"./\"", "The file name is missing or empty and is invalid.")]
    public void Validate_WithError_AddsError(string input, string expected)
    {
        // Arrange
        var rootCommand = new RootCommand();
        var serviceProviderMockObject = Mock.Of<IServiceProvider>();
        rootCommand.AddAppendFileSubcommand(serviceProviderMockObject);

        // Act
        var parseResult = rootCommand.Parse(input);
        
        // Assert
        Assert.Single(parseResult.Errors);
        Assert.Equal(expected, parseResult.Errors[0].Message);
    }
    
    /// <summary>
    ///     Tests that <see cref="FileValidator.Validate" /> does not add an error to the parse result when the input
    ///     is valid.
    /// </summary>
    [Fact]
    public void Validate_WithoutError_DoesNotAddError()
    {
        // Arrange
        var rootCommand = new RootCommand();
        var serviceProviderMockObject = Mock.Of<IServiceProvider>();
        rootCommand.AddAppendFileSubcommand(serviceProviderMockObject);
        const string input = "append-file --file \"valid-filename.txt\"";

        // Act
        var parseResult = rootCommand.Parse(input);
        
        // Assert
        Assert.Empty(parseResult.Errors);
    }
}