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
using CurrencyAPI.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using BusinessLogicLayer.Hubs;
using BusinessLogicLayer.Helpers;
using CurrencyAPI.Intefaces;
using CurrencyAPI.ApiDTO;
using CurrencyAPI;

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
        private readonly ICurrencyConvertAPI currencyConvertAPI;
        IHttpContextAccessor httpContextAccessor;
        private IMapper mapper;
        private readonly CurrencySettings currencySettings;
        protected readonly IHubContext<CurencyRateHub> hubCurrencyRateContext;
        public CurrencyService(ICurrencyRepository _currency, IMapper _mapper, IUserRepository _userRepository,
                  IOptions<CurrencySettings> _currencySettings, IHttpContextAccessor _httpContextAccessor, 
                  IHubContext<CurencyRateHub> _hubCurrencyRateContext, ICurrencyConvertAPI _currencyConvertAPI)
        {
            userRepository = _userRepository;
            currencyRepository = _currency;
            currencySettings = _currencySettings.Value ?? throw new ArgumentException(nameof(_currencySettings));
            mapper = _mapper;
            httpContextAccessor = _httpContextAccessor;
            hubCurrencyRateContext = _hubCurrencyRateContext;
            currencyConvertAPI = _currencyConvertAPI;
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
        /// Method for getting Currencies and value
        /// /// </summary>
        /// <returns>RatesResponseDTO</returns>
        public async Task<RatesResponseDTO> GetCurencies()
        {
            var ratesResponseDTO = await currencyConvertAPI.GetCurrencyAPI();
                await hubCurrencyRateContext.Clients.All.SendAsync("UpdateRates",ratesResponseDTO.rates.Keys, ratesResponseDTO.rates.Values);
                
            return ratesResponseDTO;
            
        }
        /// <summary>
        /// Transform CurrencyRequestDto <paramref name="currencyRequestDto"/> to Currency and insert it to Currency table of DB,
        ///if the the current doesn't have correct data, It'll add data from secrets.json 
        ///This method convert value, and convert result to string
        /// </summary>
        /// <param name="currencyRequestDto">
        /// Should contain not null or empty data.
        /// </param>
        /// <returns>Converted data</returns>

        public async Task<CurrencyResponceDto> ConvertCurrency(CurrencyRequestDto currencyRequestDto)
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

                var CurrencyFrom = !string.IsNullOrEmpty(currencyRequestDto.from) ? currency.CurrencyFrom = currencyRequestDto.from : currency.CurrencyFrom = currencySettings.CurrencyFrom;

                var CurrencyTo = !string.IsNullOrEmpty(currencyRequestDto.to) ? currency.CurrencyTo = currencyRequestDto.to : currency.CurrencyTo = currencySettings.CurrencyTo;
                                
                currencyRepository.Insert(currency);
                
            }
            else
            {
                var CurrencyFromDefault = !string.IsNullOrEmpty(defaltCurrency.CurrencyFrom) ? defaltCurrency.CurrencyFrom = currencyRequestDto.from : currency.CurrencyFrom = currencySettings.CurrencyFrom;
                
                var CurrencyToDefault = !string.IsNullOrEmpty(defaltCurrency.CurrencyTo) ? defaltCurrency.CurrencyTo = currencyRequestDto.to : currency.CurrencyTo = currencySettings.CurrencyTo;
                

                //Update default currency
                currencyRepository.Update(defaltCurrency);
            }

            var currencyAmountResult = await currencyConvertAPI.ConvertCurrencies(currencyRequestDto);


            if (currencyRequestDto.numberToLanguage == NumberToLanguageEnum.Engl)
            {
                currencyAmountResult.numberInString = NumberToWordsService.ConvertAmountToEng(currencyAmountResult.amount);
                SynthesisWordToAudioFileService.SynthesisToAudioFileAsync(currencyAmountResult.numberInString);
            }
            else if (currencyRequestDto.numberToLanguage == NumberToLanguageEnum.Ukr)
            {
                currencyAmountResult.numberInString = NumberToWordsService.ConvertAmountToUkr(currencyAmountResult.amount);
            }

            return currencyAmountResult;
            
        }

    }
}

