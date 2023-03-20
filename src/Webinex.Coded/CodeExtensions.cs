using System;

namespace Webinex.Coded
{
    public static class CodeExtensions
    {
        public static CodedFailure AsFailure(this Code code, object payload = null, string defaultMessage = null)
        {
            code = code ?? throw new ArgumentNullException(nameof(code));
            return new CodedFailure(code, payload, defaultMessage);
        }

        public static CodedFailure<TPayload> AsFailure<TPayload>(
            this Code code,
            TPayload payload,
            string defaultMessage = null)
        {
            code = code ?? throw new ArgumentNullException(nameof(code));
            return new CodedFailure<TPayload>(code, payload, defaultMessage);
        }

        public static CodedResult Failed(this Code code, object payload = null, string defaultMessage = null)
        {
            code = code ?? throw new ArgumentNullException(nameof(code));
            return new CodedResult(new CodedFailure(code, payload, defaultMessage), null);
        }

        public static CodedResult<TValue> Failed<TValue>(
            this Code code,
            object payload = null,
            string defaultMessage = null)
        {
            code = code ?? throw new ArgumentNullException(nameof(code));
            return new CodedResult<TValue>(new CodedFailure(code, payload, defaultMessage), default);
        }

        public static CodedResult<TValue, TFailurePayload> Failed<TValue, TFailurePayload>(
            this Code code,
            TFailurePayload payload = default,
            string defaultMessage = null)
        {
            code = code ?? throw new ArgumentNullException(nameof(code));
            return new CodedResult<TValue, TFailurePayload>(
                new CodedFailure<TFailurePayload>(code, payload, defaultMessage), default);
        }
    }
}