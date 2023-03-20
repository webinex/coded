using System;
using System.Collections.Generic;

namespace Webinex.Coded
{
    public class CodedResult
    {
        private readonly object _payload;

        public CodedResult(CodedFailureBase failure, object payload = null)
        {
            Failure = failure;
            _payload = payload;
        }

        public CodedResult(object payload)
            : this(null, payload)
        {
        }

        public CodedFailureBase Failure { get; }

        public object Payload
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

        public bool Succeed => Failure == null;

        public CodedResult<T> Cast<T>()
        {
            if (Succeed)
            {
                throw new InvalidOperationException(
                    $"{nameof(CodedResult)} {nameof(Cast)} available only for failed {nameof(CodedResult)}");
            }

            return new CodedResult<T>(Failure, default);
        }
        
        public static ThisCodedResult This { get; } = new ThisCodedResult();

        public static CodedResult Invalid(object payload = null) => CodedFailure.Invalid(payload).ToResult();
        public static CodedResult NotFound(object payload = null) => CodedFailure.NotFound(payload).ToResult();
        public static CodedResult Unauthorized(object payload = null) => CodedFailure.Unauthorized(payload).ToResult();
        public static CodedResult Forbidden(object payload = null) => CodedFailure.Forbidden(payload).ToResult();
        public static CodedResult Conflict(object payload = null) => CodedFailure.Conflict(payload).ToResult();
        public static CodedResult Locked(object payload = null) => CodedFailure.Locked(payload).ToResult();
        public static CodedResult Unexpected(object payload = null) => CodedFailure.Unexpected(payload).ToResult();

        public static CodedResult Aggregated(IEnumerable<CodedFailure> failures) =>
            CodedFailure.Aggregated(failures).ToResult();

        public static CodedResult<TResult> Invalid<TResult>(object payload = null) =>
            CodedFailure.Invalid(payload).ToResult<TResult>();

        public static CodedResult<TResult> NotFound<TResult>(object payload = null) =>
            CodedFailure.NotFound(payload).ToResult<TResult>();

        public static CodedResult<TResult> Unauthorized<TResult>(object payload = null) =>
            CodedFailure.Unauthorized(payload).ToResult<TResult>();

        public static CodedResult<TResult> Forbidden<TResult>(object payload = null) =>
            CodedFailure.Forbidden(payload).ToResult<TResult>();

        public static CodedResult<TResult> Conflict<TResult>(object payload = null) =>
            CodedFailure.Conflict(payload).ToResult<TResult>();

        public static CodedResult<TResult> Locked<TResult>(object payload = null) =>
            CodedFailure.Locked(payload).ToResult<TResult>();

        public static CodedResult<TResult> Unexpected<TResult>(object payload = null) =>
            CodedFailure.Unexpected(payload).ToResult<TResult>();

        public static CodedResult<TResult> Aggregated<TResult>(IEnumerable<CodedFailure> failures) =>
            CodedFailure.Aggregated(failures).ToResult<TResult>();
    }
}