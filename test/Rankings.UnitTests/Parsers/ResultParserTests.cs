// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using Rankings.Parsers;

namespace Rankings.UnitTests.Parsers;

/// <summary>
///     Unit tests for the <see cref="ResultParser" /> class.
/// </summary>
public class ResultParserTests
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
        var exception = Record.Exception(() => new ResultParser(input));
        
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
        $"{ResultParser.ContestantResultSeparator}",
        "",
        0,
        "",
        0)]
    [InlineData(
        $"10{ResultParser.ContestantResultSeparator}",
        "",
        10,
        "",
        0)]
    [InlineData(
        $"{ResultParser.ContestantResultSeparator}10",
        "",
        0,
        "",
        10)]
    [InlineData(
        $"10{ResultParser.ContestantResultSeparator}10",
        "",
        10,
        "",
        10)]
    [InlineData(
        $"Alice in Wonderland{ResultParser.ContestantResultSeparator}",
        "Alice in Wonderland",
        0,
        "",
        0)]
    [InlineData(
        $"{ResultParser.ContestantResultSeparator}Bob the Builder",
        "",
        0,
        "Bob the Builder",
        0)]
    [InlineData(
        $"Alice in Wonderland{ResultParser.ContestantResultSeparator}Bob the Builder",
        "Alice in Wonderland",
        0,
        "Bob the Builder",
        0)]
    [InlineData(
        $"Alice in Wonderland{ResultParser.ContestantResultSeparator}10",
        "Alice in Wonderland",
        0,
        "",
        10)]
    [InlineData(
        $"10{ResultParser.ContestantResultSeparator}Bob the Builder",
        "",
        10,
        "Bob the Builder",
        0)]
    [InlineData(
        $"-1{ResultParser.ContestantResultSeparator}",
        "",
        0,
        "",
        0)]
    [InlineData(
        $"{ResultParser.ContestantResultSeparator}-1",
        "",
        0,
        "",
        0)]
    [InlineData(
        $"Alice in Wonderland-1{ResultParser.ContestantResultSeparator}",
        "Alice in Wonderland-1",
        0,
        "",
        0)]
    [InlineData(
        $"{ResultParser.ContestantResultSeparator}Bob the Builder-1",
        "",
        0,
        "Bob the Builder-1",
        0)]
    [InlineData(
        $"-1Alice in Wonderland{ResultParser.ContestantResultSeparator}",
        "-1Alice in Wonderland",
        0,
        "",
        0)]
    [InlineData(
        $"{ResultParser.ContestantResultSeparator}-1Bob the Builder",
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
        var contestantResults = input.Split(ResultParser.ContestantResultSeparator);
        var hasNoContestant1ResultExpected = string.IsNullOrWhiteSpace(contestantResults[0]);
        var hasNoContestant2ResultExpected = string.IsNullOrWhiteSpace(contestantResults[1]);

        // Act
        var parser = new ResultParser(input);

        // Assert
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
        var exception = Record.Exception(() => new ResultParser(input));
        
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
        var exception = Record.Exception(() => new ResultParser(input));
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("input", ((ArgumentNullException)exception).ParamName);
    }
    
    /// <summary>
    ///     Tests that the constructor sets <see cref="ResultParser.IsMissingContestantResultSeparator" />
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
        var parser = new ResultParser(input);
        
        // Assert
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
    ///     Tests that the constructor sets <see cref="ResultParser.HasMultipleContestantResultSeparators" />
    ///     to <c>true</c> when the input contains multiple contestant result separators, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    [Theory]
    [InlineData(
        $"Alice 10{ResultParser.ContestantResultSeparator} Bob 20{ResultParser.ContestantResultSeparator} Charlie 30")]
    [InlineData(
        $"Alice 10 {ResultParser.ContestantResultSeparator} Bob 20 {ResultParser.ContestantResultSeparator} Charlie 30")]
    [InlineData(
        $"Alice 10 {ResultParser.ContestantResultSeparator}Bob 20 {ResultParser.ContestantResultSeparator}Charlie 30")]
    [InlineData(
        $"Alice 10{ResultParser.ContestantResultSeparator} Bob 20 {ResultParser.ContestantResultSeparator}Charlie 30")]
    [InlineData(
        $"Alice 10 {ResultParser.ContestantResultSeparator} Bob 20{ResultParser.ContestantResultSeparator} Charlie 30")]
    [InlineData(
        $"Alice 10{ResultParser.ContestantResultSeparator}Bob 20{ResultParser.ContestantResultSeparator}Charlie 30")]
    public void Ctor_WithMultipleContestantResultSeparators_SetsHasMultipleContestantResultSeparatorsToTrue(
        string input)
    {
        // Act
        var parser = new ResultParser(input);
        
        // Assert
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
    ///     Tests that the constructor sets <see cref="ResultParser.HasNoContestant1Name" />
    ///     to <c>true</c> when the input is missing the first contestant name, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"10{ResultParser.ContestantResultSeparator} Bob 20",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $" 10{ResultParser.ContestantResultSeparator} Bob 20",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $"10{ResultParser.ContestantResultSeparator} Bob 20 ",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $" 10{ResultParser.ContestantResultSeparator} Bob 20 ",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $"10{ResultParser.ContestantResultSeparator}\tBob 20",
        "",
        10,
        "Bob",
        20)]
    [InlineData(
        $" 10{ResultParser.ContestantResultSeparator}\tBob 20",
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
        var parser = new ResultParser(input);
        
        // Assert
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
    ///     Tests that the constructor sets <see cref="ResultParser.HasNoContestant1Result" />
    ///     to <c>true</c> when the input is missing the first contestant result, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"{ResultParser.ContestantResultSeparator} Bob 20",
        "Bob",
        20)]
    [InlineData(
        $" {ResultParser.ContestantResultSeparator} Bob 20",
        "Bob",
        20)]
    [InlineData(
        $"{ResultParser.ContestantResultSeparator} Bob 20 ",
        "Bob",
        20)]
    [InlineData(
        $" {ResultParser.ContestantResultSeparator} Bob 20 ",
        "Bob",
        20)]
    [InlineData(
        $" \t {ResultParser.ContestantResultSeparator} Bob 20 ",
        "Bob",
        20)]
    [InlineData(
        $" \t {ResultParser.ContestantResultSeparator} Bob 20 \t ",
        "Bob",
        20)]
    public void Ctor_WithNoContestant1Result_SetsHasNoContestant1ResultToTrue(
        string input,
        string expectedContestant2Name,
        ushort expectedContestant2Score)
    {
        // Act
        var parser = new ResultParser(input);
        
        // Assert
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
    ///     Tests that the constructor sets <see cref="ResultParser.HasNoContestant1Score" />
    ///     to <c>true</c> when the input is missing the first contestant score, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"Alice{ResultParser.ContestantResultSeparator} Bob 20",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice {ResultParser.ContestantResultSeparator} Bob 20",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice{ResultParser.ContestantResultSeparator} Bob 20 ",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice {ResultParser.ContestantResultSeparator} Bob 20 ",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice{ResultParser.ContestantResultSeparator}\tBob 20",
        "Alice",
        0,
        "Bob",
        20)]
    [InlineData(
        $"Alice {ResultParser.ContestantResultSeparator}\tBob 20",
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
        var parser = new ResultParser(input);
        
        // Assert
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
    ///     Tests that the constructor sets <see cref="ResultParser.HasNoContestant2Name" />
    ///     to <c>true</c> when the input is missing the second contestant name, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"Alice 20{ResultParser.ContestantResultSeparator} 10",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"Alice 20 {ResultParser.ContestantResultSeparator} 10",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"Alice 20{ResultParser.ContestantResultSeparator} 10 ",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"Alice 20 {ResultParser.ContestantResultSeparator} 10 ",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"\tAlice 20{ResultParser.ContestantResultSeparator}10",
        "Alice",
        20,
        "",
        10)]
    [InlineData(
        $"\tAlice 20 {ResultParser.ContestantResultSeparator} 10",
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
        var parser = new ResultParser(input);
        
        // Assert
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
    ///     Tests that the constructor sets <see cref="ResultParser.HasNoContestant2Result" />
    ///     to <c>true</c> when the input is missing the second contestant result, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    [Theory]
    [InlineData(
        $"Alice 10{ResultParser.ContestantResultSeparator}",
        "Alice",
        10)]
    [InlineData($"Alice 10{ResultParser.ContestantResultSeparator} ",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10 {ResultParser.ContestantResultSeparator}",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10 {ResultParser.ContestantResultSeparator} ",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10 {ResultParser.ContestantResultSeparator}\t",
        "Alice",
        10)]
    [InlineData(
        $"Alice 10{ResultParser.ContestantResultSeparator}\t",
        "Alice",
        10)]
    public void Ctor_WithNoContestant2Result_SetsHasNoContestant2ResultToTrue(
        string input,
        string expectedContestant1Name,
        ushort expectedContestant1Score)
    {
        // Act
        var parser = new ResultParser(input);

        // Assert
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
    ///     Tests that the constructor sets <see cref="ResultParser.HasNoContestant2Score" />
    ///     to <c>true</c> when the input is missing the second contestant score, regardless of spacing.
    /// </summary>
    /// <param name="input">The input to parse.</param>
    /// <param name="expectedContestant1Name">The expected name of the first contestant.</param>
    /// <param name="expectedContestant1Score">The expected score of the first contestant.</param>
    /// <param name="expectedContestant2Name">The expected name of the second contestant.</param>
    /// <param name="expectedContestant2Score">The expected score of the second contestant.</param>
    [Theory]
    [InlineData(
        $"Alice 20{ResultParser.ContestantResultSeparator} Bob",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"Alice 20 {ResultParser.ContestantResultSeparator} Bob",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"Alice 20{ResultParser.ContestantResultSeparator} Bob ",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"Alice 20 {ResultParser.ContestantResultSeparator} Bob ",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"\tAlice 20{ResultParser.ContestantResultSeparator}Bob",
        "Alice",
        20,
        "Bob",
        0)]
    [InlineData(
        $"\tAlice 20 {ResultParser.ContestantResultSeparator}Bob ",
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
        var parser = new ResultParser(input);
        
        // Assert
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
        $"Alice 0{ResultParser.ContestantResultSeparator} Bob 0",
        "Alice",
        0,
        "Bob",
        0)]
    [InlineData(
        $"Alice in Wonderland 0{ResultParser.ContestantResultSeparator} Bob the Builder 0",
        "Alice in Wonderland",
        0,
        "Bob the Builder",
        0)]
    [InlineData(
        $"Awesome FC 22{ResultParser.ContestantResultSeparator} Boresome FC 11",
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
        var parser = new ResultParser(input);
        
        // Assert
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
}