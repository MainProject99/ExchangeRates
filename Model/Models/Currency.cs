using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyTo { get; set; }
        public string CurrencyFrom { get; set; }

        //[ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
        public Currency() 
        { 
        
        }

       
    }
}
