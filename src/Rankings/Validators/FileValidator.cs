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
            // The result should never be null.
            Debug.Assert(result != null);
            Debug.Assert(result.GetValueOrDefault<FileInfo>() != null);
            
            var fileInfo = result.GetValueOrDefault<FileInfo>();
            var fileExists = fileInfo.Exists;
            
            /*
             * Identify validation errors in the order they should be reported.
             * While only one validation is currently performed, this structure follows the established pattern and
             * provides for extension in the future with minimal effort.
             */
            var validations = new List<(bool IsError, string ErrorMessage)>
            {
                (!fileExists, string.Format(Common.FileOption_Validation_FileDoesNotExist, fileInfo.FullName))
            };
            
            foreach (var (isError, errorMessage) in validations)
            {
                if (!isError) continue;
                result.AddError(errorMessage);
                return;
            }
        };
    }
}