using BusinessLogicLayer.DTO;
using CurrencyAPI.ApiDTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICurrencyService
    {
        CurrencyDefaultInfoDTO GetCurrencyDefaultInfo();
        Task<CurrencyAPI.ApiDTO.RatesResponseDTO> GetCurencies();
        Task<CurrencyResponceDto> ConvertCurrency(CurrencyRequestDto currencyRequestDto);        
    }
}
