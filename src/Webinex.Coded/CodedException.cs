using System;
using System.Collections.Generic;

namespace Webinex.Coded
{
    /// <summary>
    ///     Represents coded exception
    /// </summary>
    public class CodedException : Exception
    {
        /// <summary>
        ///     Creates new instance of coded exception
        /// </summary>
        /// <param name="code">Failure code. Required.</param>
        /// <param name="payload">Failure payload. Optional.</param>
        /// <param name="defaultMessage">Default message. Optional.</param>
        public CodedException(string code, object payload = null, string defaultMessage = null)
            : this(new CodedFailure(code ?? throw new ArgumentNullException(nameof(code)), payload, defaultMessage))
        {
        }

        /// <summary>
        ///     Creates new instance of coded exception
        /// </summary>
        /// <param name="failure">Coded exception failure. Required</param>
        public CodedException(CodedFailureBase failure)
        {
            Failure = failure ?? throw new ArgumentNullException(nameof(failure));
        }

        /// <summary>
        ///     Coded failure. Not null.
        /// </summary>
        public CodedFailureBase Failure { get; }

        public override string ToString()
        {
            return $"Coded exception:{Environment.NewLine}{Failure}{Environment.NewLine}{base.ToString()}";
        }

        public static CodedException Invalid(object payload = null) =>
            CodedFailure.Invalid(payload).Throw();

        public static CodedException NotFound(object payload = null) =>
            CodedFailure.NotFound(payload).Throw();

        public static CodedException Unauthorized(object payload = null) =>
            CodedFailure.Unauthorized(payload).Throw();

        public static CodedException Forbidden(object payload = null) =>
            CodedFailure.Forbidden(payload).Throw();

        public static CodedException Conflict(object payload = null) =>
            CodedFailure.Conflict(payload).Throw();

        public static CodedException Locked(object payload = null) =>
            CodedFailure.Locked(payload).Throw();

        public static CodedException Unexpected(object payload = null) =>
            CodedFailure.Unexpected(payload).Throw();

        public static CodedException Aggregated(IEnumerable<CodedFailure> failures) =>
            CodedFailure.Aggregated(failures).Throw();

        public static CodedException<TPayload> Invalid<TPayload>(TPayload payload) =>
            CodedFailure.Invalid(payload).Throw();

        public static CodedException<TPayload> NotFound<TPayload>(TPayload payload) =>
            CodedFailure.NotFound(payload).Throw();

        public static CodedException<TPayload> Unauthorized<TPayload>(TPayload payload) =>
            CodedFailure.Unauthorized(payload).Throw();

        public static CodedException<TPayload> Forbidden<TPayload>(TPayload payload) =>
            CodedFailure.Forbidden(payload).Throw();

        public static CodedException<TPayload> Conflict<TPayload>(TPayload payload) =>
            CodedFailure.Conflict(payload).Throw();

        public static CodedException<TPayload> Locked<TPayload>(TPayload payload) =>
            CodedFailure.Locked(payload).Throw();

        public static CodedException<TPayload> Unexpected<TPayload>(TPayload payload) =>
            CodedFailure.Unexpected(payload).Throw();
    }
}