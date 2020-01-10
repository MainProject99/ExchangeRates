using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class CurrencyRequestDto
    {
        [JsonProperty("api_key")]
        public string api_key { get; set; }
        [JsonProperty("from")]
        public string from { get; set; }
        
        [JsonProperty("to")]
        public string to { get; set; }
       
        [JsonProperty("amount")]
        public int amount { get; set; }
        
        [JsonProperty("format")]
        public string format { get; set; }

        public NumberToLanguageEnum numberToLanguage { get; set; }


    }
}
