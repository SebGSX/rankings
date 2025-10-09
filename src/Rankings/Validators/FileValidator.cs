// Copyright © 2025 Seb Garrioch. All rights reserved.
// Published under the MIT License.

using System.CommandLine.Parsing;
using System.Diagnostics;
using Rankings.Resources;

namespace Rankings.Validators;

/// <summary>
///     Validates the file option.
/// </summary>
public abstract class FileValidator
{
    /// <summary>
    ///     Ensures that the option result contains a valid file with contestant results.
    /// </summary>
    /// <returns>An action that validates the option result.</returns>
    public static Action<OptionResult> Validate()
    {
        return result =>
        {
            const int notFound = -1;
            
            // The result should never be null.
            Debug.Assert(result != null);
            Debug.Assert(result.GetValueOrDefault<string>() != null);
            
            var filePath = result.GetValueOrDefault<string>();
            var hasFilePath = !string.IsNullOrWhiteSpace(filePath);
            
            // Identify validation errors in the order they should be reported.
            var validations = new List<(bool IsError, string ErrorMessage)>
            {
                (!hasFilePath, Common.FileOption_Validation_MissingFileName)
            };

            // Guard against missing or empty file path before checking for invalid characters.
            if (hasFilePath)
            {
                var fileInfo = new FileInfo(filePath);
                var hasFileName = !string.IsNullOrWhiteSpace(fileInfo.Name);

                // Identify validation errors in the order they should be reported.
                validations.AddRange(new List<(bool, string)>
                {
                    (!hasFileName, Common.FileOption_Validation_MissingFileName),
                    (hasFileName && fileInfo.Name.IndexOfAny(Path.GetInvalidFileNameChars()) != notFound,
                        Common.FileOption_Validation_InvalidFileName),
                    (fileInfo.DirectoryName!.IndexOfAny(Path.GetInvalidPathChars()) != notFound,
                        Common.FileOption_Validation_InvalidDirectoryName),
                });
            }

            foreach (var (isError, errorMessage) in validations)
            {
                if (!isError) continue;
                result.AddError(errorMessage);
                return;
            }
        };
    }
}