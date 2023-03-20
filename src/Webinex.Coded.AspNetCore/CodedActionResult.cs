using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Webinex.Coded.AspNetCore
{
    public class CodedActionResult : ActionResult
    {
        public CodedActionResult([NotNull] CodedFailureBase failure)
        {
            failure = failure ?? throw new ArgumentNullException(nameof(failure));
            Result = new CodedResult(failure);
        }

        public CodedActionResult([NotNull] CodedResult result)
        {
            result = result ?? throw new ArgumentNullException(nameof(result));
            Result = result;
        }

        [NotNull] public CodedResult Result { get; }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            if (Result.Succeed)
            {
                await new OkObjectResult(Result.Payload).ExecuteResultAsync(context);
                return;
            }
            
            context.HttpContext.Response.StatusCode = StatusCode(context);
            context.HttpContext.Response.Headers.Add(CodedFailureDefaults.HeaderName, Content(context));
        }

        private int StatusCode(ActionContext context)
        {
            var resolver = context.HttpContext.RequestServices.GetRequiredService<IStatusCodeResolver>();
            return (int)resolver.Of(Result.Failure);
        }

        private string Content(ActionContext context)
        {
            var jsonOptions = JsonOptions(context).Value;
            var serializationOptions = new JsonSerializerOptions(jsonOptions.JsonSerializerOptions)
            {
                Converters = { new CodeJsonConverter() },
            };

            return JsonSerializer.Serialize(Result.Failure, serializationOptions);
        }

        private IOptions<JsonOptions> JsonOptions(ActionContext context)
        {
            return context.HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>();
        }
    }
}