using System.Collections.Generic;

namespace Webinex.Coded
{
    /// <summary>
    ///     Represents failure data model
    /// </summary>
    public class CodedFailure : CodedFailureBase
    {
        /// <summary>
        ///     Creates new instance of CodeFailure
        /// </summary>
        /// <param name="code">Failure code. Required.</param>
        /// <param name="defaultMessage">Default message. Optional.</param>
        /// <param name="payload">Failure payload. Optional.</param>
        public CodedFailure(Code code, object payload = null, string defaultMessage = null)
            : base(code, payload, defaultMessage)
        {
        }

        public CodedResult ToResult()
        {
            return new CodedResult(this, default);
        }

        public CodedResult<TResultPayload> ToResult<TResultPayload>()
        {
            return new CodedResult<TResultPayload>(this, default);
        }

        public static ThisCodedFailure This { get; } = new ThisCodedFailure();

        public static CodedFailure Invalid(object payload = null) => new CodedFailure(Code.INVALID, payload, "Invalid operation state");
        public static CodedFailure NotFound(object payload = null) => new CodedFailure(Code.NOT_FOUND, payload, "Entity not found");
        public static CodedFailure Unauthorized(object payload = null) => new CodedFailure(Code.UNAUTHORIZED, payload, "Insufficient permissions");
        public static CodedFailure Forbidden(object payload = null) => new CodedFailure(Code.FORBIDDEN, payload, "Insufficient permissions");
        public static CodedFailure Conflict(object payload = null) => new CodedFailure(Code.CONFLICT, payload, "Concurrent modification");
        public static CodedFailure Locked(object payload = null) => new CodedFailure(Code.LOCKED, payload, "Resource in locked state");
        public static CodedFailure Unexpected(object payload = null) => new CodedFailure(Code.UNEXPECTED, payload, "Unexpected failure had happened");
        public static CodedFailure Aggregated(IEnumerable<CodedFailure> failures) => new CodedFailure(Code.AGGREGATED, failures, "Multiple failures occured");

        public static CodedFailure<TPayload> Invalid<TPayload>(TPayload payload) => new CodedFailure<TPayload>(Code.INVALID, payload, "Invalid operation state");
        public static CodedFailure<TPayload> NotFound<TPayload>(TPayload payload) => new CodedFailure<TPayload>(Code.NOT_FOUND, payload, "Entity not found");
        public static CodedFailure<TPayload> Unauthorized<TPayload>(TPayload payload) => new CodedFailure<TPayload>(Code.UNAUTHORIZED, payload, "Insufficient permissions");
        public static CodedFailure<TPayload> Forbidden<TPayload>(TPayload payload) => new CodedFailure<TPayload>(Code.FORBIDDEN, payload, "Insufficient permissions");
        public static CodedFailure<TPayload> Conflict<TPayload>(TPayload payload) => new CodedFailure<TPayload>(Code.CONFLICT, payload, "Concurrent modification");
        public static CodedFailure<TPayload> Locked<TPayload>(TPayload payload) => new CodedFailure<TPayload>(Code.LOCKED, payload, "Resource in locked state");
        public static CodedFailure<TPayload> Unexpected<TPayload>(TPayload payload) => new CodedFailure<TPayload>(Code.UNEXPECTED, payload, "Unexpected failure had happened");
    }
}