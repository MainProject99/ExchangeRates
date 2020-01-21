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

        public CurrencyDefaultInfoDTO GetCurrencyDefaultInfo()
        {
            try
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                var currentUserInfo = currencyRepository.GetById(Int16.Parse(userId));

                return mapper.Map<CurrencyDefaultInfoDTO>(currentUserInfo);

            }
            catch (Exception e)
            {

                throw e;
            }

        }

       

        public async Task<CurrencyResponceDto> PostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var currentId = int.Parse(userId);
            var defaltCurrency = currencyRepository.GetById(currentId);

            if (defaltCurrency == null)
            {
                Currency currency = new Currency();
                 if (!string.IsNullOrEmpty(userId))
                    currency.UserId = currentId;

                if (!string.IsNullOrEmpty(currencyRequestDto.from))
                    currency.CurrencyFrom = currencyRequestDto.from;

                if (!string.IsNullOrEmpty(currencyRequestDto.to))
                    currency.CurrencyTo = currencyRequestDto.to;
                currencyRepository.Insert(currency);
                
            }
            else
            {

                if (!string.IsNullOrEmpty(defaltCurrency.CurrencyFrom))
                    defaltCurrency.CurrencyFrom = currencyRequestDto.from;

                if (!string.IsNullOrEmpty(defaltCurrency.CurrencyTo))
                    defaltCurrency.CurrencyTo = currencyRequestDto.to;

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

