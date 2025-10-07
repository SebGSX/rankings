// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine;
using Rankings.Resources;

namespace Rankings;

/// <summary>
///     Represents the application.
/// </summary>
public abstract class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var rootCommand = new RootCommand(Common.RootCommand_Description);
        rootCommand.Parse(args).Invoke();
    }
}