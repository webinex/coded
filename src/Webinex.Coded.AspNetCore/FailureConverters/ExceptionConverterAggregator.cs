using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Webinex.Coded.AspNetCore.FailureConverters
{
    internal interface IExceptionConverterAggregator
    {
        ConvertResult Convert(Exception ex);
    }

    internal class ExceptionConverterAggregator : IExceptionConverterAggregator
    {
        private readonly IEnumerable<ICodedFailureConverter> _converters;
        private readonly ICodedFailuresSettings _settings;
        private readonly ILogger<ExceptionConverterAggregator> _logger;

        public ExceptionConverterAggregator(
            IEnumerable<ICodedFailureConverter> converters,
            ICodedFailuresSettings settings,
            ILogger<ExceptionConverterAggregator> logger)
        {
            _settings = settings;
            _logger = logger;
            _converters = converters?.ToArray() ?? throw new ArgumentNullException(nameof(converters));
        }

        public ConvertResult Convert(Exception ex)
        {
            return _settings.ExceptionConverters
                       .Select(converterType => Convert(ex, converterType))
                       .FirstOrDefault(x => x.Succeed)
                   ?? ConvertResult.Nope();
        }

        private ConvertResult Convert(Exception ex, Type converterType)
        {
            var converter = GetConverter(converterType);
            return converter == null ? ConvertResult.Nope() : converter.Convert(ex);
        }

        private ICodedFailureConverter GetConverter(Type type)
        {
            var converter = _converters.FirstOrDefault(x => x.GetType() == type);
            if (converter == null)
                _logger.LogWarning($"Converter of type {type.Name} not found. Ensure it registered in DI container.");

            return converter;
        }
    }
}