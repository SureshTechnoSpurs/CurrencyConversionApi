using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyConversionApi.Responses
{
    [ExcludeFromCodeCoverage]
    public class CurrenyConversionResponse
    {
        [JsonProperty("exchangeRate")]
        public decimal ExchangeRate { get; set; }

        [JsonProperty("convertedAmount")]
        public decimal ConvertedAmount { get; set; }

        public CurrenyConversionResponse(decimal exchangeRate, decimal convertedAmount)
        {
            ExchangeRate = exchangeRate;
            ConvertedAmount = convertedAmount;
        }

        public CurrenyConversionResponse() { }
    }
}
