using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RESTfulAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private ICurrencyService currencyService;
        public CurrencyConverterController(ICurrencyService _currencyService)
        {
            currencyService = _currencyService;       
        }
        //[HttpPost]
        //public async Task ConvertData(string from, string to, double amount)
        //{
        //    var result = currencyService.PostClienConverterAsync(from, to, amount);
        //    //return await result;
        //}

        
        [HttpPost]
        public async Task<CurrencyResponceDto> TrialPostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
        {
            var result = currencyService.TrialPostClienConverterAsync(currencyRequestDto);
            return await result;
            //return await result;
        }
    }
}
