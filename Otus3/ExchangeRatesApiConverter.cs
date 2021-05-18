using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Otus3.Abstractions;

namespace Otus3
{
    public class ExchangeRatesApiConverter: ICurrencyConverter
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly string _apiKey;

        public ExchangeRatesApiConverter(HttpClient httpClient, IMemoryCache memoryCache, string apiKey)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _apiKey = apiKey;
        }

        public ICurrencyAmount ConvertCurrency(ICurrencyAmount amount, string currencyCode)
        {
            if (amount.CurrencyCode == currencyCode)
            {
                return amount;
            }

            var rate = _memoryCache.GetOrCreateAsync(amount.CurrencyCode, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                return GetCurrencyRateAsync(amount.CurrencyCode, currencyCode);
            }).Result;

            var result = new CurrencyAmount
            {
                Amount = amount.Amount * rate,
                CurrencyCode = currencyCode
            };

            return result;
        }

        private async Task<decimal> GetCurrencyRateAsync(string baseCurrencyCode, string targetCurrencyCode)
        {
            var msg = await _httpClient.GetAsync($"http://api.exchangeratesapi.io/v1/latest?access_key={_apiKey}&base={baseCurrencyCode}");
            var response = await msg.Content.ReadAsStringAsync();
            var exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesApiResponse>(response);
            if (exchangeRates == null)
            {
                throw new ArgumentNullException(nameof(exchangeRates));
            }

            return exchangeRates.Rates[targetCurrencyCode];
        }

        public class ExchangeRatesApiResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }

            [JsonProperty("base")]
            public string Base { get; set; }

            [JsonProperty("date")]
            public DateTimeOffset Date { get; set; }

            [JsonProperty("rates")]
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }
}
