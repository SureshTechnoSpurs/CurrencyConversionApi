using CurrencyConversionApi.Common;
using CurrencyConversionApi.Dto;
using CurrencyConversionApi.Request;
using CurrencyConversionApi.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConversionApi.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private Dictionary<string, decimal> _rates;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CurrencyConversionService> _logger;

        public CurrencyConversionService(IConfiguration configuration, ILogger<CurrencyConversionService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get the converted currency based on the input
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CurrenyConversionResponse GetCurrenyConversionAsync(CurrenyConversionRequest request)
        {
            var response = new CurrenyConversionResponse();

            ValidateInput(request.SourceCurrency, request.TargetCurrency, request.Amount);

            var sourceCurrency = request.SourceCurrency.ToUpper();
            var targetCurrency = request.TargetCurrency.ToUpper();
            var amount = request.Amount;

            _logger.LogInformation($"Validate Currency for the source {sourceCurrency} and target {targetCurrency}");

            var (isValid, errorMessage) = ValidateCurrency(sourceCurrency, targetCurrency);

            if (isValid)
            {
                LoadExchangeRates();
                OverrideWithEnvironmentVariables();

                var currencyConversionResult = Convert(amount, sourceCurrency, targetCurrency);

                response = new CurrenyConversionResponse(currencyConversionResult.ExchangeRate, currencyConversionResult.ConvertedAmount);
            }
            else
            {
                _logger.LogError($"Validate Currency for the source {sourceCurrency} and target {targetCurrency}");
                throw new ArgumentException(errorMessage);
            }
            return response;
        }

        private static void ValidateInput(string sourceCurrency, string targetCurrency, decimal amount)
        {
            // Check if sourceCurrency and targetCurrency are not null or empty
            if (string.IsNullOrEmpty(sourceCurrency) || string.IsNullOrEmpty(targetCurrency))
            {
                throw new ArgumentException("SourceCurrency and TargetCurrency cannot be null or empty.");
            }

            // Check if amount is greater than 0
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0.");
            }
        }

        public CurrencyConversionDto Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            string key = $"{fromCurrency.ToUpper()}_TO_{toCurrency.ToUpper()}";

            _logger.LogInformation($"Form the currenry key {key}");

            if (_rates.ContainsKey(key))
            {
                decimal rate = _rates[key];
                decimal convertedAmount = amount * rate;

                _logger.LogInformation($"Return the ExchangeRate {rate} AND ConvertedAmount {convertedAmount}");
                return new CurrencyConversionDto
                {
                    ExchangeRate = rate,
                    ConvertedAmount = convertedAmount
                };
            }
            else
            {
                _logger.LogError($"Currency pair : '{key}' is invalid");
                throw new ArgumentException($"Currency pair : '{key}' is invalid");
            }
        }

        public (bool isValid, string errorMessage) ValidateCurrency(string sourceCurrency, string targetCurrency)
        {
            bool sourceIsValid = Enum.TryParse(sourceCurrency.ToUpper(), out ExchangeRatesEnum _);
            bool targetIsValid = Enum.TryParse(targetCurrency.ToUpper(), out ExchangeRatesEnum _);

            if (sourceIsValid && targetIsValid)
            {
                return (true, null); // Both currencies are valid
            }
            else if (!sourceIsValid && !targetIsValid)
            {
                _logger.LogError($"Source currency code '{sourceCurrency}' and target currency code '{targetCurrency}' are invalid");
                return (false, $"Source currency code '{sourceCurrency}' and target currency code '{targetCurrency}' are invalid");
            }
            else if (!sourceIsValid)
            {
                _logger.LogError($"Source currency code '{sourceCurrency}' is invalid");
                return (false, $"Source currency code '{sourceCurrency}' is invalid");
            }
            else
            {
                _logger.LogError($"Target currency code '{targetCurrency}' is invalid");
                return (false, $"Target currency code '{targetCurrency}' is invalid");
            }
        }

        private void OverrideWithEnvironmentVariables()
        {
            string jsonFilePath = "exchangeRates.json";
            // Read exchange rates from JSON file
            _logger.LogInformation($"Read the json file {jsonFilePath}");
            string json = File.ReadAllText(jsonFilePath);

            // Parse the JSON string into a JObject
            JObject jsonObject = JObject.Parse(json);

            foreach (var key in _rates.Keys.ToList())
            {
                string envVar = _configuration[key.ToUpper()];
                if (!string.IsNullOrEmpty(envVar) && decimal.TryParse(envVar, out decimal rate))
                {
                    _rates[key] = rate;
                    _logger.LogInformation($"Update the value  {jsonFilePath} from environment variable");
                    jsonObject[key] = rate;
                    _logger.LogInformation($"Update the json file {jsonFilePath} with the key {key}");
                }
            }

            // Serialize the JObject back to a JSON string
            string updatedJson = jsonObject.ToString(Formatting.Indented);

            // Write the updated JSON string back to the file
            File.WriteAllText(jsonFilePath, updatedJson);
            _logger.LogInformation($"Update the json file {jsonFilePath} with the updated value");
        }

        private void LoadExchangeRates()
        {

            // Read exchange rates from JSON file
            string json = File.ReadAllText("exchangeRates.json");
            var exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(json);
            _logger.LogInformation($"Read the json file and deserialize the json object");
            // Store rates in a dictionary
            _rates = new Dictionary<string, decimal>
            {
                { "USD_TO_INR", exchangeRates.USD_TO_INR },
                { "INR_TO_USD", exchangeRates.INR_TO_USD },
                { "USD_TO_EUR", exchangeRates.USD_TO_EUR },
                { "EUR_TO_USD", exchangeRates.EUR_TO_USD },
                { "INR_TO_EUR", exchangeRates.INR_TO_EUR },
                { "EUR_TO_INR", exchangeRates.EUR_TO_INR }
            };

            _logger.LogInformation($"Successully loaded the json values");
        }

    }
}

