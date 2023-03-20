using System;

namespace Webinex.Coded
{
    public static class CodedResults
    {
        public static CodedResult Success()
        {
            return new CodedResult(null, null);
        }

        public static CodedResult<TPayload> Success<TPayload>(TPayload payload)
        {
            return new CodedResult<TPayload>(null, payload);
        }

        public static CodedResult Failed(CodedFailure failure)
        {
            failure = failure ?? throw new ArgumentNullException(nameof(failure));
            return new CodedResult(failure, null);
        }

        public static CodedResult Failed(string code, object payload = null, string defaultMessage = null)
        {
            code = code ?? throw new ArgumentNullException(nameof(code));
            return Failed(new CodedFailure(code, payload, defaultMessage));
        }
    }
}