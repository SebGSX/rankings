// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Parsers;

namespace Rankings.UnitTests.Parsers;

/// <summary>
///     Unit tests for the <see cref="ContestResult" /> class.
/// </summary>
public class ContestResultTests
{
    /// <summary>
    ///     Tests that <see cref="ContestResult.ToString" /> returns the expected string representation.
    /// </summary>
    [Fact]
    public void ToString_ReturnsExpectedString()
    {
        // Arrange
        const string expected = $"Alice 10{ContestResultParser.ContestantResultSeparator} Bob 8";
        var contestResult = new ContestResult
        {
            Contestant1Name = "Alice",
            Contestant1Score = 10,
            Contestant2Name = "Bob",
            Contestant2Score = 8
        };

        // Act
        var actual = contestResult.ToString();

        // Assert
        Assert.Equal(expected, actual);
    }
}