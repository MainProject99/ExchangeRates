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
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private ICurrencyService currencyService;
        public CurrencyConverterController(ICurrencyService _currencyService)
        {
            currencyService = _currencyService;       
        }
        
        [HttpPost("PostToConvert")]
        public async Task<CurrencyResponceDto> PostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
        {
            var result = currencyService.PostClienConverterAsync(currencyRequestDto);
            return await result;
        
        }
        [HttpGet]
        public async Task GetClient() 
        {
        
        }
    }
}
