using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services;
using DataAccessLayer.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Model.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Test
{
    [TestFixture]
    public class CurrencyServiceTest : TestInitializer
    {

        private CurrencyService currencyService;
        private Currency currency;
        private Mock<ICurrencyRepository> mockCurrencyRepository;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IOptions<CurrencySettings>> currencySettingsOptions;
        private Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private const string URL = "https://api.iban.com/clients/api/currency/convert/";

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            mockCurrencyRepository = new Mock<ICurrencyRepository>();
            mockUserRepository = new Mock<IUserRepository>();
            CurrencySettings currencySettings = new CurrencySettings()
            { CurrencyFrom = "Usd", CurrencyTo = "EUR", ApiKey = "116677a09fe37ba01ebe3e35688ab41c" };
            currencySettingsOptions = new Mock<IOptions<CurrencySettings>>();
            // We need to set the Value of IOptions to be the SampleOptions Class
            currencySettingsOptions.Setup(ap => ap.Value).Returns(currencySettings);

            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
               .Returns(new Claim("Id", "1"));

            currencyService = new CurrencyService(mockCurrencyRepository.Object, mockMapper.Object,
                mockUserRepository.Object, currencySettingsOptions.Object, mockHttpContextAccessor.Object);
            currency = new Currency() { Id = 1, UserId = 1, CurrencyFrom = "USD", CurrencyTo = "EUR" };

            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void GetCurrencyDefaultInfo_Currency_DefaultCurrency()
        {

            var currencyDefaultInfoDTO = new CurrencyDefaultInfoDTO();

            mockCurrencyRepository.Setup(m => m.GetById(1)).Returns(currency);
            mockMapper.Setup(m => m.Map<CurrencyDefaultInfoDTO>(It.Is<Currency>(x => x == currency)))
                .Returns(currencyDefaultInfoDTO);

            var result = currencyService.GetCurrencyDefaultInfo();
            Assert.AreEqual(currencyDefaultInfoDTO, result);
        }
        [Test]
        public void GetCurrencyDefaultInfo_Currency_Null()
        {
            var currency = new Currency();
            var currencyDefaultInfoDTO = new CurrencyDefaultInfoDTO();

            mockCurrencyRepository.Setup(m => m.GetById(0)).Returns(currency);
            mockMapper.Setup(m => m.Map<CurrencyDefaultInfoDTO>(It.Is<Currency>(x => x == currency)))
                .Returns(currencyDefaultInfoDTO);

            var result = currencyService.GetCurrencyDefaultInfo();
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task PostClienConverterAsync_Currency_CurrencyResponceDto()
        {
            var currencyRequestDto = new CurrencyRequestDto() { from = "USD", to = "EUR", amount = 120 };                   
            
            mockCurrencyRepository.Setup(m => m.GetById(1)).Returns(currency);
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer","Token");
            var expectedUri = new Uri(URL + $"{currencyRequestDto.amount}/{currencyRequestDto.from }/{currencyRequestDto.to}");

            HttpResponseMessage resulPost = await httpClient.PostAsync(expectedUri, null);
            HttpContent content = resulPost.Content;
            string mycontent = await content.ReadAsStringAsync();

            var resulThatExpected = JsonConvert.DeserializeObject<CurrencyResponceDto>(mycontent);
            var resultCheck = resulThatExpected as CurrencyResponceDto;

            var result = currencyService.PostClienConverterAsync(currencyRequestDto);
 
            Assert.That(resulThatExpected, Is.Not.Null);
            Assert.NotNull(result);


        }
        
    }
}

