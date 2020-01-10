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

namespace BusinessLogicLayer.Services
{

    public class CurrencyService : ICurrencyService
    {
        private const string URL = "https://api.iban.com/clients/api/currency/convert/";
        private string apiKey = "116677a09fe37ba01ebe3e35688ab41c";

        static readonly HttpClient client = new HttpClient();
        private readonly ICurrencyRepository currency;
        private IMapper mapper;
        public CurrencyService(ICurrencyRepository _currency, IMapper _mapper)
        {
            currency = _currency;
            mapper = _mapper;
        }

            public async Task<CurrencyResponceDto> PostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
            {
            //var json = JsonConvert.SerializeObject(currencyRequestDto);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Add an Accept header for JSON format. 
            //client.DefaultRequestHeaders.Accept.Add(
            //new MediaTypeWithQualityHeaderValue("application/json"));
            //urlConverter.
            //client.DefaultRequestHeaders.Add("api_key", apiKey);
            
            var urlConverter = new Uri(URL + $"?api_key={currencyRequestDto.api_key}&format={currencyRequestDto.format}&from={currencyRequestDto.from}&to={currencyRequestDto.to}&amount={currencyRequestDto.amount}");

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
                //var mapTo = mapper.Map<CurrencyResponceDto,Currencies>(currencyResponceDto);
                //var mapFrom = mapper.Map<CurrencyResponceDto,CurrencyFrom>(currencyResponceDto);

                return currencyResponceDto;
                }            
            }
    }
}
    
