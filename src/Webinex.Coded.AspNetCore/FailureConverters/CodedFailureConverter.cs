using System;

namespace Webinex.Coded.AspNetCore.FailureConverters
{
    /// <summary>
    ///     Provides ability to convert thrown exception to coded failures.
    /// </summary>
    /// <typeparam name="T">Type of exception</typeparam>
    public abstract class CodedFailureConverter<T> : ICodedFailureConverter
        where T : Exception
    {
        public ConvertResult Convert(Exception ex)
        {
            ex = ex ?? throw new ArgumentNullException(nameof(ex));

            if (ex.GetType() != typeof(T))
                return ConvertResult.Nope();

            return ConvertException((T)ex);
        }

        protected abstract ConvertResult ConvertException(T ex);
    }
}