namespace Webinex.Coded
{
    /// <summary>
    ///     Represents failure data model with typed payload
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public class CodedFailure<TPayload> : CodedFailureBase
    {
        /// <summary>
        ///     Failure payload. Optional.
        /// </summary>
        public new TPayload Payload { get; }

        /// <summary>
        ///     Creates new instance of CodedFailure
        /// </summary>
        /// <param name="code">Failure code. Required.</param>
        /// <param name="payload">Failure payload. Optional.</param>
        /// <param name="defaultMessage">Default message. Optional.</param>
        public CodedFailure(Code code, TPayload payload = default, string defaultMessage = null) : base(code, payload,
            defaultMessage)
        {
            Payload = payload;
        }

        public CodedResult ToResult()
        {
            return new CodedResult(this, default);
        }

        public CodedResult<TResultPayload, TPayload> ToResult<TResultPayload>()
        {
            return new CodedResult<TResultPayload, TPayload>(this, default);
        }

        public new CodedException<TPayload> Throw()
        {
            throw new CodedException<TPayload>(this);
        }
    }
}