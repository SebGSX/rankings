// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings;

/// <summary>
///     Represents the command line subcommands.
/// </summary>
public abstract record CommandLineSubCommands
{
    /// <summary>
    ///     Represents the append-file subcommand used to append contestant results from a file.
    /// </summary>
    public static string AppendFile { get; } = "append-file";
    
    /// <summary>
    ///     Represents the append-result subcommand used to append a contestant result from the command line.
    /// </summary>
    public static string AppendResult { get; } = "append-result";
}