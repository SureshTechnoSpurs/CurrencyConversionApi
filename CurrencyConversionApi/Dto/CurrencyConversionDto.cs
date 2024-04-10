using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyConversionApi.Dto
{
    [ExcludeFromCodeCoverage]
    public class CurrencyConversionDto
    {
        public decimal ExchangeRate { get; set; }

        public decimal ConvertedAmount { get; set; }

        public CurrencyConversionDto()
        {

        }
    }
}
