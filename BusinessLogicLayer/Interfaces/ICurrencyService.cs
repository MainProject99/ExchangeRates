using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyResponceDto> PostClienConverterAsync(string from, string to, double amount);
    }
}
