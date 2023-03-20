using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Coded.AspNetCore.FailureConverters
{
    /// <summary>
    ///     Gives ability to convert thrown exceptions to coded failures
    /// </summary>
    public interface ICodedFailureConverter
    {
        /// <summary>
        ///     Converts <paramref name="ex"/> to coded failure.
        ///     If succeed returns successful result.
        /// </summary>
        /// <param name="ex">Exception to convert</param>
        /// <returns>Convert result</returns>
        ConvertResult Convert([NotNull] Exception ex);
    }
}