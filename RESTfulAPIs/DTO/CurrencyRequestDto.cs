using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPIs.DTO
{
    public class CurrencyRequestDto
    {
        [JsonProperty("base_currency")]
        public string from { get; set; }
        
        [JsonProperty("to")]
        public string to { get; set; }
       
        [JsonProperty("amount")]
        public int amount { get; set; }
        
        [JsonProperty("format")]
        public int format { get; set; }
        
    }
}
