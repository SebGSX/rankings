// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Rankings.Parsers;
using Rankings.Resources;
using Rankings.Validators;

namespace Rankings.Extensions;

/// <summary>
///     Extensions for the <see cref="RootCommand" /> class.
/// </summary>
public static class RankingsRootCommandExtensions
{
    /// <summary>
    ///     Adds the append file subcommand to the root command.
    /// </summary>
    /// <param name="rootCommand">The root command receiving the subcommand.</param>
    /// <returns>The root command with the configured subcommand added.</returns>
    public static RootCommand AddAppendFileSubCommand(this RootCommand rootCommand)
    {
        var appendFileCommand = new Command(
            CommandLineSubCommands.AppendFile,
            Common.AppendFile_Subcommand_Description);
        
        var fileOption = new Option<FileInfo>(
            name: CommandLineOptions.FileOption[0],
            aliases: CommandLineOptions.FileOption[1..])
        {
            Description = Common.FileOption_Description,
            Validators = { FileValidator.Validate() }
        };
        
        appendFileCommand.Add(fileOption);
        rootCommand.Add(appendFileCommand);
        
        return rootCommand;
    }
    
    /// <summary>
    ///     Adds the append result subcommand to the root command.
    /// </summary>
    /// <param name="rootCommand">The root command receiving the subcommand.</param>
    /// <returns>The root command with the configured subcommand added.</returns>
    public static RootCommand AddAppendResultSubCommand(this RootCommand rootCommand)
    {
        var appendResultCommand = new Command(
            CommandLineSubCommands.AppendResult,
            Common.AppendResult_Subcommand_Description);
        
        var resultOption = new Option<string>(
            name: CommandLineOptions.ResultOption[0],
            aliases: CommandLineOptions.ResultOption[1..])
        {
            Description = string.Format(Common.ResultOption_Description, ResultParser.ContestantResultSeparator),
            Validators = { ResultValidator.Validate() }
        };
        
        appendResultCommand.Add(resultOption);
        rootCommand.Add(appendResultCommand);
        
        return rootCommand;
    }
}