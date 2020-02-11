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
        /// This method allows to Convert currency
        /// </summary>
        /// <param name="currencyRequestDto">Required</param>
        /// <returns>CurrencyResponceDto</returns>
        /// <response code="200"></response> 
        /// <response code="400">If Convert process failed</response>
        [HttpPost("PostToConvert")]
        public async Task<CurrencyResponceDto> PostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
        {
            var result = currencyService.ConvertCurrency(currencyRequestDto);
            return await result;
        
        }

        /// <summary>
        /// Method for getting currency
        /// </summary>
        /// <returns>Currencies.</returns>
        ///<response code="200">Client’s request was accepted successfully</response> 
        ///<response code="404">There isn't default currency of current user</response> 

        [HttpGet("GetCurrency")]
        public async Task<RatesResponseDTO> GetCurrency()
        {
            var result =  await currencyService.GetCurencies();

            return result;
        }
        /// <summary>
        /// Method for getting default currency of current user
        /// </summary>
        /// <returns>Default currencies.</returns>
        ///<response code="200">Client’s request was accepted successfully</response> 
        ///<response code="404">There isn't default currency of current user</response> 

        [HttpGet("GetDefaultCurrencyInfo")]
        public IActionResult GetDefaultCurrencyInfo()
        {
            var result = currencyService.GetCurrencyDefaultInfo();
            if (result == null)
            {
                return new BadRequestResult();
            }

            return Ok(result);
        }

    }
}
