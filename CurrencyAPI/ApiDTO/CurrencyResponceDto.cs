using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyAPI.ApiDTO
{
    public class CurrencyResponceDto
    {
        [JsonProperty("time")]
        public DateTime time { get; set; }

        [JsonProperty("amount")]
        public double amount { get; set; }
        
        public string numberInString { get; set; }
    }
}
