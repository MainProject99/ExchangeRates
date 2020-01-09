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
                }
                //var map = mapper.Map<CurrencyResponceDto,CurrencyRequestDto>(currencyResponceDto);
                return currencyResponceDto;
                }            
            }

        public static string NumberToWords(int number)
        {
            if (number == 0) { return "zero"; }
            if (number < 0) { return "minus " + NumberToWords(Math.Abs(number)); }
            string words = "";
            if ((number / 10000000) > 0) { words += NumberToWords(number / 10000000) + " Crore "; number %= 10000000; }
            if ((number / 100000) > 0) { words += NumberToWords(number / 100000) + " Lakh "; number %= 100000; }
            if ((number / 1000) > 0) { words += NumberToWords(number / 1000) + " Thousand "; number %= 1000; }
            if ((number / 100) > 0) { words += NumberToWords(number / 100) + " Hundred "; number %= 100; }
            if (number > 0)
            {
                if (words != "") { words += "and "; }
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "seventy", "Eighty", "Ninety" };
                if (number < 20) { words += unitsMap[number]; }
                else { words += tensMap[number / 10]; if ((number % 10) > 0) { words += "-" + unitsMap[number % 10]; } }
            }
            return words;
        }
    }
}
    
