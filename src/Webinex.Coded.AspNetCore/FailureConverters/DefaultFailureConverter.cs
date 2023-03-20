using System;

namespace Webinex.Coded.AspNetCore.FailureConverters
{
    internal class DefaultFailureConverter : ICodedFailureConverter
    {
        public ConvertResult Convert(Exception ex)
        {
            var converted = ConvertToCoded(ex);
            return converted != null ? ConvertResult.Success(converted) : ConvertResult.Nope();
        }

        private CodedFailureBase ConvertToCoded(Exception ex)
        {
            switch (ex)
            {
                case CodedException coded:
                    return coded.Failure;

                default:
                    return new CodedFailure(Code.UNEXPECTED);
            }
        }
    }
}