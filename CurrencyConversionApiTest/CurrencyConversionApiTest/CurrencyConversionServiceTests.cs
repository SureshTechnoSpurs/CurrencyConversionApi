using Castle.Core.Logging;
using CurrencyConversionApi.Request;
using CurrencyConversionApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CurrencyConversionApiTest
{
    public class CurrencyConversionServiceTests
    {
        private readonly ICurrencyConversionService _currencyConversionServiceTest;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<CurrencyConversionService>> _mockLogger;

        public CurrencyConversionServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<CurrencyConversionService>>();
            _currencyConversionServiceTest = new CurrencyConversionService(_mockConfiguration.Object, _mockLogger.Object);
        }

        [Fact]
        public void Convert_INR_to_USD_Success()
        {
            // Arrange
            var request = new CurrenyConversionRequest("INR", "USD", 100);

            // Act
            var result = _currencyConversionServiceTest.GetCurrenyConversionAsync(request);

            // Assert
            Assert.Equal(0.013M, result.ExchangeRate);
            Assert.Equal(1.30M, decimal.Round(result.ConvertedAmount, 2));
        }

        [Fact]
        public void Convert_USD_to_INR_Success()
        {
            // Arrange
            var request = new CurrenyConversionRequest("USD", "INR", 10);

            // Act
            var result = _currencyConversionServiceTest.GetCurrenyConversionAsync(request);

            // Assert
            Assert.Equal(74.00M, result.ExchangeRate);
            Assert.Equal(740.00M, result.ConvertedAmount);
        }

        [Fact]
        public void Convert_INR_to_EUR_Success()
        {
            // Arrange
            var request = new CurrenyConversionRequest("INR", "EUR", 500);

            // Act
            var result = _currencyConversionServiceTest.GetCurrenyConversionAsync(request);

            // Assert
            Assert.Equal(0.011M, result.ExchangeRate);
            Assert.Equal(5.50M, decimal.Round(result.ConvertedAmount, 2));
        }

        [Fact]
        public void Convert_Invalid_CurrencyPair()
        {
            // Arrange
            decimal amountUSD = 10;
            // Arrange
            var request = new CurrenyConversionRequest("USD", "GBP", 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _currencyConversionServiceTest.GetCurrenyConversionAsync(request));
        }
    }
}