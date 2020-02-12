using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Models
{
    public class Currency
    {
        /// <summary>The Id of the Currency</summary>
        public int Id { get; set; }
        /// <summary>The Currency to wich we want to translate money</summary>
        public string CurrencyTo { get; set; }

        /// <summary>The Currency from wich we want to translate money</summary>
        public string CurrencyFrom { get; set; }

        /// <summary>The User Id is relationship with One-to-One with Currency</summary>
        public int UserId { get; set; }

        public User User { get; set; }
        public Currency() 
        { 
        
        }

       
    }
}
