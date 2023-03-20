using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Coded.AspNetCore
{
    public static class CodedFailuresServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds coded failures services
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCodedFailures(
            [NotNull] this IServiceCollection services)
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            return AddCodedFailures(services, _ => { });
        }

        /// <summary>
        ///     Adds coded failures services with ability to configure
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configure">Configuration delegate</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCodedFailures(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<ICodedFailuresConfiguration> configure)
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            configure = configure ?? throw new ArgumentNullException(nameof(configure));

            var configuration = CodedFailuresConfiguration.GetOrCreate(services);
            configure(configuration);

            return services;
        }
    }
}