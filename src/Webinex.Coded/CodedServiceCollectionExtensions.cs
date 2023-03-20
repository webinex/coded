using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Webinex.Coded
{
    public static class CodedServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddCodedExceptionHttpMessageHandler(
            this IHttpClientBuilder builder,
            Action<CodedExceptionDelegatingHandlerConfiguration> configure = null)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            var configuration = new CodedExceptionDelegatingHandlerConfiguration();
            configure?.Invoke(configuration);

            builder.Services.Configure<HttpClientFactoryOptions>(
                builder.Name,
                options =>
                {
                    options.HttpMessageHandlerBuilderActions.Add(b => b.AdditionalHandlers.Add(new CodedExceptionDelegatingHandler(configuration)));
                });

            return builder;
        }
    }
}