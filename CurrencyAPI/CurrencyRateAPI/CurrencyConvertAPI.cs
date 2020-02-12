using CurrencyAPI.ApiDTO;
using CurrencyAPI.Helpers;
using CurrencyAPI.Intefaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAPI.CurrencyRateAPI
{
    public class CurrencyConvertAPI : ICurrencyConvertAPI
    {
        static readonly HttpClient client = new HttpClient();
        private readonly CurrencySettings currencySettings;

        public CurrencyConvertAPI(IOptions<CurrencySettings> _currencySettings)
        {
            currencySettings = _currencySettings.Value ?? throw new ArgumentException(nameof(_currencySettings));

        }
        private const string URL = "https://currency.labstack.com/api/v1/";

        
        public async Task<RatesResponseDTO> GetCurrencyAPI()
        {
            client.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue("Bearer", currencySettings.ApiKey);
            var urlConverter = new Uri(URL + $"rates");

            using (HttpResponseMessage response = await client.GetAsync(urlConverter))
            {
                RatesResponseDTO ratesResponceDto = null;
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();

                    ratesResponceDto = JsonConvert.DeserializeObject<RatesResponseDTO>(mycontent);

                }
                return ratesResponceDto;
            }
        }
        public async Task<CurrencyResponceDto> ConvertCurrencies(CurrencyRequestDto currencyRequestDto)
        {
            client.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue("Bearer", currencySettings.ApiKey);

            var urlConverter = new Uri(URL + $"convert/{currencyRequestDto.amount}/{currencyRequestDto.from }/{currencyRequestDto.to}");

            using (HttpResponseMessage response = await client.GetAsync(urlConverter))
            {
                CurrencyResponceDto currencyResponceDto = null;
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();

                    currencyResponceDto = JsonConvert.DeserializeObject<CurrencyResponceDto>(mycontent);                   
                }
                return currencyResponceDto;
            }
        }
    }
}
