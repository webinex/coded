using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Webinex.Coded.AspNetCore.FailureConverters;

namespace Webinex.Coded.AspNetCore
{
    internal interface ICodedFailuresSettings
    {
        LinkedList<Type> ExceptionConverters { get; }
        IDictionary<string, HttpStatusCode> HttpCodeByFailureCode { get; }
    }
    
    public interface ICodedFailuresConfiguration
    {
        /// <summary>
        ///     <see cref="IServiceCollection"/>, for extensions
        /// </summary>
        IServiceCollection Services { get; }
        
        /// <summary>
        ///     Registered exception converters. Implementation guarantee that it would be executed in same order.
        /// </summary>
        LinkedList<Type> ExceptionConverters { get; }
        
        /// <summary>
        ///     Failure code to http code mappings.
        ///     They are inheritable.
        ///
        ///     For example,
        ///     we have "INV" = HTTP 400,
        ///     "INV.EMAIL" would return HTTP 400 if "INV.EMAIL" code not provided.
        ///
        ///     If "INV.EMAIL" = HTTP 403
        ///     than "INV.EMAIL.WRNG" = HTTP 403
        /// </summary>
        IDictionary<string, HttpStatusCode> HttpCodeByFailureCode { get; }
    }
    
    internal class CodedFailuresConfiguration : ICodedFailuresConfiguration, ICodedFailuresSettings
    {
        public CodedFailuresConfiguration(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));

            ExceptionConverters = new LinkedList<Type>();
            ExceptionConverters.AddFirst(typeof(DefaultFailureConverter));

            services.AddSingleton<ICodedFailureConverter, DefaultFailureConverter>();
            services.AddTransient<IExceptionConverterAggregator, ExceptionConverterAggregator>();
            services.AddSingleton<ICodedFailuresSettings>(this);
            services.AddSingleton<IStatusCodeResolver, StatusCodeResolver>();
            services.AddSingleton(this);
        }

        internal static CodedFailuresConfiguration GetOrCreate(IServiceCollection services)
        {
            var found = services.FirstOrDefault(x =>
                x.ImplementationInstance?.GetType() == typeof(CodedFailuresConfiguration))?.ImplementationInstance;

            return found != null
                ? (CodedFailuresConfiguration)found
                : new CodedFailuresConfiguration(services);
        }

        public IServiceCollection Services { get; }
        public LinkedList<Type> ExceptionConverters { get; }

        public IDictionary<string, HttpStatusCode> HttpCodeByFailureCode { get; } = new Dictionary<string, HttpStatusCode>
        {
            [Code.LOCKED] = HttpStatusCode.Locked,
            [Code.INVALID] = HttpStatusCode.BadRequest,
            [Code.CONFLICT] = HttpStatusCode.Conflict,
            [Code.NOT_FOUND] = HttpStatusCode.NotFound,
            [Code.FORBIDDEN] = HttpStatusCode.Forbidden,
            [Code.UNAUTHORIZED] = HttpStatusCode.Unauthorized,
            [Code.UNEXPECTED] = HttpStatusCode.InternalServerError,
        };
    }

    public static class CodedFailuresConfigurationExtensions
    {
        /// <summary>
        ///     Adds converter before all registered
        /// </summary>
        /// <param name="configuration"><see cref="ICodedFailuresConfiguration"/></param>
        /// <param name="lifetime">If null - service would not be registered and might be manually registered.</param>
        /// <typeparam name="T">Type of converter</typeparam>
        /// <returns><see cref="ICodedFailuresConfiguration"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICodedFailuresConfiguration AddFirst<T>(
            [NotNull] this ICodedFailuresConfiguration configuration,
            ServiceLifetime? lifetime = ServiceLifetime.Transient)
            where T : ICodedFailureConverter
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            configuration.ExceptionConverters.AddFirst(typeof(T));
            
            if (lifetime != null)
                configuration.Services.Add(new ServiceDescriptor(typeof(ICodedFailureConverter), typeof(T), lifetime.Value));

            return configuration;
        }
        
        /// <summary>
        ///     Adds converter after all registered
        /// </summary>
        /// <param name="configuration"><see cref="ICodedFailuresConfiguration"/></param>
        /// <param name="lifetime">If null - service would not be registered and might be manually registered.</param>
        /// <typeparam name="T">Type of converter</typeparam>
        /// <returns><see cref="ICodedFailuresConfiguration"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICodedFailuresConfiguration AddLast<T>(
            [NotNull] this ICodedFailuresConfiguration configuration,
            ServiceLifetime? lifetime = ServiceLifetime.Transient)
            where T : ICodedFailureConverter
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            configuration.ExceptionConverters.AddLast(typeof(T));
            
            if (lifetime != null)
                configuration.Services.Add(new ServiceDescriptor(typeof(ICodedFailureConverter), typeof(T), lifetime.Value));

            return configuration;
        }
        
        /// <summary>
        ///     Adds failure code to http code mapping.
        ///     They are inheritable.
        ///
        ///     For example,
        ///     we have "INV" = HTTP 400,
        ///     "INV.EMAIL" would return HTTP 400 if "INV.EMAIL" code not provided.
        ///
        ///     If "INV.EMAIL" = HTTP 403
        ///     than "INV.EMAIL.WRNG" = HTTP 403
        /// </summary>
        /// <param name="configuration"><see cref="ICodedFailuresConfiguration"/></param>
        /// <param name="code">Failure code.</param>
        /// <param name="httpCode">Http status code.</param>
        /// <returns><see cref="ICodedFailuresConfiguration"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICodedFailuresConfiguration AddCode(
            [NotNull] this ICodedFailuresConfiguration configuration,
            [NotNull] string code,
            HttpStatusCode httpCode)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            code = code ?? throw new ArgumentNullException(nameof(code));

            configuration.HttpCodeByFailureCode[code] = httpCode;
            
            return configuration;
        }
    }
}