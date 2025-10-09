// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

namespace Rankings.Services;

/// <summary>
///     Represents a service that processes contest results by parsing then storing them.
/// </summary>
public interface IContestResultsProcessor
{
    /// <summary>
    ///     Processes the provided contest results.
    /// </summary>
    /// <param name="contestResults">The contest results to process.</param>
    public void Process(string[] contestResults);
}