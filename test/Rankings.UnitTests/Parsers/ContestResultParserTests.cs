// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Parsers;

namespace Rankings.UnitTests.Parsers;

/// <summary>
///     Unit tests for the <see cref="ContestResultParser" /> class.
/// </summary>
public class ContestResultParserTests
{
    /// <summary>
    ///     Tests that the constructor throws an <see cref="ArgumentException" /> when the input is empty.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("\t")]
    [InlineData("\t ")]
    [InlineData(" \t")]
    [InlineData(" \t ")]
    public void Ctor_WithEmptyInput_ThrowsArgumentException(string input)
    {
        // Act
        var exception = Record.Exception(() => new ContestResultParser(input));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("input", ((ArgumentException)exception).ParamName);
        Assert.Equal("Value cannot be empty or white-space. (Parameter 'input')", exception.Message);
    }

    /// <summary>
    ///     Tests that the constructor sets all flags correctly when the input is invalid in multiple ways.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    /// <remarks>
    ///     This test checks combinations that are not covered by the other tests.
    /// </remarks>
    [Theory]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator}",
        "",
        0,
        "",
        0)]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator}",
        "",
        10,
        "",
        0)]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator}10",
        "",
        0,
        "",
        10)]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator}10",
        "",
        10,
        "",
        10)]
    [InlineData(
        $"Alice in Wonderland{ContestResultParser.ContestantResultSeparator}",
        "Alice in Wonderland",
        0,
        "",
        0)]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator}Bob the Builder",
        "",
        0,
        "Bob the Builder",
        0)]
    [InlineData(
        $"Alice in Wonderland{ContestResultParser.ContestantResultSeparator}Bob the Builder",
        "Alice in Wonderland",
        0,
        "Bob the Builder",
        0)]
    [InlineData(
        $"Alice in Wonderland{ContestResultParser.ContestantResultSeparator}10",
        "Alice in Wonderland",
        0,
        "",
        10)]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator}Bob the Builder",
        "",
        10,
        "Bob the Builder",
        0)]
    [InlineData(
        $"-1{ContestResultParser.ContestantResultSeparator}",
        "",
        0,
        "",
        0)]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator}-1",
        "",
        0,
        "",
        0)]
    [InlineData(
        $"Alice in Wonderland-1{ContestResultParser.ContestantResultSeparator}",
        "Alice in Wonderland-1",
        0,
        "",
        0)]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator}Bob the Builder-1",
        "",
        0,
        "Bob the Builder-1",
        0)]
    [InlineData(
        $"-1Alice in Wonderland{ContestResultParser.ContestantResultSeparator}",
        "-1Alice in Wonderland",
        0,
        "",
        0)]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator}-1Bob the Builder",
        "",
        0,
        "-1Bob the Builder",
        0)]
    public void Ctor_WithSingleContestantResultSeparatorAndInvalidInput_SetsFlagsAndPropertiesCorrectly(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Arrange
        var results = input.Split(ContestResultParser.ContestantResultSeparator);
        var hasNoContestant1ResultExpected = string.IsNullOrWhiteSpace(results[0]);
        var hasNoContestant2ResultExpected = string.IsNullOrWhiteSpace(results[1]);

        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.Equal(string.IsNullOrWhiteSpace(expectedContestant1Name), parser.HasNoContestant1Name);
        Assert.Equal(hasNoContestant1ResultExpected, parser.HasNoContestant1Result);
        Assert.Equal(expectedContestant1Score == 0, parser.HasNoContestant1Score);

        Assert.Equal(string.IsNullOrWhiteSpace(expectedContestant2Name), parser.HasNoContestant2Name);
        Assert.Equal(hasNoContestant2ResultExpected, parser.HasNoContestant2Result);
        Assert.Equal(expectedContestant2Score == 0, parser.HasNoContestant2Score);

        Assert.Equal(expectedContestant1Name, parser.Contestant1Name);
        Assert.Equal(expectedContestant1Score, parser.Contestant1Score);

        Assert.Equal(expectedContestant2Name, parser.Contestant2Name);
        Assert.Equal(expectedContestant2Score, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor throws an <see cref="ArgumentException" /> when the input contains a new line.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    [Theory]
    [InlineData("Alice 1 \r Bob 2")]
    [InlineData("Alice 1 \n Bob 2")]
    [InlineData("Alice 1 \r\n Bob 2")]
    [InlineData("Alice 1 \r\n\r\n Bob 2")]
    [InlineData("Alice 1 \n\n Bob 2")]
    [InlineData("Alice 1 \r\r Bob 2")]
    [InlineData("\r")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    [InlineData("\r\n\r\n")]
    [InlineData("\n\n")]
    [InlineData("\r\r")]
    public void Ctor_WithNewLineInInput_ThrowsArgumentException(string input)
    {
        // Act
        var exception = Record.Exception(() => new ContestResultParser(input));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("input", ((ArgumentException)exception).ParamName);
        Assert.Equal("Value cannot contain a new line. (Parameter 'input')", exception.Message);
    }

    /// <summary>
    ///     Tests that the constructor throws an <see cref="ArgumentNullException" /> when the input is <c>null</c>.
    /// </summary>
    [Fact]
    public void Ctor_WithNullInput_ThrowsArgumentNullException()
    {
        // Arrange
        string input = null!;

        // Act
        var exception = Record.Exception(() => new ContestResultParser(input));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("input", ((ArgumentNullException)exception).ParamName);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.IsMissingContestantResultSeparator" />
    ///     to <c>true</c> when the input is missing the contestant result separator, regardless of spacing.
    /// </summary>
    /// <param name="input"></param>
    [Theory]
    [InlineData("Alice 10 Bob 20")]
    [InlineData("Alice 10  Bob 20")]
    [InlineData("Alice 10\tBob 20")]
    public void Ctor_WithMissingContestantResultSeparator_SetsIsMissingContestantResultSeparatorToTrue(string input)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.True(parser.IsMissingContestantResultSeparator);

        Assert.True(parser.HasNoContestant1Name);
        Assert.True(parser.HasNoContestant1Result);
        Assert.True(parser.HasNoContestant1Score);

        Assert.True(parser.HasNoContestant2Name);
        Assert.True(parser.HasNoContestant2Result);
        Assert.True(parser.HasNoContestant2Score);

        Assert.Empty(parser.Contestant1Name);
        Assert.Equal(0, parser.Contestant1Score);

        Assert.Empty(parser.Contestant2Name);
        Assert.Equal(0, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.HasMultipleContestantResultSeparators" />
    ///     to <c>true</c> when the input contains multiple contestant result separators, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    [Theory]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator} Bob 20{ContestResultParser.ContestantResultSeparator} Charlie 30")]
    [InlineData(
        $"Alice 10 {ContestResultParser.ContestantResultSeparator} Bob 20 {ContestResultParser.ContestantResultSeparator} Charlie 30")]
    [InlineData(
        $"Alice 10 {ContestResultParser.ContestantResultSeparator}Bob 20 {ContestResultParser.ContestantResultSeparator}Charlie 30")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator} Bob 20 {ContestResultParser.ContestantResultSeparator}Charlie 30")]
    [InlineData(
        $"Alice 10 {ContestResultParser.ContestantResultSeparator} Bob 20{ContestResultParser.ContestantResultSeparator} Charlie 30")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator}Bob 20{ContestResultParser.ContestantResultSeparator}Charlie 30")]
    public void Ctor_WithMultipleContestantResultSeparators_SetsHasMultipleContestantResultSeparatorsToTrue(
        string input)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.True(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.True(parser.HasNoContestant1Name);
        Assert.True(parser.HasNoContestant1Result);
        Assert.True(parser.HasNoContestant1Score);

        Assert.True(parser.HasNoContestant2Name);
        Assert.True(parser.HasNoContestant2Result);
        Assert.True(parser.HasNoContestant2Score);

        Assert.Empty(parser.Contestant1Name);
        Assert.Equal(0, parser.Contestant1Score);

        Assert.Empty(parser.Contestant2Name);
        Assert.Equal(0, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.HasNoContestant1Name" />
    ///     to <c>true</c> when the input is missing the first contestant name, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator} Bob 20",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $" 10{ContestResultParser.ContestantResultSeparator} Bob 20",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator} Bob 20 ",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $" 10{ContestResultParser.ContestantResultSeparator} Bob 20 ",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator}\tBob 20",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $" 10{ContestResultParser.ContestantResultSeparator}\tBob 20",
        "",
        10,
        "Bob",
        20)]
    public void Ctor_WithNoContestant1Name_SetsHasNoContestant1NameToTrue(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.True(parser.HasNoContestant1Name);
        Assert.False(parser.HasNoContestant1Result);
        Assert.False(parser.HasNoContestant1Score);

        Assert.False(parser.HasNoContestant2Name);
        Assert.False(parser.HasNoContestant2Result);
        Assert.False(parser.HasNoContestant2Score);

        Assert.Equal(expectedContestant1Name, parser.Contestant1Name);
        Assert.Equal(expectedContestant1Score, parser.Contestant1Score);

        Assert.Equal(expectedContestant2Name, parser.Contestant2Name);
        Assert.Equal(expectedContestant2Score, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.HasNoContestant1Result" />
    ///     to <c>true</c> when the input is missing the first contestant result, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator} Bob 20",
        "Bob",
        20)]
    [InlineData(
        $" {ContestResultParser.ContestantResultSeparator} Bob 20",
        "Bob",
        20)]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator} Bob 20 ",
        "Bob",
        20)]
    [InlineData(
        $" {ContestResultParser.ContestantResultSeparator} Bob 20 ",
        "Bob",
        20)]
    [InlineData(
        $" \t {ContestResultParser.ContestantResultSeparator} Bob 20 ",
        "Bob",
        20)]
    [InlineData(
        $" \t {ContestResultParser.ContestantResultSeparator} Bob 20 \t ",
        "Bob",
        20)]
    public void Ctor_WithNoContestant1Result_SetsHasNoContestant1ResultToTrue(
        string input,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.True(parser.HasNoContestant1Name);
        Assert.True(parser.HasNoContestant1Result);
        Assert.True(parser.HasNoContestant1Score);

        Assert.False(parser.HasNoContestant2Name);
        Assert.False(parser.HasNoContestant2Result);
        Assert.False(parser.HasNoContestant2Score);

        Assert.Empty(parser.Contestant1Name);
        Assert.Equal(0, parser.Contestant1Score);

        Assert.Equal(expectedContestant2Name, parser.Contestant2Name);
        Assert.Equal(expectedContestant2Score, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.HasNoContestant1Score" />
    ///     to <c>true</c> when the input is missing the first contestant score, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"Alice{ContestResultParser.ContestantResultSeparator} Bob 20",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice {ContestResultParser.ContestantResultSeparator} Bob 20",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice{ContestResultParser.ContestantResultSeparator} Bob 20 ",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice {ContestResultParser.ContestantResultSeparator} Bob 20 ",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice{ContestResultParser.ContestantResultSeparator}\tBob 20",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice {ContestResultParser.ContestantResultSeparator}\tBob 20",
        "Alice",
        0,
        "Bob",
        20)]
    public void Ctor_WithNoContestant1Score_SetsHasNoContestant1ScoreToTrue(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.False(parser.HasNoContestant1Name);
        Assert.False(parser.HasNoContestant1Result);
        Assert.True(parser.HasNoContestant1Score);

        Assert.False(parser.HasNoContestant2Name);
        Assert.False(parser.HasNoContestant2Result);
        Assert.False(parser.HasNoContestant2Score);

        Assert.Equal(expectedContestant1Name, parser.Contestant1Name);
        Assert.Equal(expectedContestant1Score, parser.Contestant1Score);

        Assert.Equal(expectedContestant2Name, parser.Contestant2Name);
        Assert.Equal(expectedContestant2Score, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.HasNoContestant2Name" />
    ///     to <c>true</c> when the input is missing the second contestant name, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"Alice 20{ContestResultParser.ContestantResultSeparator} 10",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"Alice 20 {ContestResultParser.ContestantResultSeparator} 10",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"Alice 20{ContestResultParser.ContestantResultSeparator} 10 ",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"Alice 20 {ContestResultParser.ContestantResultSeparator} 10 ",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"\tAlice 20{ContestResultParser.ContestantResultSeparator}10",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"\tAlice 20 {ContestResultParser.ContestantResultSeparator} 10",
        "Alice",
        20,
        "",
        10)]
    public void Ctor_WithNoContestant2Name_SetsHasNoContestant2NameToTrue(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.False(parser.HasNoContestant1Name);
        Assert.False(parser.HasNoContestant1Result);
        Assert.False(parser.HasNoContestant1Score);

        Assert.True(parser.HasNoContestant2Name);
        Assert.False(parser.HasNoContestant2Result);
        Assert.False(parser.HasNoContestant2Score);

        Assert.Equal(expectedContestant1Name, parser.Contestant1Name);
        Assert.Equal(expectedContestant1Score, parser.Contestant1Score);

        Assert.Equal(expectedContestant2Name, parser.Contestant2Name);
        Assert.Equal(expectedContestant2Score, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.HasNoContestant2Result" />
    ///     to <c>true</c> when the input is missing the second contestant result, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    [Theory]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator}",
        "Alice",
        10)]
    [InlineData($"Alice 10{ContestResultParser.ContestantResultSeparator} ",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10 {ContestResultParser.ContestantResultSeparator}",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10 {ContestResultParser.ContestantResultSeparator} ",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10 {ContestResultParser.ContestantResultSeparator}\t",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator}\t",
        "Alice",
        10)]
    public void Ctor_WithNoContestant2Result_SetsHasNoContestant2ResultToTrue(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.False(parser.HasNoContestant1Name);
        Assert.False(parser.HasNoContestant1Result);
        Assert.False(parser.HasNoContestant1Score);

        Assert.True(parser.HasNoContestant2Name);
        Assert.True(parser.HasNoContestant2Result);
        Assert.True(parser.HasNoContestant2Score);

        Assert.Equal(expectedContestant1Name, parser.Contestant1Name);
        Assert.Equal(expectedContestant1Score, parser.Contestant1Score);

        Assert.Empty(parser.Contestant2Name);
        Assert.Equal(0, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets <see cref="ContestResultParser.HasNoContestant2Score" />
    ///     to <c>true</c> when the input is missing the second contestant score, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"Alice 20{ContestResultParser.ContestantResultSeparator} Bob",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"Alice 20 {ContestResultParser.ContestantResultSeparator} Bob",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"Alice 20{ContestResultParser.ContestantResultSeparator} Bob ",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"Alice 20 {ContestResultParser.ContestantResultSeparator} Bob ",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"\tAlice 20{ContestResultParser.ContestantResultSeparator}Bob",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"\tAlice 20 {ContestResultParser.ContestantResultSeparator}Bob ",
        "Alice",
        20,
        "Bob",
        0)]
    public void Ctor_WithNoContestant2Score_SetsHasNoContestant2ScoreToTrue(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.False(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.False(parser.HasNoContestant1Name);
        Assert.False(parser.HasNoContestant1Result);
        Assert.False(parser.HasNoContestant1Score);

        Assert.False(parser.HasNoContestant2Name);
        Assert.False(parser.HasNoContestant2Result);
        Assert.True(parser.HasNoContestant2Score);

        Assert.Equal(expectedContestant1Name, parser.Contestant1Name);
        Assert.Equal(expectedContestant1Score, parser.Contestant1Score);

        Assert.Equal(expectedContestant2Name, parser.Contestant2Name);
        Assert.Equal(expectedContestant2Score, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that the constructor sets all properties correctly when the input is valid.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"Alice 0{ContestResultParser.ContestantResultSeparator} Bob 0",
        "Alice",
        0,
        "Bob",
        0)]
    [InlineData(
        $"Alice in Wonderland 0{ContestResultParser.ContestantResultSeparator} Bob the Builder 0",
        "Alice in Wonderland",
        0,
        "Bob the Builder",
        0)]
    [InlineData(
        $"Awesome FC 22{ContestResultParser.ContestantResultSeparator} Boresome FC 11",
        "Awesome FC",
        22,
        "Boresome FC",
        11)]
    public void Ctor_WithValidInput_SetsAllPropertiesCorrectly(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Act
        var parser = new ContestResultParser(input);

        // Assert
        Assert.True(parser.IsValid);

        Assert.False(parser.HasMultipleContestantResultSeparators);
        Assert.False(parser.IsMissingContestantResultSeparator);

        Assert.False(parser.HasNoContestant1Name);
        Assert.False(parser.HasNoContestant1Result);
        Assert.False(parser.HasNoContestant1Score);

        Assert.False(parser.HasNoContestant2Name);
        Assert.False(parser.HasNoContestant2Result);
        Assert.False(parser.HasNoContestant2Score);

        Assert.Equal(expectedContestant1Name, parser.Contestant1Name);
        Assert.Equal(expectedContestant1Score, parser.Contestant1Score);

        Assert.Equal(expectedContestant2Name, parser.Contestant2Name);
        Assert.Equal(expectedContestant2Score, parser.Contestant2Score);
    }

    /// <summary>
    ///     Tests that <see cref="ContestResultParser.GetContestResult" /> throws an
    ///     <see cref="InvalidOperationException" /> when the input is invalid.
    /// </summary>
    [Fact]
    public void GetContestResult_WithInvalidInput_ThrowsInvalidOperationException()
    {
        // Arrange
        var parser = new ContestResultParser("Alice 10 Bob 20");

        // Act
        var exception = Record.Exception(() => parser.GetContestResult());

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("Cannot generate a valid contest result from invalid input.", exception.Message);
    }

    /// <summary>
    ///     Tests that <see cref="ContestResultParser.GetContestResult" /> returns a valid
    ///     <see cref="ContestResult" /> when the input is valid.
    /// </summary>
    [Fact]
    public void GetContestResult_WithValidInput_ReturnsContestResult()
    {
        // Arrange
        var parser = new ContestResultParser($"Alice 10{ContestResultParser.ContestantResultSeparator} Bob 20");

        // Act
        var actual = parser.GetContestResult();

        // Assert
        Assert.NotNull(actual);
        Assert.Equal("Alice", actual.Contestant1Name);
        Assert.Equal(10, actual.Contestant1Score);
        Assert.Equal("Bob", actual.Contestant2Name);
        Assert.Equal(20, actual.Contestant2Score);
    }

    /// <summary>
    ///     Tests that <see cref="ContestResultParser.GetNextError" /> returns the correct error message for invalid input.
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <param name="expected">The expected error message.</param>
    [Theory]
    [InlineData(
        $"1{ContestResultParser.ContestantResultSeparator}2{ContestResultParser.ContestantResultSeparator}3",
        $"A result can only contain one {ContestResultParser.ContestantResultSeparator} symbol.")]
    [InlineData(
        "12345",
        $"A result must be separated into two parts by the {ContestResultParser.ContestantResultSeparator} symbol; one part for each contestant's name and score.")]
    [InlineData(
        $"{ContestResultParser.ContestantResultSeparator} Bob 20",
        "A result must include the results for both contestants. Cannot find a result for contestant 1.")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator}",
        "A result must include the results for both contestants. Cannot find a result for contestant 2.")]
    [InlineData(
        $"10{ContestResultParser.ContestantResultSeparator} Bob 20",
        "A result must include names for both contestants. Cannot find a name for contestant 1.")]
    [InlineData(
        $"Alice{ContestResultParser.ContestantResultSeparator} Bob 20",
        "A result must include scores for both contestants. Cannot find a score for contestant 1.")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator}20",
        "A result must include names for both contestants. Cannot find a name for contestant 2.")]
    [InlineData(
        $"Alice 10{ContestResultParser.ContestantResultSeparator} Bob",
        "A result must include scores for both contestants. Cannot find a score for contestant 2.")]
    public void GetNextError_WithInvalid_ReturnsErrors(string input, string expected)
    {
        // Arrange
        var parser = new ContestResultParser(input);

        // Act
        var actual = parser.GetNextError();

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    ///     Tests that <see cref="ContestResultParser.GetNextError" /> returns <c>null</c> for valid input.
    /// </summary>
    [Fact]
    public void GetNextError_WithValidInput_ReturnsNull()
    {
        // Arrange
        var parser = new ContestResultParser($"Alice 10{ContestResultParser.ContestantResultSeparator} Bob 20");

        // Act
        var actual = parser.GetNextError();

        // Assert
        Assert.Null(actual);
    }
}