using CurrencyAPI.ApiDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAPI.Intefaces
{
    public interface ICurrencyConvertAPI
    {
        Task<RatesResponseDTO> GetCurrencyAPI();
        Task<CurrencyResponceDto> ConvertCurrencies(CurrencyRequestDto currencyRequestDto);
    }
}
