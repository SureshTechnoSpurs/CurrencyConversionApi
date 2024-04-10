using CurrencyConversionApi.Request;
using CurrencyConversionApi.Responses;
using CurrencyConversionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConversionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConversionController : ControllerBase
    {

        private readonly ILogger<CurrencyConversionController> _logger;
        private readonly ICurrencyConversionService _currencyConversionService;
        public CurrencyConversionController(ILogger<CurrencyConversionController> logger,
             ICurrencyConversionService currencyConversionService)
        {
            _logger = logger;
            _currencyConversionService = currencyConversionService;
        }

        [HttpGet("convert")]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(CurrenyConversionResponse), 200)]
        public async Task<ObjectResult> GetCurrenyConversion(string sourceCurrency, string targetCurrency, decimal amount)
        {
            _logger.LogInformation($"Entering the GetCurrenyConversion method for Source Currency : {sourceCurrency}, Target Currency : {targetCurrency}, Amount :  {amount}");

            var request = new CurrenyConversionRequest(sourceCurrency, targetCurrency, amount);

            var result = _currencyConversionService.GetCurrenyConversionAsync(request);

            _logger.LogInformation($"Suceesfully copleted GetCurrenyConversion method for Source Currency : {sourceCurrency}, Target Currency : {targetCurrency}, Amount :  {amount}");

            return Ok(result);
        }

    }
}
