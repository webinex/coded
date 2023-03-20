using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Webinex.Coded.AspNetCore.FailureConverters;

namespace Webinex.Coded.AspNetCore
{
    internal class CodedExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CodedExceptionMiddleware> _logger;

        public CodedExceptionMiddleware(RequestDelegate next, ILogger<CodedExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(
            HttpContext context,
            IExceptionConverterAggregator converter,
            IStatusCodeResolver statusCodeResolver,
            IOptions<JsonOptions> options)
        {
            await new ExceptionHandler(_next, context, _logger, converter, options, statusCodeResolver).InvokeAsync();
        }

        private class ExceptionHandler
        {
            private readonly IOptions<JsonOptions> _jsonOptions;
            private readonly HttpContext _context;
            private readonly ILogger _logger;
            private readonly IExceptionConverterAggregator _converter;
            private readonly IStatusCodeResolver _statusCodeResolver;
            private readonly RequestDelegate _next;

            public ExceptionHandler(
                RequestDelegate next,
                HttpContext context,
                ILogger logger,
                IExceptionConverterAggregator converter,
                IOptions<JsonOptions> jsonOptions,
                IStatusCodeResolver statusCodeResolver)
            {
                _jsonOptions = jsonOptions;
                _statusCodeResolver = statusCodeResolver;
                _converter = converter;
                _context = context;
                _logger = logger;
                _next = next;
            }

            public async Task InvokeAsync()
            {
                try
                {
                    await _next.Invoke(_context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Exception occured in request with trace identifier: {TraceIdentifier}",
                        _context.TraceIdentifier);

                    if (!TryHandle(ex))
                        throw;
                }
            }

            private bool TryHandle(Exception ex)
            {
                var result = _converter.Convert(ex);

                if (result.Succeed)
                {
                    SendResponse(result);
                }

                return result.Succeed;
            }

            private void SendResponse(ConvertResult result)
            {
                var content = Serialize(result);
                var httpCode = HttpStatusCode(result);

                _context.Response.Clear();
                _context.Response.Headers.Add(CodedFailureDefaults.HeaderName, content);
                _context.Response.StatusCode = httpCode;
            }

            private string Serialize(ConvertResult result)
            {
                var options = new JsonSerializerOptions(_jsonOptions.Value.SerializerOptions)
                {
                    Converters = { new CodeJsonConverter() },
                };

                return JsonSerializer.Serialize(result.Failure, options);
            }

            private int HttpStatusCode(ConvertResult result)
            {
                return (int)_statusCodeResolver.Of(result.Failure);
            }
        }
    }
}