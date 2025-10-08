// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine.Parsing;
using System.Diagnostics;
using Rankings.Parsers;
using Rankings.Resources;

namespace Rankings.Validators;

/// <summary>
///     Validates the result option.
/// </summary>
public abstract class ResultValidator
{
    /// <summary>
    ///     Ensures that the option result contains a valid contestant result.
    /// </summary>
    /// <returns>An action that validates the option result.</returns>
    public static Action<OptionResult> Validate()
    {
        return result =>
        {
            try
            {
                // The result should never be null.
                Debug.Assert(result != null);
                Debug.Assert(result.GetValueOrDefault<string>() != null);
                var resultParser = new ResultParser(result.GetValueOrDefault<string>());

                // Identify validation errors in the order they should be reported.
                var validations = new List<(bool IsError, string ErrorMessage)>
                {
                    (resultParser.HasMultipleContestantResultSeparators,
                        string.Format(
                            Common.ResultOption_Validation_MultipleContestantResultSeparators,
                            ResultParser.ContestantResultSeparator)),
                    (resultParser.IsMissingContestantResultSeparator,
                        string.Format(
                            Common.ResultOption_Validation_MissingContestantResultSeparator,
                            ResultParser.ContestantResultSeparator)),
                    (resultParser.HasNoContestant1Result,
                        string.Format(Common.ResultOption_Validation_NoContestantResult, 1)),
                    (resultParser.HasNoContestant2Result,
                        string.Format(Common.ResultOption_Validation_NoContestantResult, 2)),
                    (resultParser.HasNoContestant1Name,
                        string.Format(Common.ResultOption_Validation_NoContestantName, 1)),
                    (resultParser.HasNoContestant1Score,
                        string.Format(Common.ResultOption_Validation_NoContestantScore, 1)),
                    (resultParser.HasNoContestant2Name,
                        string.Format(Common.ResultOption_Validation_NoContestantName, 2)),
                    (resultParser.HasNoContestant2Score,
                        string.Format(Common.ResultOption_Validation_NoContestantScore, 2))
                };
                
                // Stop validation on the first error found, working from left to right.
                foreach (var (isError, errorMessage) in validations)
                {
                    if (!isError) continue;
                    result.AddError(errorMessage);
                    return;
                }
            }
            catch (ArgumentException)
            {
                result.AddError(Common.ResultOption_Validation_NewLineWithinContestantResult);
            }
        };
    }
}