// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Rankings.Extensions;
using Rankings.Resources;
using Rankings.Services;
using Rankings.Storage;

namespace Rankings;

/// <summary>
///     Represents the application.
/// </summary>
public abstract class Program
{
    /// <summary>
    ///     Represents the root command of the application.
    /// </summary>
    private static RootCommand? _rootCommand;
    
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IStorageFactory, StorageFactory>();
        serviceCollection.AddScoped<IContestResultsProcessor, ContestResultsProcessor>();

        serviceCollection.Configure<ContestResultsProcessorOptions>(options =>
        {
            options.FilePath = "contest-results.jsonl";
        });
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        _rootCommand = new RootCommand(Common.RootCommand_Description);
        _rootCommand.AddAppendFileSubcommand(serviceProvider);
        _rootCommand.AddAppendResultSubcommand(serviceProvider);
        _rootCommand.AddClearContestResultsSubcommand(serviceProvider);
        _rootCommand.SetAction(_ => 0);
        
        // Automatically handles unhandled exceptions thrown during parsing or invocation.
        _rootCommand.Parse(args).Invoke();
    }
    
    /// <summary>
    ///     Gets the configured options for the application.
    /// </summary>
    public static IEnumerable<string> ConfiguredOptions => _rootCommand!.Options.Select(o => o.Name);
    
    /// <summary>
    ///     Gets the configured subcommands for the application.
    /// </summary>
    public static IEnumerable<string> ConfiguredSubcommands => _rootCommand!.Subcommands.Select(s => s.Name);
}