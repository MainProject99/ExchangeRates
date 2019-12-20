using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyTo { get; set; }

        public User User { get; set; }

        public ICollection <CurrencyFrom> CurrencyFroms { get; set; }

        public Currency ()
        {
            CurrencyFroms = new List<CurrencyFrom>();
        }
    }
}
