using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyAPI.ApiDTO
{
    public class CurrencyRequestDto
    {
 
        
        [Required]
        [JsonProperty("from")]
        public string from { get; set; }        
        [Required]
        [JsonProperty("to")]
        public string to { get; set; }
        [Required(ErrorMessage = "Please provide integer value.")]
        
        [JsonProperty("amount")]
        public int amount { get; set; }
        public NumberToLanguageEnum numberToLanguage { get; set; }


    }
}
