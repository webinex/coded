using System;

namespace Webinex.Coded
{
    public class CodedFailureBase
    {
        /// <summary>
        ///     Creates new instance of CodeFailure
        /// </summary>
        /// <param name="code">Failure code. Required.</param>
        /// <param name="defaultMessage">Default message. Optional.</param>
        /// <param name="payload">Failure payload. Optional.</param>
        public CodedFailureBase(Code code, object payload = null, string defaultMessage = null)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Payload = payload;
            DefaultMessage = defaultMessage;
        }

        /// <summary>
        ///     Failure code. Not null.
        /// </summary>
        public Code Code { get; }
        
        /// <summary>
        ///     Default failure description. Optional.
        /// </summary>
        public string DefaultMessage { get; }
        
        /// <summary>
        ///     Failure payload. Optional.
        /// </summary>
        public object Payload { get; }

        public override string ToString()
        {
            return $"Coded Failure: {Code} - {DefaultMessage}";
        }

        public CodedException Throw()
        {
            throw new CodedException(this);
        }
    }
}