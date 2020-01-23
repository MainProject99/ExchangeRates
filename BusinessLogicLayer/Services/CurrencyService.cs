using BusinessLogicLayer.Interfaces;
using DataAccessLayer.IRepositories;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using Model.Models;
using BusinessLogicLayer.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BusinessLogicLayer.Helpers;
using Microsoft.Extensions.Options;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// Service for Currency, include all methods needed to work with Currency storage.
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        
        static readonly HttpClient client = new HttpClient();
        private readonly ICurrencyRepository currencyRepository;
        private readonly IUserRepository userRepository;
        IHttpContextAccessor httpContextAccessor;
        private IMapper mapper;
        private readonly CurrencySettings currencySettings;

        private const string URL = "https://api.iban.com/clients/api/currency/convert/";

        public CurrencyService(ICurrencyRepository _currency, IMapper _mapper, IUserRepository _userRepository,
                  IOptions<CurrencySettings> _currencySettings, IHttpContextAccessor _httpContextAccessor)
        {
            userRepository = _userRepository;
            currencyRepository = _currency;
            currencySettings = _currencySettings.Value ?? throw new ArgumentException(nameof(_currencySettings)); ;
            mapper = _mapper;
            httpContextAccessor = _httpContextAccessor;
        }

        /// <summary>
        /// Method for getting Currencies and User Id from DB
        /// /// </summary>
        /// <returns>Default CurrencyTo and Currency from</returns>
        public CurrencyDefaultInfoDTO GetCurrencyDefaultInfo()
        {
           
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                if (userId == null)
                {
                    throw new AppException($"Current user equal {userId} !");
                }
                var currentUserInfo = currencyRepository.GetById(Int16.Parse(userId));

                return mapper.Map<CurrencyDefaultInfoDTO>(currentUserInfo);
            

        }


        /// <summary>
        /// Transform CurrencyRequestDto <paramref name="currencyRequestDto"/> to Currency and insert it to Currency table of DB,
        ///if the the current doesn't have correct data, It'll add data from secrets.json 
        ///This method convert value, and convert result to string
        /// </summary>
        /// <param name="currencyRequestDto">
        /// Should contain not null or empty data .
        /// </param>
        /// <returns>Converted data</returns>

        public async Task<CurrencyResponceDto> PostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            if (userId == null) 
            { 
                throw new AppException($"Current user equal {userId} !");
            }
            var currentId = int.Parse(userId);
            var defaltCurrency = currencyRepository.GetById(currentId);
            Currency currency = new Currency();

            if (defaltCurrency == null)
            {

                
                if (!string.IsNullOrEmpty(userId))
                {
                    currency.UserId = currentId;
                }

                if (!string.IsNullOrEmpty(currencyRequestDto.from))
                {
                    currency.CurrencyFrom = currencyRequestDto.from;
                }
                else 
                {
                    //Set default currency if currency is incorrect
                    currency.CurrencyFrom = currencySettings.CurrencyFrom;
                }

                if (!string.IsNullOrEmpty(currencyRequestDto.to))
                {
                    currency.CurrencyTo = currencyRequestDto.to;
                }
                else
                {
                    currency.CurrencyTo = currencySettings.CurrencyTo;
                }
                
                currencyRepository.Insert(currency);
                
            }
            else
            {

                if (!string.IsNullOrEmpty(defaltCurrency.CurrencyFrom))
                {
                    defaltCurrency.CurrencyFrom = currencyRequestDto.from;
                }
                else
                {
                    //Set default currency if currency is incorrect
                    currency.CurrencyFrom = currencySettings.CurrencyFrom;
                }
                if (!string.IsNullOrEmpty(defaltCurrency.CurrencyTo))
                {
                    defaltCurrency.CurrencyTo = currencyRequestDto.to;
                }
                else
                {
                    currency.CurrencyTo = currencySettings.CurrencyTo;
                }

                //Update default currency
                currencyRepository.Update(defaltCurrency);
            }



            var urlConverter = new Uri(URL + $"?api_key={currencySettings.ApiKey}&format={currencyRequestDto.format}&from={currencyRequestDto.from }&to={currencyRequestDto.to}&amount={currencyRequestDto.amount}");

            using (HttpResponseMessage response = await client.PostAsync(urlConverter, null))
            {
                CurrencyResponceDto currencyResponceDto = null;
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();

                    currencyResponceDto = JsonConvert.DeserializeObject<CurrencyResponceDto>(mycontent);
                    #region Translate Number to Words

                    if (currencyRequestDto.numberToLanguage == NumberToLanguageEnum.Engl)
                    {
                        currencyResponceDto.numberInString = NumberToWordsService.ConvertAmountToEng(currencyResponceDto.convert_result);
                    }
                    else if (currencyRequestDto.numberToLanguage == NumberToLanguageEnum.Ukr)
                    {
                        currencyResponceDto.numberInString = NumberToWordsService.ConvertAmountToUkr(currencyResponceDto.convert_result);
                    }
                    #endregion
                }


                return currencyResponceDto;
            }
        }

    }
}

