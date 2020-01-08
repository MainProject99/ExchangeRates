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


        public async Task PostClienConverterAsync(string from, string to, float amount)
        {

            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("from", from));
            formData.Add(new KeyValuePair<string, string>("to", to));
            formData.Add(new KeyValuePair<string, string>("amount", $"{amount}"));

            var encodedContent = new FormUrlEncodedContent(formData);



            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Add("api_key", apiKey);

            using (HttpResponseMessage response = await client.PostAsync(URL, encodedContent))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                }
            }
            //var response = await client.PostAsync(URL, encodedContent);

            //string result = response.Content.ReadAsStringAsync().Result;

            //var map = mapper.Map<CurrencyResponceDto>(result);

            //return map;
        }

            public async Task<CurrencyResponceDto> TrialPostClienConverterAsync(CurrencyRequestDto currencyRequestDto)
            {
                //var json = "json=" + System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(currencyRequestDto));
                var json = JsonConvert.SerializeObject(currencyRequestDto);
                var data = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Add("api_key", apiKey);
            // List data response.
            using (HttpResponseMessage response = await client.PostAsync(new Uri(URL), data))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                    }
                    var map = mapper.Map<CurrencyResponceDto>(response);
                    return map;
                }            
            }




            //public void CreateClient()
            //{
            //    var request = (HttpWebRequest)WebRequest.Create("https://api.iban.com/clients/api/currency/rates/");

            //    var postData = "api_key=[YOUR_API_KEY]";
            //        postData += "&format=json";
            //        postData += "¤cy=USD";

            //    var data = Encoding.ASCII.GetBytes(postData);

            //    request.Method = "POST";
            //    request.ContentType = "application/x-www-form-urlencoded";
            //    request.ContentLength = data.Length;

            //    using (var stream = request.GetRequestStream())
            //    {
            //        stream.Write(data, 0, data.Length);
            //    }

            //    var response = (HttpWebResponse)request.GetResponse();

            //    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //}
            //    var request = (HttpWebRequest)WebRequest.Create("https://api.iban.com/clients/api/currency/convert/");

            //    var postData = "api_key=[YOUR_API_KEY]";
            //    postData += "&format=json";
            //         postData += "&from=USD";
            //         postData += "&to=EUR";
            //         postData += "&amount=120";

            //        var data = Encoding.ASCII.GetBytes(postData);

            //    request.Method = "POST";
            //        request.ContentType = "application/x-www-form-urlencoded";
            //        request.ContentLength = data.Length;

            //        using (var stream = request.GetRequestStream())
            //        {
            //         stream.Write(data, 0, data.Length);
            //        }

            //        var response = (HttpWebResponse)request.GetResponse();

            //       var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //




           
        
    }
}
    
