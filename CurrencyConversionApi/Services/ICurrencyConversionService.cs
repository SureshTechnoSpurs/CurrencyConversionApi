using CurrencyConversionApi.Request;
using CurrencyConversionApi.Responses;

namespace CurrencyConversionApi.Services
{
    public interface ICurrencyConversionService
    {
        /// <summary>
        /// This method is used to get the converted currency based on the input.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CurrenyConversionResponse GetCurrenyConversionAsync(CurrenyConversionRequest request);
    }
}
