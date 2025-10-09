// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rankings.Services;

/// <summary>
///     Represents options for configuring the <see cref="ContestResultsProcessor" />.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "POCOs are not tested.")]
[DebuggerDisplay("{" + nameof(FilePath) + "}", Name = nameof(FilePath))]
public class ContestResultsProcessorOptions
{
    /// <summary>
    ///     Gets or sets the file path where contest results are stored and retrieved after processing.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;
}