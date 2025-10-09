// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Rankings.Parsers;
using Rankings.Resources;
using Rankings.Storage;

namespace Rankings.Services;

/// <inheritdoc />
public class ContestResultsProcessor : IContestResultsProcessor
{
    /// <summary>
    ///     The options for the contest results processor.
    /// </summary>
    private readonly IOptions<ContestResultsProcessorOptions> _options;

    /// <summary>
    ///     The storage factory used to create storage instances.
    /// </summary>
    private readonly IStorageFactory _storageFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ContestResultsProcessor" /> class.
    /// </summary>
    /// <param name="options">The options for the contest results processor.</param>
    /// <param name="storageFactory">The storage factory.</param>
    public ContestResultsProcessor(IOptions<ContestResultsProcessorOptions> options, IStorageFactory storageFactory)
    {
        _options = options;
        _storageFactory = storageFactory;
    }

    /// <inheritdoc />
    public void ClearContestResults()
    {
        var store = _storageFactory.CreateFileStore(_options.Value.FilePath);

        if (store.IsInitialized) store.Reset();

        Console.Write(Common.ContestResultsProcessor_Clear_Success);
    }

    /// <inheritdoc />
    public void DisplayRankingTable()
    {
        var readOnlyStore = _storageFactory.CreateFileReadOnlyStore(_options.Value.FilePath);
        if (!readOnlyStore.IsInitialized || readOnlyStore.IsEmpty)
        {
            Console.Write(Common.ContestResultsProcessor_Display_NoData);
            return;
        }

        // Get the data, then deserialize.
        var allLines = readOnlyStore.ReadAllLines();
        var allResults = new List<ContestResult>();
        foreach (var line in allLines)
        {
            var result = JsonSerializer.Deserialize<ContestResult>(line)!;
            // The result should never be null due to validation during processing.
            Debug.Assert(result != null);
            allResults.Add(result);
        }

        var rankings = CalculateRankings(allResults);

        // Sort the rankings by points (descending) and then by name (ascending).
        var sortedRankings = rankings
            .OrderByDescending(entry => entry.Value)
            .ThenBy(entry => entry.Key)
            .ToList();

        // Display the rankings.
        Console.WriteLine(Common.ContestResultsProcessor_RankingDisplay_Header);
        var rank = 0;
        var rankOffset = 0;
        var previousPoints = -1;
        foreach (var (contestantName, points) in sortedRankings)
        {
            var pointLabel = points == 1
                ? Common.ContestResultsProcessor_RankingDisplay_PointSingular
                : Common.ContestResultsProcessor_RankingDisplay_PointPlural;

            if (previousPoints != points)
            {
                rank += 1 + rankOffset;
                rankOffset = 0;
            }
            else
            {
                rankOffset++;
            }

            previousPoints = points;

            Console.WriteLine(
                Common.ContestResultsProcessor_DisplayRankingTable_Row,
                rank,
                contestantName,
                points,
                pointLabel);
        }
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">Thrown if any of the results are invalid.</exception>
    public void Process(string[] contestResults)
    {
        var parsedResults = new List<ContestResultParser>();
        ContestResultParser? error = null;
        foreach (var contestantResult in contestResults)
        {
            var parser = new ContestResultParser(contestantResult);
            if (!parser.IsValid)
            {
                error = parser;
                break;
            }

            parsedResults.Add(parser);
        }

        if (error != null)
        {
            Console.Error.Write(Common.ContestResultsProcessor_Validation_ParsingError, parsedResults.Count + 1);
            throw new InvalidOperationException(error.GetNextError());
        }

        var store = _storageFactory.CreateFileStore(_options.Value.FilePath);

        if (!store.IsInitialized) store.Initialize();

        var jsonLines = parsedResults
            .Select(contestResult => JsonSerializer
                .Serialize(contestResult.GetContestResult(), new JsonSerializerOptions()))
            .ToArray();

        store.AppendAllLines(jsonLines);

        Console.Write(Common.ContestResultsProcessor_Processing_Success, parsedResults.Count);
    }

    /// <summary>
    ///     Calculates the rankings for contestants based on the provided results.
    /// </summary>
    /// <param name="allResults">The list of all contest results.</param>
    /// <returns>
    ///     A hashtable where the keys are contestant names and the values are their total points.
    /// </returns>
    private static Dictionary<string, ushort> CalculateRankings(List<ContestResult> allResults)
    {
        const ushort pointsForWin = 3;
        const ushort pointsForDraw = 1;
        const ushort pointsForLoss = 0;

        var rankings = new Dictionary<string, ushort>();

        foreach (var result in allResults)
        {
            var contestant1Points = result.Contestant1Score > result.Contestant2Score
                ? pointsForWin
                : pointsForLoss;
            if (result.Contestant1Score == result.Contestant2Score)
                contestant1Points = pointsForDraw;

            var contestant2Points = result.Contestant2Score > result.Contestant1Score
                ? pointsForWin
                : pointsForLoss;
            if (result.Contestant1Score == result.Contestant2Score)
                contestant2Points = pointsForDraw;

            if (!rankings.TryAdd(result.Contestant1Name, contestant1Points))
                rankings[result.Contestant1Name] =
                    (ushort)(rankings[result.Contestant1Name] + contestant1Points);
            if (!rankings.TryAdd(result.Contestant2Name, contestant2Points))
                rankings[result.Contestant2Name] =
                    (ushort)(rankings[result.Contestant2Name] + contestant2Points);
        }

        return rankings;
    }
}