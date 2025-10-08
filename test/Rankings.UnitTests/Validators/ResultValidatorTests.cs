// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Rankings.Extensions;
using Rankings.Parsers;
using Rankings.Validators;

namespace Rankings.UnitTests.Validators;

/// <summary>
///     Unit tests for the <see cref="ResultValidator" /> class.
/// </summary>
public class ResultValidatorTests
{
    /// <summary>
    ///     Tests that the validator adds an error when the input is invalid.
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <param name="expected">The expected error message.</param>
    [Theory]
    [InlineData("append-result --result", "Required argument missing for option: '--result'.")]
    [InlineData(
        "append-result --result \"1\n2\"",
        "A result must not contain any line breaks, it must be a single line.")]
    [InlineData(
        "append-result --result \"1\r2\"",
        "A result must not contain any line breaks, it must be a single line.")]
    [InlineData(
        "append-result --result \"1\r\n2\"",
        "A result must not contain any line breaks, it must be a single line.")]
    [InlineData(
        $"append-result --result \"1{ResultParser.ContestantResultSeparator}2{ResultParser.ContestantResultSeparator}3\"",
        $"A result can only contain one {ResultParser.ContestantResultSeparator} symbol.")]
    [InlineData(
        "append-result --result \"12345\"",
        $"A result must be separated into two parts by the {ResultParser.ContestantResultSeparator} symbol; one part for each contestant's name and score.")]
    [InlineData(
        $"append-result --result \"{ResultParser.ContestantResultSeparator} Bob 20\"",
        "A result must include the results for both contestants. Cannot find a result for contestant 1.")]
    [InlineData(
        $"append-result --result \"Alice 10{ResultParser.ContestantResultSeparator}\"",
        "A result must include the results for both contestants. Cannot find a result for contestant 2.")]
    [InlineData(
        $"append-result --result \"10{ResultParser.ContestantResultSeparator} Bob 20\"",
        "A result must include names for both contestants. Cannot find a name for contestant 1.")]
    [InlineData(
        $"append-result --result \"Alice{ResultParser.ContestantResultSeparator} Bob 20\"",
        "A result must include scores for both contestants. Cannot find a score for contestant 1.")]
    [InlineData(
        $"append-result --result \"Alice 10{ResultParser.ContestantResultSeparator}20\"",
        "A result must include names for both contestants. Cannot find a name for contestant 2.")]
    [InlineData(
        $"append-result --result \"Alice 10{ResultParser.ContestantResultSeparator} Bob\"",
        "A result must include scores for both contestants. Cannot find a score for contestant 2.")]
    public void Validate_WithError_AddsError(string input, string expected)
    {
        // Arrange
        var rootCommand = new RootCommand();
        rootCommand.AddAppendResultSubCommand();

        // Act
        var parseResult = rootCommand.Parse(input);
        
        // Assert
        Assert.Single(parseResult.Errors);
        Assert.Equal(expected, parseResult.Errors[0].Message);
    }
}