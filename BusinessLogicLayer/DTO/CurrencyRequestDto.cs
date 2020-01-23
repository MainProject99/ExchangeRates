﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class CurrencyRequestDto
    {
 
        [JsonProperty("api_key")]
        public string api_key { get; set; }
        [Required]
        [JsonProperty("from")]
        public string from { get; set; }
        
        [Required]
        [JsonProperty("to")]
        public string to { get; set; }
        [Required(ErrorMessage = "You should write Integer number to convert")]
        [JsonProperty("amount")]
        public int amount { get; set; }
        [Required(ErrorMessage = "You should write json ")]
        [JsonProperty("format")]
        public string format { get; set; }

        public NumberToLanguageEnum numberToLanguage { get; set; }


    }
}
