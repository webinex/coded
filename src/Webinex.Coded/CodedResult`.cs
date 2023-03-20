namespace Webinex.Coded
{
    public class CodedResult<TPayload> : CodedResult
    {
        private readonly TPayload _payload;

        public new TPayload Payload
        {
            get
            {
                if (!Succeed)
                {
                    throw Failure.Throw();
                }

                return _payload;
            }
        }

        public CodedResult(CodedFailureBase failure, TPayload payload) : base(failure, payload)
        {
            _payload = payload;
        }
    }
}