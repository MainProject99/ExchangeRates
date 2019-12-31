using BLL.Interfaces;
using DLL.IRepositories;
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

namespace BLL.Services
{

    public class CurrencyService : ICurrencyService
    {
        static readonly HttpClient client = new HttpClient();
        private readonly ICurrencyRepository currency;
        public CurrencyService(ICurrencyRepository _currency)
        {
            _currency = currency;
        }
        public void CreateClient()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.iban.com/clients/api/currency/rates/");

            var postData = "api_key=[YOUR_API_KEY]";
                postData += "&format=json";
                postData += "¤cy=USD";

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        }





        static async Task MethodAsync()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.iban.com/clients/api/currency/rates/");
                response.EnsureSuccessStatusCode();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                // Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                // Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    } 
}
    
