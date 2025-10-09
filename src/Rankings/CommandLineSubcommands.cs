// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings;

/// <summary>
///     Represents the command line subcommands.
/// </summary>
public abstract record CommandLineSubcommands
{
    /// <summary>
    ///     Represents the append-file subcommand used to append contest results from a file.
    /// </summary>
    public static string AppendFile { get; } = "append-file";

    /// <summary>
    ///     Represents the append-result subcommand used to append a contest result from the command line.
    /// </summary>
    public static string AppendResult { get; } = "append-result";

    /// <summary>
    ///     Represents the clear-contest-results subcommand used to clear all contest results.
    /// </summary>
    public static string ClearContestResults { get; } = "clear-contest-results";
}