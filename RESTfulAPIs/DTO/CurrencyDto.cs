using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPIs.DTO
{
    public class CurrencyDto
    {

        public string base_currency { get; set; }

        public string time { get; set; }
        public int timestamp { get; set; }

        IDictionary<string, int> rates;
        public CurrencyDto()
        {
            rates = new Dictionary<string, int>();
        }
    }
}
