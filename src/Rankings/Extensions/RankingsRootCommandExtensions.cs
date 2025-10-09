// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Rankings.Parsers;
using Rankings.Resources;
using Rankings.Services;
using Rankings.Storage;
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

        /*
         * The handler for a command is dependent on its options and arguments. As such, the cleanest way to define
         * the handler is where the options and arguments are defined to avoid brittle abstractions.
         */
        appendFileCommand.SetAction(result =>
        {
            // The result should never be null.
            Debug.Assert(result != null);
            Debug.Assert(result.GetValue(fileOption) != null);

            try
            {
                // Get the option value.
                var filePath = result.GetValue(fileOption)!;
                
                // Resolve dependencies.
                var storageFactory = serviceProvider.GetService<IStorageFactory>()
                                     ?? throw new InvalidOperationException(Common.StorageFactory_NotRegistered);
                var processor = serviceProvider.GetService<IContestResultsProcessor>()
                                ?? throw new InvalidOperationException(Common.ContestResultsProcessor_NotRegistered);
                
                // Get the contest results from the file.
                var fileReadOnlyStore = storageFactory.CreateFileReadOnlyStore(filePath);
                if (!fileReadOnlyStore.IsInitialized)
                {
                    throw new InvalidOperationException(Common.CommonAppendFile_Subcommand_FileNotAccessible);
                }

                var contestantResults = fileReadOnlyStore.ReadAllLines();
                
                // Process the contest results.
                processor.Process(contestantResults);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.ExitCode = 1;
                return Environment.ExitCode;
            }

            Environment.ExitCode = 0;
            return Environment.ExitCode;
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
            Description = string.Format(Common.ResultOption_Description, ContestResultParser.ContestantResultSeparator),
            Validators = { ResultValidator.Validate() }
        };
        
        /*
         * The handler for a command is dependent on its options and arguments. As such, the cleanest way to define
         * the handler is where the options and arguments are defined to avoid brittle abstractions.
         */
        appendResultCommand.SetAction(result =>
        {
            // The result should never be null.
            Debug.Assert(result != null);
            Debug.Assert(result.GetValue(resultOption) != null);

            try
            {
                // Get the option value.
                var contestantResults = new[] { result.GetValue(resultOption)! };
                
                // Resolve dependencies.
                var processor = serviceProvider.GetService<IContestResultsProcessor>()
                                ?? throw new InvalidOperationException(Common.ContestResultsProcessor_NotRegistered);
                
                // Process the contest results.
                processor.Process(contestantResults);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.ExitCode = 1;
                return Environment.ExitCode;
            }

            Environment.ExitCode = 0;
            return Environment.ExitCode;
        });
        
        appendResultCommand.Add(resultOption);
        rootCommand.Add(appendResultCommand);
        
        return rootCommand;
    }
    
    /// <summary>
    ///     Adds the clear contest results subcommand to the root command.
    /// </summary>
    /// <param name="rootCommand">The root command receiving the subcommand.</param>
    /// <param name="serviceProvider">The service provider used to support dependency injection.</param>
    /// <returns>The root command with the configured subcommand added.</returns>
    public static RootCommand AddClearContestResultsSubcommand(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var clearContestResultsCommand = new Command(
            CommandLineSubcommands.ClearContestResults,
            Common.ClearContestResults_Subcommand_Description);
        
        /*
         * The handler for a command is dependent on its options and arguments. As such, the cleanest way to define
         * the handler is where the options and arguments are defined to avoid brittle abstractions.
         */
        clearContestResultsCommand.SetAction(_ =>
        {
            try
            {
                // Resolve dependencies.
                var processor = serviceProvider.GetService<IContestResultsProcessor>()
                                ?? throw new InvalidOperationException(Common.ContestResultsProcessor_NotRegistered);
                
                // Clear all contest results that were previously processed and stored.
                processor.ClearContestResults();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.ExitCode = 1;
                return Environment.ExitCode;
            }

            Environment.ExitCode = 0;
            return Environment.ExitCode;
        });
        
        rootCommand.Add(clearContestResultsCommand);
        
        return rootCommand;
    }
}