using System;

namespace Webinex.Coded
{
    /// <summary>
    ///     Represents failure codes.
    ///     Codes inherited. When you create code "INV.EMAIL" http status code would infer from it's parent (INV).
    ///
    ///     You can override it by adding "INV.EMAIL" custom http status mapping.
    /// </summary>
    public class Code
    {
        public Code(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }

        /// <summary>
        ///     Represents invalid state or request model
        /// </summary>
        public static readonly Code INVALID = "INV";

        /// <summary>
        ///     Represents not found entity failures
        /// </summary>
        public static readonly Code NOT_FOUND = "NTFND";

        /// <summary>
        ///     Represents user or tenant authorization failures
        /// </summary>
        public static readonly Code UNAUTHORIZED = "UNAUTH";

        /// <summary>
        ///     Represents request to forbidden resource
        /// </summary>
        public static readonly Code FORBIDDEN = "FRBD";

        /// <summary>
        ///     Represents conflicted modifications
        /// </summary>
        public static readonly Code CONFLICT = "CNFLT";

        /// <summary>
        ///     Represents failures for modification or reads on locked resources
        /// </summary>
        public static readonly Code LOCKED = "LCKD";

        /// <summary>
        ///     Represents unexpected failures. You might not use it directly
        /// </summary>
        public static readonly Code UNEXPECTED = "UNEXP";

        /// <summary>
        ///     Represents aggregated failure. Payload might be failures in this case.
        /// </summary>
        public static readonly Code AGGREGATED = "AGGR";

        public static bool operator ==(Code left, Code right)
        {
            return EqualOperator(left, right);
        }

        public static bool operator !=(Code left, Code right)
        {
            return NotEqualOperator(left, right);
        }

        public static implicit operator string(Code code)
        {
            return code?.Value;
        }

        public static implicit operator Code(string value)
        {
            return value == null ? null : new Code(value);
        }

        public CodedFailure ToFailure(object payload = null, string defaultMessage = null)
        {
            return new CodedFailure(this, payload, defaultMessage);
        }

        public CodedFailure<TPayload> ToFailure<TPayload>(
            TPayload payload,
            string defaultMessage = null)
        {
            return new CodedFailure<TPayload>(this, payload, defaultMessage);
        }

        public CodedResult Failed(Code code, object payload = null, string defaultMessage = null)
        {
            return new CodedResult(new CodedFailure(code, payload, defaultMessage), null);
        }

        public CodedResult<TValue> Failed<TValue>(
            object payload = null,
            string defaultMessage = null)
        {
            return new CodedResult<TValue>(new CodedFailure(this, payload, defaultMessage), default);
        }

        public CodedResult<TValue, TFailurePayload> Failed<TValue, TFailurePayload>(
            TFailurePayload payload = default,
            string defaultMessage = null)
        {
            return new CodedResult<TValue, TFailurePayload>(
                new CodedFailure<TFailurePayload>(this, payload, defaultMessage), default);
        }

        protected static bool EqualOperator(Code left, Code right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }

            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(Code left, Code right)
        {
            return !EqualOperator(left, right);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var typedObj = (Code)obj;
            return Value == typedObj.Value;
        }

        public bool IsChildOf(Code code)
        {
            if (code == null)
                return false;

            return Value.StartsWith($"{code.Value}.");
        }

        public bool IsOrChildOf(Code code)
        {
            if (code == null)
                return false;

            return this == code || IsChildOf(code);
        }

        public Code Child(string subCode)
        {
            subCode = subCode ?? throw new ArgumentNullException(nameof(subCode));
            return new Code($"{Value}.{subCode}");
        }

        public override string ToString()
        {
            return Value;
        }
    }
}