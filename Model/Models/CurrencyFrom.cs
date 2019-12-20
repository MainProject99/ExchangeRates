using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class CurrencyFrom
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Currency Currency { get; set; }
    }
}
