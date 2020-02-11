using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogicLayer.DTO
{
    public class RatesRequestDTO
    {
        [JsonProperty("base")]
        public string baseCurrency { get; set; }


             
    }
}
