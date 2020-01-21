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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private ICurrencyService currencyService;
        public CurrencyConverterController(ICurrencyService _currencyService)
        {
            currencyService = _currencyService;       
        }
        /// <summary>
        /// This method allows register user
        /// </summary>
        /// <param name="currencyRequestDto">Required</param>
        /// <returns>CurrencyResponceDto</returns>
        /// <response code="200">Model valid</response> 
        /// <response code="400">If Convert process failed</response>
        [HttpPost("PostToConvert")]
        public async Task<CurrencyResponceDto> PostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
        {
            var result = currencyService.PostClienConverterAsync(currencyRequestDto);
            return await result;
        
        }

        /// <summary>
        /// Method for getting default currency of current user
        /// </summary>
        /// <returns>Default currencies.</returns>
        [HttpGet("GetCurrencyInfo")]
        public IActionResult GetCurrencyInfo()
        {
            var result = currencyService.GetCurrencyDefaultInfo();

            return Ok(result);
        }
    }
}
