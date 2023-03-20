using System;

namespace Webinex.Coded
{
    public class CodedException<TPayload> : CodedException
    {
        public CodedException(string code, TPayload payload, string defaultMessage = null) : base(
            new CodedFailure<TPayload>(code ?? throw new ArgumentNullException(nameof(code)),
                payload ?? throw new ArgumentNullException(nameof(payload)), defaultMessage))
        {
        }

        public CodedException(CodedFailure<TPayload> failure)
            : base(failure ?? throw new ArgumentException(nameof(failure)))
        {
        }

        public new CodedFailure<TPayload> Failure => (CodedFailure<TPayload>)base.Failure;
    }
}