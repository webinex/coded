using System;
using System.Linq;
using System.Net;

namespace Webinex.Coded.AspNetCore
{
    internal interface IStatusCodeResolver
    {
        HttpStatusCode Of(CodedFailureBase failure);
    }
    
    internal class StatusCodeResolver : IStatusCodeResolver
    {
        private readonly ICodedFailuresSettings _settings;

        public StatusCodeResolver(ICodedFailuresSettings settings)
        {
            _settings = settings;
        }

        public HttpStatusCode Of(CodedFailureBase failure)
        {
            failure = failure ?? throw new ArgumentNullException(nameof(failure));
            var code = failure.Code ?? throw new ArgumentException("Code might not be null", nameof(failure));
            return Of(code);
        }

        private HttpStatusCode Of(string code)
        {
            if (_settings.HttpCodeByFailureCode.TryGetValue(code, out var httpCode))
                return httpCode;

            var parentHttpCode = Parent(code);
            return parentHttpCode ?? HttpStatusCode.InternalServerError;
        }

        private HttpStatusCode? Parent(string code)
        {
            var parents = code
                .Split('.', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .SkipLast(1)
                .ToArray();

            for (int i = 0; i < parents.Length; i++)
            {
                var parentCode = string.Join(".", parents.SkipLast(i));
                
                if (_settings.HttpCodeByFailureCode.TryGetValue(parentCode, out var httpCode))
                    return httpCode;
            }

            return null;
        }
    }
}