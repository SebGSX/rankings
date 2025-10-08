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
    ///     Adds the result option to the root command.
    /// </summary>
    /// <param name="rootCommand">The root command receiving the option.</param>
    /// <returns>The root command with the option added.</returns>
    public static RootCommand AddResultOption(this RootCommand rootCommand)
    {
        var resultOption = new Option<string>(
            name: CommandLineOptions.ResultOption[0],
            aliases: CommandLineOptions.ResultOption[1..])
        {
            Description = string.Format(Common.ResultOption_Description, ResultParser.ContestantResultSeparator),
            Validators = { ResultValidator.Validate() }
        };
        
        rootCommand.Options.Add(resultOption);
        
        return rootCommand;
    }
}