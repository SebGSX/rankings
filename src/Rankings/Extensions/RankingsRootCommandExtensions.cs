// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using System.Diagnostics;
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
    /// <param name="serviceProvider">The service provider used to support dependency injection.</param>
    /// <returns>The root command with the configured subcommand added.</returns>
    public static RootCommand AddAppendFileSubcommand(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var appendFileCommand = new Command(
            CommandLineSubcommands.AppendFile,
            Common.AppendFile_Subcommand_Description);
        
        var fileOption = new Option<string>(
            name: CommandLineOptions.FileOption[0],
            aliases: CommandLineOptions.FileOption[1..])
        {
            Description = Common.FileOption_Description,
            Validators = { FileValidator.Validate() }
        };

        appendFileCommand.SetAction(result =>
        {
            // The result should never be null.
            Debug.Assert(result != null);
            Debug.Assert(result.GetValue(fileOption) != null);
        });
        
        appendFileCommand.Add(fileOption);
        rootCommand.Add(appendFileCommand);
        
        return rootCommand;
    }
    
    /// <summary>
    ///     Adds the append result subcommand to the root command.
    /// </summary>
    /// <param name="rootCommand">The root command receiving the subcommand.</param>
    /// <param name="serviceProvider">The service provider used to support dependency injection.</param>
    /// <returns>The root command with the configured subcommand added.</returns>
    public static RootCommand AddAppendResultSubcommand(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var appendResultCommand = new Command(
            CommandLineSubcommands.AppendResult,
            Common.AppendResult_Subcommand_Description);
        
        var resultOption = new Option<string>(
            name: CommandLineOptions.ResultOption[0],
            aliases: CommandLineOptions.ResultOption[1..])
        {
            Description = string.Format(Common.ResultOption_Description, ResultParser.ContestantResultSeparator),
            Validators = { ResultValidator.Validate() }
        };
        
        appendResultCommand.SetAction(result =>
        {
            // The result should never be null.
            Debug.Assert(result != null);
            Debug.Assert(result.GetValue(resultOption) != null);
        });
        
        appendResultCommand.Add(resultOption);
        rootCommand.Add(appendResultCommand);
        
        return rootCommand;
    }
}