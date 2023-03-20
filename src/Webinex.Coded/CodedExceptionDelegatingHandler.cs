using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Webinex.Coded
{
    public class CodedExceptionDelegatingHandler : DelegatingHandler
    {
        private readonly CodedExceptionDelegatingHandlerConfiguration _configuration;

        public CodedExceptionDelegatingHandler(CodedExceptionDelegatingHandlerConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
                return response;

            if (!response.Headers.Contains(_configuration.HttpHeader))
                return response;

            var failure = Deserialize(response.Headers.GetValues(_configuration.HttpHeader).SingleOrDefault());
            throw failure.Throw();
        }

        private CodedFailure Deserialize(string value)
        {
            var failure = JsonConvert.DeserializeObject<CodedFailureDto>(value).ToFailure();

            if (failure.Code.IsOrChildOf(Code.AGGREGATED))
                failure = JsonConvert.DeserializeObject<AggregatedCodedFailureDto>(value).ToFailure();

            return failure;
        }

        private class CodedFailureDto
        {
            public string Code { get; set; }
            public string DefaultMessage { get; set; }
            public object Payload { get; set; }
            public CodedFailure ToFailure() => new CodedFailure(Code, Payload, DefaultMessage);
        }

        private class AggregatedCodedFailureDto
        {
            public string Code { get; set; }
            public string DefaultMessage { get; set; }
            public CodedFailureDto[] Payload { get; set; }
            public CodedFailure ToFailure() => new CodedFailure(Code, Payload, DefaultMessage);
        }
    }
}