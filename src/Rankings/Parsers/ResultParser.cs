// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.Diagnostics;
using Rankings.Resources;

namespace Rankings.Parsers;

/// <summary>
///     Represents the contestant result parser.
/// </summary>
/// <remarks>Operates on a single line of input only.</remarks>
public class ResultParser
{
    /// <summary>
    ///     The character that separates individual contestant results in an input string.
    /// </summary>
    public const string ContestantResultSeparator = ",";
    
    /// <summary>
    ///     The character that separates a contestant's name from their score in an input string.
    /// </summary>
    private const string ContestantScoreSeparator = " ";
    
    /// <summary>
    ///     Indicates that a value was not found when parsing.
    /// </summary>
    private const int NotFound = -1;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ResultParser" /> class with the specified input string.
    /// </summary>
    /// <param name="input">The input to parse for contestant results.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="input" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="input" /> is empty, consists only of whitespace, or contains new line characters.
    /// </exception>
    public ResultParser(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        // Check for new line characters in the input first to avoid ambiguity with whitespace check.
        if (input.Contains('\n', StringComparison.InvariantCultureIgnoreCase)
            || input.Contains('\r', StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ArgumentException(Common.CannotContainNewLine, nameof(input));
        }
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException(Common.EmptyOrWhiteSpace, nameof(input));
        }
        var inputTrimmed = input.Trim();
        
        /*
         * Validate the input and store the result flags.
         *
         * Since string operations are generally costly and a results file may contain many entries, performing the
         * validation once during initialization and storing the result flags is more efficient than multiple checks
         * for the same condition for each contestant result. While seemingly small, the cumulative performance impact
         * over a large input dataset can be significant depending on how the flags are used later in the processing.
         */
        var firstIndex = inputTrimmed.IndexOf(ContestantResultSeparator, StringComparison.InvariantCultureIgnoreCase);
        var lastIndex = inputTrimmed.LastIndexOf(ContestantResultSeparator, StringComparison.InvariantCultureIgnoreCase);
        
        #region Validate Basic Structure
        HasMultipleContestantResultSeparators = firstIndex != lastIndex;
        IsMissingContestantResultSeparator = firstIndex == NotFound;

        if (HasMultipleContestantResultSeparators
            || IsMissingContestantResultSeparator)
        {
            /*
             * If the input is invalid, set default values for contestant names and scores as well as the associated
             * flags.
             */
            Contestant1Name = string.Empty;
            Contestant1Score = 0;
            Contestant2Name = string.Empty;
            Contestant2Score = 0;
            
            HasNoContestant1Name = true;
            HasNoContestant1Result = true;
            HasNoContestant1Score = true;
            HasNoContestant2Name = true;
            HasNoContestant2Result = true;
            HasNoContestant2Score = true;
            return;
        }
        #endregion
        
        var contestantResults = inputTrimmed.Split(ContestantResultSeparator);
        // There should always be exactly two results after splitting by the separator.
        Debug.Assert(contestantResults.Length == 2);
        var contestant1ResultString = contestantResults[0].Trim();
        var contestant2ResultString = contestantResults[1].Trim();

        var contestant1Result = ParseContestantResult(contestant1ResultString);
        var contestant2Result = ParseContestantResult(contestant2ResultString);

        #region Validate Parsed Results
        HasNoContestant1Result = firstIndex == 0; // Separator is at the start.
        HasNoContestant2Result = lastIndex == inputTrimmed.Length - 1; // Separator is at the end.

        HasNoContestant1Name = string.IsNullOrWhiteSpace(contestant1Result.Name);
        HasNoContestant1Score = contestant1Result.Score == NotFound;
        HasNoContestant2Name = string.IsNullOrWhiteSpace(contestant2Result.Name);
        HasNoContestant2Score = contestant2Result.Score == NotFound;
        
        Contestant1Name = contestant1Result.Name;
        Contestant1Score = contestant1Result.Score != NotFound ? (ushort)contestant1Result.Score : (ushort)0;
        Contestant2Name = contestant2Result.Name;
        Contestant2Score = contestant2Result.Score != NotFound ? (ushort)contestant2Result.Score : (ushort)0;
        #endregion
    }
    
    /// <summary>
    ///     Gets the first contestant's name, if any.
    /// </summary>
    /// <remarks>
    ///     If the name is missing, this property returns an empty string.
    /// </remarks>
    public string Contestant1Name { get; }
    
    /// <summary>
    ///     Gets the first contestant's score, if any.
    /// </summary>
    /// <remarks>
    ///     If the score is missing or invalid, this property returns <c>0</c>.
    /// </remarks>
    public ushort Contestant1Score { get; }
    
    /// <summary>
    ///     Gets the second contestant's name, if any.
    /// </summary>
    /// <remarks>
    ///     If the name is missing, this property returns an empty string.
    /// </remarks>
    public string Contestant2Name { get; }
    
    /// <summary>
    ///     Gets the second contestant's score, if any.
    /// </summary>
    /// <remarks>
    ///     If the score is missing or invalid, this property returns <c>0</c>.
    /// </remarks>
    public ushort Contestant2Score { get; }

    /// <summary>
    ///     Indicates whether the input has multiple contestant result separators.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input has multiple contestant result separators; otherwise, <c>false</c>.
    /// </returns>
    public bool HasMultipleContestantResultSeparators { get; }
    
    /// <summary>
    ///     Indicates whether the input is missing the first contestant's name.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input has no first contestant name; otherwise, <c>false</c>.
    /// </returns>
    public bool HasNoContestant1Name { get; }

    /// <summary>
    ///     Indicates whether the input is missing the first contestant result.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input has no first contestant result separators; otherwise, <c>false</c>.
    /// </returns>
    public bool HasNoContestant1Result { get; }

    /// <summary>
    ///     Indicates whether the input is missing the first contestant's score.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input has no first contestant score; otherwise, <c>false</c>.
    /// </returns>
    public bool HasNoContestant1Score { get; }

    /// <summary>
    ///     Indicates whether the input is missing the second contestant's name.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input has no second contestant name; otherwise, <c>false</c>.
    /// </returns>
    public bool HasNoContestant2Name { get; }
    
    /// <summary>
    ///     Indicates whether the input is missing the second contestant result.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input has no second contestant result separators; otherwise, <c>false</c>.
    /// </returns>
    public bool HasNoContestant2Result { get; }
    
    /// <summary>
    ///     Indicates whether the input is missing the second contestant's score.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input has no second contestant score; otherwise, <c>false</c>.
    /// </returns>
    public bool HasNoContestant2Score { get; }

    /// <summary>
    ///     Indicates whether the input is missing the contestant result separator.
    /// </summary>
    /// <returns>
    ///     <c>True</c> if the input is missing the contestant result separator; otherwise, <c>false</c>.
    /// </returns>
    public bool IsMissingContestantResultSeparator { get; }

    /// <summary>
    ///     Parses a contestant result string into a name and score.
    /// </summary>
    /// <param name="contestantResult">The contestant result string to parse.</param>
    /// <returns>
    ///     A tuple containing the contestant's name and score. If the name is missing, an empty string is returned. If
    ///     the score is missing or invalid, <c>-1</c> is returned.
    /// </returns>
    private static (string Name, short Score) ParseContestantResult(string contestantResult)
    {
        var contestantLastSpaceIndex = contestantResult
            .LastIndexOf(ContestantScoreSeparator, StringComparison.InvariantCultureIgnoreCase);
        
        // If there's no space, either the name or score is missing.
        if (contestantLastSpaceIndex == NotFound)
        {
            return short.TryParse(contestantResult, out var parsedOnlyScore)
                ? (string.Empty, parsedOnlyScore) // Only the score is present.
                : (contestantResult, (short) NotFound); // Only the name is present.
        }

        // Try to parse the score from the remainder of the text; if it fails, return noScore.
        return short.TryParse(contestantResult[(contestantLastSpaceIndex + 1)..].Trim(), out var score)
            ? (contestantResult[..contestantLastSpaceIndex].Trim(), score) // Successfully parsed both name and score.
            : (contestantResult, (short) NotFound); // Name is present, but score parsing failed.
    }
}