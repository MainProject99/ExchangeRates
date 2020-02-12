using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyAPI.ApiDTO
{
    public class RatesResponseDTO
    {
        [JsonProperty("time")]
        public DateTime time { get; set; }

        [JsonProperty("base")]
        public string baseCurrency { get; set; }
        [JsonProperty("rates")]
        public IDictionary<string, long> rates { get; set; }
    }
}