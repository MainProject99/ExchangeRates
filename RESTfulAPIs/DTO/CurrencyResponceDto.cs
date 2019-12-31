using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPIs.DTO
{
    public class CurrencyResponceDto
    {
        [JsonProperty("time")]
        public string time { get; set; }

        [JsonProperty("timestamp")]
        public int timestamp { get; set; }

        [JsonProperty("from")]
        public string from { get; set; }

        [JsonProperty("to")]
        public string to { get; set; }

        [JsonProperty("amount")]
        public int amount { get; set; }

        [JsonProperty("rate")]
        public float rate { get; set; }

        [JsonProperty("convert_result")]
        public float convert_result { get; set; }
        
    }
}
