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
    ///     Ensures that the option result contains a valid contest result.
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

                var contestResultParser = new ContestResultParser(result.GetValueOrDefault<string>());
                var error = contestResultParser.GetNextError();

                if (!string.IsNullOrEmpty(error))
                {
                    result.AddError(error);
                }
            }
            catch (ArgumentException)
            {
                result.AddError(Common.ContestResultParser_Validation_NewLineWithinContestantResult);
            }
        };
    }
}