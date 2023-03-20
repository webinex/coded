using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Coded.AspNetCore.FailureConverters
{
    /// <summary>
    ///     Represents exception to coded exception convertion result
    /// </summary>
    public class ConvertResult
    {
        private ConvertResult(bool succeed, CodedFailureBase failure)
        {
            Succeed = succeed;
            Failure = failure;
        }

        public bool Succeed { get; }
        
        [MaybeNull]
        public CodedFailureBase Failure { get; }

        /// <summary>
        ///     Creates new instance of successfully converted exception
        /// </summary>
        /// <param name="failure">Coded exception</param>
        public static ConvertResult Success([NotNull] CodedFailureBase failure)
        {
            failure = failure ?? throw new ArgumentNullException(nameof(failure));
            return new ConvertResult(true, failure);
        }

        /// <summary>
        ///     Creates new instance of unsuccessful convert
        /// </summary>
        public static ConvertResult Nope()
        {
            return new ConvertResult(false, null);
        }
    }
}