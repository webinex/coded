namespace Webinex.Coded
{
    public class CodedResult<TPayload, TFailurePayload> : CodedResult<TPayload>
    {
        public new CodedFailure<TFailurePayload> Failure { get; }

        public CodedResult(CodedFailure<TFailurePayload> failure, TPayload payload) : base(failure, payload)
        {
            Failure = failure;
        }
    }
}