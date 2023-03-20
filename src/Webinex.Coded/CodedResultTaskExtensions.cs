using System;
using System.Threading.Tasks;

namespace Webinex.Coded
{
    public static class CodedResultTaskExtensions
    {
        public static async Task<CodedResult> ThrowAsync(this Task<CodedResult> task)
        {
            task = task ?? throw new ArgumentNullException(nameof(task));

            var result = await task;
            result.Throw();

            return result;
        }

        public static async Task<CodedResult<TPayload>> ThrowAsync<TPayload>(this Task<CodedResult<TPayload>> task)
        {
            task = task ?? throw new ArgumentNullException(nameof(task));

            var result = await task;
            result.Throw();

            return result;
        }

        public static async Task<TPayload> PayloadAsync<TPayload>(this Task<CodedResult<TPayload>> task)
        {
            task = task ?? throw new ArgumentNullException(nameof(task));

            var result = await task;
            result.Throw();

            return result.Payload;
        }
    }
}