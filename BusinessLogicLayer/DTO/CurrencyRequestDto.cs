using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class CurrencyRequestDto
    {
        [JsonProperty("base_currency")]
        public string from { get; set; }
        
        [JsonProperty("to")]
        public string to { get; set; }
       
        [JsonProperty("amount")]
        public string amount { get; set; }
        
        [JsonProperty("format")]
        public string format { get; set; }
        
    }
}
