// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings;

/// <summary>
///     Represents the sets of command line options including their aliases.
/// </summary>
public abstract record CommandLineOptions
{
    /// <summary>
    ///     Represents the result option and its aliases.
    /// </summary>
    /// <remarks>The first element is the primary name, and the rest are aliases.</remarks>
    public static string[] ResultOption
    {
        get;
    } = ["--result", "-r"];
}