// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.Diagnostics;

namespace Rankings.Parsers;

/// <summary>
///     Represents a contest result.
/// </summary>
[DebuggerDisplay("{" + nameof(ToString) + "()}")]
public class ContestResult
{
    /// <summary>
    ///     Gets the first contestant's name, if any.
    /// </summary>
    public string Contestant1Name { get; set; } = string.Empty;
    
    /// <summary>
    ///     Gets the first contestant's score, if any.
    /// </summary>
    public ushort Contestant1Score { get; set; }
    
    /// <summary>
    ///     Gets the second contestant's name, if any.
    /// </summary>
    public string Contestant2Name { get; set; } = string.Empty;
    
    /// <summary>
    ///     Gets the second contestant's score, if any.
    /// </summary>
    public ushort Contestant2Score { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return
            $"{Contestant1Name} {Contestant1Score}{ContestResultParser.ContestantResultSeparator} {Contestant2Name} {Contestant2Score}";
    }
}