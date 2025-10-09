// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

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
    ///     Initializes a new instance of the <see cref="ContestResultsProcessor"/> class.
    /// </summary>
    /// <param name="options">The options for the contest results processor.</param>
    /// <param name="storageFactory">The storage factory.</param>
    public ContestResultsProcessor(IOptions<ContestResultsProcessorOptions> options, IStorageFactory storageFactory)
    {
        _options = options;
        _storageFactory = storageFactory;
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
}