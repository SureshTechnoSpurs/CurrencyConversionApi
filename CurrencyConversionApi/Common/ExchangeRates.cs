using System.Diagnostics.CodeAnalysis;

namespace CurrencyConversionApi.Common
{
    [ExcludeFromCodeCoverage]
    public class ExchangeRates
    {
        public decimal USD_TO_INR { get; set; }
        public decimal INR_TO_USD { get; set; }
        public decimal USD_TO_EUR { get; set; }
        public decimal EUR_TO_USD { get; set; }
        public decimal INR_TO_EUR { get; set; }
        public decimal EUR_TO_INR { get; set; }
    }
}
