// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Rankings.Extensions;
using Rankings.Resources;

namespace Rankings;

/// <summary>
///     Represents the application.
/// </summary>
public abstract class Program
{
    /// <summary>
    ///     Represents the root command of the application.
    /// </summary>
    private static readonly RootCommand RootCommand = new(Common.RootCommand_Description);
    
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        RootCommand.AddResultOption();
        RootCommand.AddFileOption();
        
        // Automatically handles unhandled exceptions thrown during parsing or invocation.
        RootCommand.Parse(args).Invoke();
    }
    
    /// <summary>
    ///     Gets the configured options for the application.
    /// </summary>
    public static IList<Option> ConfiguredOptions => RootCommand.Options;
}