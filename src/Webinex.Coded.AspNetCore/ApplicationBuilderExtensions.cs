using System;
using Microsoft.AspNetCore.Builder;

namespace Webinex.Coded.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds coded exception middleware to request pipeline.
        ///     It catches exceptions and sends response with coded exception header.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <returns><see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseCodedExceptions(
            this IApplicationBuilder app)
        {
            app = app ?? throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<CodedExceptionMiddleware>();
            return app;
        }
    }
}