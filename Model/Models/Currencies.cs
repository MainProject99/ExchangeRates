using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class Currencies
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User User { get; set; }

        public ICollection <CurrencyFrom> CurrencyFrom { get; set; }

        public Currencies () 
        {
            CurrencyFrom = new List<CurrencyFrom>();
        }
    }
}
