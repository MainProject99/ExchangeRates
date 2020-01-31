using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services;
using DataAccessLayer;
using DataAccessLayer.IRepositories;
using DataAnnotationsExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Model.Models;
using Moq;
using NUnit.Framework;

namespace BusinessLogicLayer.Test.ServiceTests
{
    [TestFixture]
    public class UserServiceTest : TestInitializer
   {
       private UserService userService;
       private Mock<IUserRepository> mockUserRepository;
       private Mock<ApplicationDbContext> mockDataBaseContext;
       private User user;

        [SetUp]
        protected override void Initialize()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockDataBaseContext = new Mock<ApplicationDbContext>();

            
            user = new User { Id = 1, Email = "tmikola11@gmail.com", Name = "Nick99", Password = "Nick" };
            
            userService = new UserService(mockUserRepository.Object,mockDataBaseContext.Object);
            TestContext.WriteLine("Overrided");
        }
        #region Exist of User
        [Test]
        public void Exist_CheckEmail_ReturnFalse()
        {
            mockUserRepository.Setup(x => x.Exist(It.Is<string>(y => y == "tmikola@gmail.com"))).Returns(false);
            var existsOfEmail = userService.Exist(user.Email);

            Assert.IsFalse(existsOfEmail);
        }
        [Test]
        public void Exist_CheckEmail_ReturnTrue()
        {
            // user = new User { Id = 1, Email = "tmikola11@gmail.com", Name = "Nick99", Password = "Nick" };
            mockUserRepository.Setup(x=>x.Exist(It.Is<string>(y => y == "tmikola11@gmail.com"))).Returns(true);

            userService = new UserService(mockUserRepository.Object, mockDataBaseContext.Object);
            var existsOfEmail = userService.Exist(user.Email);
            Assert.That(existsOfEmail, Is.EqualTo(true));
        }
        #endregion        
        [Test]
        public void CreateUser_User_Exception()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyRate")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options, ""))
            {
                context.Users.Add(new User() {Id = 1, Email = "tmikola11@gmail.com", Name = "Nick99", Password = "Nick"});
                context.SaveChanges();

            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options, ""))
            {
                var userService = new UserService(mockUserRepository.Object, context);

                Assert.Throws<AppException>(()=> userService.CreateUser(user, "Nick"));
            }
        }
        [Test]
        public void CreateUser_User_CreateUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyRate")
                .Options;           
            using (var context = new ApplicationDbContext(options, ""))
            {
                var userService = new UserService(mockUserRepository.Object, context);
                var result = userService.CreateUser(user, "Nick");
                Assert.That(result, Is.Not.Null);
            }
        }

        [Test]
        public void Authenticate_User_Null()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyRate")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options, ""))
            {
                context.Users.Add(new User() { Id = 1, Email = "tmikola11@gmail.com", Name = "Nick99", Password = "Nick" });
                context.SaveChanges();

            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options, ""))
            {
                var userService = new UserService(mockUserRepository.Object, context);
                var result = userService.Authenticate("", "");
                Assert.That(result, Is.Null);
            }

        }
        [Test]
        public void Authenticate_User_AuthorithAndGetToken()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyRate")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options, ""))
            {
                context.Users.Add(new User() { Id = 1, Email = "tmikokla11@gmail.com", Name = "Nick99", Password = "Nick" });
                context.SaveChanges();

            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options, ""))
            {

                var userService = new UserService(mockUserRepository.Object, context);

                Assert.Throws<NullReferenceException>(() => userService.Authenticate("tmikola11@gmail.com", "Nick"));

            }

        }
        [Test]
        public void Update_User_Null()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyRate")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options, ""))
            {
                context.Users.Add(new User() { Id = 2, Email = "tmikola11@gmail.com", Name = "Nick99", Password = "Nick" });
                context.SaveChanges();

            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options, ""))
            {
                mockUserRepository.Setup(x => x.GetById(It.Is<int>(y => y == 1))).Returns(user);

                var userService = new UserService(mockUserRepository.Object, context);
                userService.UpdateUser(user, "Nick");
                Assert.IsNull(null);

            }

        }
    }
}
