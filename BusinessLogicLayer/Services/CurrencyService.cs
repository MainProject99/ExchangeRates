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
        private string urlParameters = "?api_key=";

        static readonly HttpClient client = new HttpClient();
        private readonly ICurrencyRepository currency;
        private IMapper mapper;
        public CurrencyService(ICurrencyRepository _currency, IMapper _mapper)
        {
            currency = _currency;
            mapper = _mapper;
        }


        public async Task<CurrencyResponceDto> PostClienConverterAsync(string from, string to, double amount)
        {
            var currencyRequestDto = new CurrencyRequestDto();
            currencyRequestDto.from = $"&from={from}";
            currencyRequestDto.to = $"&to={to}";
            currencyRequestDto.amount = $"&amount={amount}";
            currencyRequestDto.format = $"&format=json";

            var json = JsonConvert.SerializeObject(currencyRequestDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(URL, data);

            string result = response.Content.ReadAsStringAsync().Result;

            var map = mapper.Map<CurrencyResponceDto>(result);

            return map;
        }

        public async Task<List<CurrencyRequestDto>> ClienConverterAsync(string from, string to, double amount)
        {
            List<CurrencyRequestDto> currencyRequest = null;
            client.BaseAddress = new Uri(URL);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {

                // Parse the response body.
                string responseBody = await response.Content.ReadAsStringAsync();
                currencyRequest = JsonConvert.DeserializeObject<List<CurrencyRequestDto>>(responseBody);

            }
            return currencyRequest;
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




            //public async Task<List<CurrencyRequestDto>> MethodAsync()
            //{
            //    List<CurrencyRequestDto> contributors = null;
            //    var url = "https://api.iban.com/clients/api/currency/rates/";


            //        client.DefaultRequestHeaders.Accept.Add(
            //        new MediaTypeWithQualityHeaderValue("application/json"));

            //        HttpResponseMessage response = await client.GetAsync(url);
            //        //    .ContinueWith((taskwithresponse) =>
            //        //{
            //        //    var response = taskwithresponse.Result;
            //        //    var jsonString = response.Content.ReadAsStringAsync();
            //        //    jsonString.Wait();
            //        //    model = JsonConvert.DeserializeObject<Currencies>(jsonString.Result);
            //        //});
            //        //task.Wait(); ;
            //        response.EnsureSuccessStatusCode();


            //        string responseBody = await response.Content.ReadAsStringAsync();

            //        contributors = JsonConvert.DeserializeObject<List<CurrencyRequestDto>>(responseBody);



            //    //}
            //    //catch (HttpRequestException e)
            //    //{
            //        // Console.WriteLine("\nException Caught!");
            //        //Console.WriteLine("Message :{0} ", e.Message);
            //    //}
            //     return contributors;
            //}
        } 
}
    
