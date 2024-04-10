using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyConversionApi.Request
{
    [ExcludeFromCodeCoverage]
    public class CurrenyConversionRequest
    {
        [JsonProperty("sourceCurrency")]
        public string SourceCurrency { get; set; }

        [JsonProperty("targetCurrency")]
        public string TargetCurrency { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        public CurrenyConversionRequest(string sourceCurrency, string targetCurrency, decimal amount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Amount = amount;
        }
    }
}
